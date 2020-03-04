using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour, IDamageEntity, IDamageable, IParryable
{
    public bool lockOn;
    public bool isOnAir;
    public bool isGrounded;
    public bool isRolling;
    public bool isInteracting;
    public bool isSprinting;

    [Header("Controller")]
    public float movementSpeed = 3;
    public float sprintingSpeed = 3;
    public float rotationSpeed = 10;
    public float attackRotationSpeed = 3;
    public float groundDownDistanceOnAir = .4f;
    public float groundedSpeed = 0.1f;
    public float groundedDistanceRay = .5f;
    float velocityMultiplier = 1;

    Animator anim;
    new Rigidbody rigidbody;

    [HideInInspector]
    public Transform currentLockTarget;

    [HideInInspector]
    public Transform mTransform;

    LayerMask ignoreForGroundCheck;

    public List<ArmorItem> startingArmor;
    public ItemActionContainer[] currentActions;
    //public ItemActionContainer[] defaultActions;
    ItemActionContainer currentAction;
    InventoryManager_temp _inventoryManager;
    public InventoryManager_temp inventoryManager {
        get {
            return _inventoryManager;
        }
    }
    ArmorManager armorManager;
    [HideInInspector]
    public AnimatorHook animatorHook;
    Vector3 currentNormal;

    Collider controllerCollider;
    public GameObject parryCollider;
    //public GameObject unarmedCollider;

    ActionContainer _lastAction;

    public int health = 100; //TODO: Hook this with other stuff.

    public ActionContainer lastAction {
        get {
            if (_lastAction == null)
            {
                _lastAction = new ActionContainer();
            }

            _lastAction.owner = mTransform; //For directional attacks
            _lastAction.damage = currentAction.damage;
            _lastAction.overrideReactAnim = currentAction.overrideReactAnim;
            _lastAction.reactAnim = currentAction.reactAnim;

            return _lastAction;
        }
    }


    public void InitInventory(PlayerProfile profile)
    {
        _inventoryManager.Init(profile, this);
        //LoadWeapon(rh, false);
        //LoadWeapon(lh, true);
    }

    public void Init()
    {
        mTransform = this.transform;
        rigidbody = GetComponentInChildren<Rigidbody>();
        controllerCollider = GetComponent<Collider>();

        anim = GetComponentInChildren<Animator>();
        _inventoryManager = GetComponent<InventoryManager_temp>();
        animatorHook = GetComponentInChildren<AnimatorHook>();
        armorManager = GetComponent<ArmorManager>();


        ResetCurrentActions();

        currentPosition = mTransform.position;
        ignoreForGroundCheck = ~(1 << 9 | 1 << 10 | 1 << 12);
    }

    public void InitArmor()
    {
        armorManager.Init();
        armorManager.LoadListOfItems(startingArmor);
    }

    private void Update()
    {
        isInteracting = anim.GetBool("isInteracting");

        if (animatorHook.canDoCombo)
        {
            if (!isInteracting)
            {
                animatorHook.canDoCombo = false;
            }
        }

        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer < 0)
            {
                isHit = false;
            }
        }
    }

    #region Movement
    public void HandleCombo()
    {

    }

    Vector3 currentPosition;

    public void MoveCharacter(float vertical, float horizontal, Vector3 moveDirection, float delta)
    {
        CheckGround();
        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        HandleDamageCollider();

        //HANDLE ROTATION
        if (!isInteracting || animatorHook.canRotate)
        {
            Vector3 rotationDir = moveDirection;

            if (lockOn && !isSprinting)
            {
                rotationDir = currentLockTarget.position - mTransform.position;
            }

            HandleRotation(moveAmount, rotationDir, delta);
        }

        Vector3 targetVelocity = Vector3.zero;

        if (lockOn && !isSprinting)
        {
            targetVelocity = mTransform.forward * vertical * movementSpeed;
            targetVelocity += mTransform.right * horizontal * movementSpeed;
        }
        else
        {
            float speed = movementSpeed;
            if (isSprinting)
            {
                speed = sprintingSpeed;
            }

            targetVelocity = moveDirection * speed;
        }

        if (isInteracting)
        {
            targetVelocity = animatorHook.deltaPosition * velocityMultiplier;
        }
        else
        {
            controllerCollider.isTrigger = false;
        }

        //HANDLE MOVEMENT
        if (isGrounded)
        {
            targetVelocity = Vector3.ProjectOnPlane(targetVelocity, currentNormal);
            rigidbody.velocity = targetVelocity;

            Vector3 groundedPosition = mTransform.position;
            groundedPosition.y = currentPosition.y;
            mTransform.position = Vector3.Lerp(mTransform.position, groundedPosition, delta / groundedSpeed);
        }

        HandleAnimations(vertical, horizontal, moveAmount, delta);
    }

    void CheckGround()
    {
        RaycastHit hit;
        Vector3 origin = mTransform.position;
        origin.y += .5f;

        float dis = groundedDistanceRay;
        if (isOnAir)
        {
            dis = groundDownDistanceOnAir;
        }

        Debug.DrawRay(origin, Vector3.down * dis, Color.red);
        if (Physics.SphereCast(origin, .2f, Vector3.down, out hit, dis, ignoreForGroundCheck))
        {
            isGrounded = true;
            currentPosition = hit.point;
            currentNormal = hit.normal;

            float angle = Vector3.Angle(Vector3.up, currentNormal);
            if (angle > 45)
            {
                isGrounded = false;
            }

            if (isOnAir)
            {
                isOnAir = false;
                PlayTargetAnimation("Empty", false, false);
            }
        }
        else
        {
            if (isGrounded)
            {
                isGrounded = false;
            }

            if (isOnAir == false)
            {
                isOnAir = true;
                PlayTargetAnimation("OnAir", true, false);
            }

        }
    }

    void HandleDamageCollider()
    {
        if (currentAction != null)
        {
            if (currentAction.weaponHook != null)
            {
                currentAction.weaponHook.DamageColliderStatus(animatorHook.openDamageCollider);
            }
        }
    }

    void HandleRotation(float moveAmount, Vector3 targetDir, float delta)
    {
        float moveOverride = moveAmount;
        if (lockOn)
        {
            moveOverride = 1;
        }

        targetDir.Normalize();
        targetDir.y = 0;
        if (targetDir == Vector3.zero)
            targetDir = mTransform.forward;

        float actualRotationSpeed = rotationSpeed;
        if (isInteracting)
            actualRotationSpeed = attackRotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(
            mTransform.rotation, tr,
            delta * moveOverride * actualRotationSpeed);

        mTransform.rotation = targetRotation;
    }

    public void HandleAnimations(float vertical, float horizontal, float moveAmount, float delta)
    {
        if (isGrounded)
        {
            anim.SetBool("isSprinting", isSprinting);

            if (lockOn && !isSprinting)
            {
                float v = Mathf.Abs(vertical);
                float f = 0;
                if (v > 0 && v <= .5f)
                    f = .5f;
                else if (v > 0.5f)
                    f = 1;

                if (vertical < 0)
                    f = -f;

                anim.SetFloat("forward", f, .2f, delta);

                float h = Mathf.Abs(horizontal);
                float s = 0;
                if (h > 0 && h <= .5f)
                    s = .5f;
                else if (h > 0.5f)
                    s = 1;

                if (horizontal < 0)
                    s = -1;

                anim.SetFloat("sideways", s, .2f, delta);
            }
            else
            {

                float m = moveAmount;
                float f = 0;
                if (m > 0 && m <= .5f)
                    f = .5f;
                else if (m > 0.5f)
                    f = 1;

                anim.SetFloat("forward", f, .02f, Time.deltaTime);
                anim.SetFloat("sideways", 0, 0.2f, delta);
            }
        }
        else
        {
        }
    }

    #endregion

    #region Items & Actions
    void ResetCurrentActions()
    {
        currentActions = new ItemActionContainer[4];

        for (int i = 0; i < 4; i++)
        {
            currentActions[i] = new ItemActionContainer();
            currentActions[i].attackInput = (AttackInputs)i;
            currentActions[i].isMirrored = (currentActions[i].attackInput == AttackInputs.lt
                || currentActions[i].attackInput == AttackInputs.lb);
        }
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool isMirror = false, float velocityMultiplier = 1)
    {
        anim.SetBool("isMirror", isMirror);
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
        this.isInteracting = isInteracting;
        this.velocityMultiplier = velocityMultiplier;
    }

    public void PlayTargetItemAction(AttackInputs attackInput)
    {
        animatorHook.canRotate = false;
        currentAction = GetItemActionContainer(attackInput, currentActions);

        if (currentAction.canBackstab || currentAction.canParry)
        {
            RaycastHit hit;
            Vector3 origin = mTransform.position;
            origin.y += 1f;

            Vector3 dir = mTransform.forward;
            Debug.DrawRay(origin, dir * 2f, Color.red, 2, false);

            if (Physics.SphereCast(origin, 0.5f, dir, out hit, 2f))
            {
                IParryable parryable = hit.transform.GetComponentInParent<IParryable>();
                if (parryable != null)
                {
                    Transform enTransform = parryable.getTransform();

                    if (parryable.canBeParried())
                    {
                        float angle = Vector3.Angle(-enTransform.forward, mTransform.forward);

                        if (angle < 45f)
                        {
                            PlayTargetAnimation("Parry Attack", true, currentAction.isMirrored);
                            parryable.GetParried(mTransform.position, mTransform.forward);

                            return;
                        }
                    }
                    else if (parryable.canBeBackstabbed())
                    {
                        float angle = Vector3.Angle(enTransform.forward, mTransform.forward);

                        if (angle < 45f)
                        {
                            PlayTargetAnimation("Parry Attack", true, currentAction.isMirrored);
                            parryable.GetBackstabbed(mTransform.position, mTransform.forward);

                            return;
                        }
                    }
                }
            }

        }

        if (!string.IsNullOrEmpty(currentAction.animName))
        {
            PlayTargetAnimation(currentAction.animName, true, currentAction.isMirrored);
        }
    }

    protected ItemActionContainer GetItemActionContainer(AttackInputs ai, ItemActionContainer[] l)
    {
        if (l == null)
            return null;

        for (int i = 0; i < l.Length; i++)
        {
            if (l[i].attackInput == ai)
            {
                return l[i];
            }
        }

        return null;
    }

    public void LoadWeapon(Item item, bool isLeft)
    {
        //if (!(item is WeaponItem))
        //    return;

        WeaponItem weaponItem = null;

        if (item is WeaponItem)
        {
            weaponItem = (WeaponItem)item;
        }

        WeaponHook weaponHook = _inventoryManager.LoadWeaponOnHook(weaponItem, isLeft);

        int steps = 2;

        if (isLeft)
        {
            if (inventoryManager.isRightEmpty)
            {
                steps += 2;
            }
        }
        else
        {
            if (inventoryManager.isLeftEmpty)
            {
                steps += 2;
            }
        }

        for (int i = 0; i < steps; i++)
        {
            if (i > weaponItem.itemActions.Length - 1)
            {
                break;
            }

            ItemActionContainer wa = weaponItem.itemActions[i];
            ItemActionContainer ic = GetItemActionContainer(GetAttackInput(wa.attackInput, isLeft), currentActions);
            ic.isMirrored = (isLeft);
            CopyItemActionContainer(wa, ic);
            ic.weaponHook = weaponHook;
        }
    }

    public void LoadArmor(Item item)
    {
        if (item is ArmorItem)
        {
            armorManager.LoadItem((ArmorItem)item);
        }
    }

    void CopyItemActionContainer(ItemActionContainer from, ItemActionContainer to)
    {
        to.animName = from.animName;
        to.itemActual = from.itemActual;
        to.canParry = from.canParry;
        to.reactAnim = from.reactAnim;
        to.damage = from.damage;
        to.overrideReactAnim = from.overrideReactAnim;
        to.canBackstab = from.canBackstab;
    }

    AttackInputs GetAttackInput(AttackInputs inp, bool isLeft)
    {
        if (!isLeft)
        {
            return inp;
        }
        else
        {
            switch (inp)
            {
                case AttackInputs.rb:
                    return AttackInputs.lb;
                case AttackInputs.lb:
                    return AttackInputs.rb;
                case AttackInputs.rt:
                    return AttackInputs.lt;
                case AttackInputs.lt:
                    return AttackInputs.rt;
                case AttackInputs.none:
                default:
                    return inp;
            }
        }
    }

    #region Combos
    private Combo[] combos;
    public void LoadCombos(Combo[] targetCombo)
    {
        combos = targetCombo;
    }

    public void DoCombo(AttackInputs inp)
    {
        Combo c = GetComboFromInp(inp);

        if (c == null)
        {
            return;
        }

        PlayTargetAnimation(c.animName, true, currentAction.isMirrored);//
        animatorHook.canDoCombo = false;
    }

    Combo GetComboFromInp(AttackInputs inp)
    {
        if (combos == null)
            return null;

        for (int i = 0; i < combos.Length; i++)
        {
            if (combos[i].inp == inp)
                return combos[i];
        }

        return null;
    }
    #endregion

    public ILockable FindLockableTarget()
    {
        Collider[] cols = Physics.OverlapSphere(mTransform.position, 20);
        for (int i = 0; i < cols.Length; i++)
        {
            ILockable lockable = cols[i].GetComponentInParent<ILockable>();
            if (lockable != null)
            {
                return lockable;
            }
        }

        return null;
    }

    public ActionContainer GetActionContainer()
    {
        return lastAction;
    }

    bool isHit;
    float hitTimer;

    public void OnInteractAnimation(string anim)
    {
        PlayTargetAnimation(anim, true, false);
        controllerCollider.isTrigger = true;
        isInteracting = true;
    }

    public void OnDamage(ActionContainer action)
    {
        if (action.owner == mTransform)
            return;

        if (controllerCollider.isTrigger)
            return;


        if (!isHit)
        {
            animatorHook.openDamageCollider = false;

            SoundManager.PlaySound(SoundManager.Sound.PlayerHit, mTransform.position);

            isHit = true;
            hitTimer = 1f;

            health -= action.damage; // TODO: Draw defensive stats here.
            //Debug.Log("Player received " + action.damage + " new health is " + health);

            if (health <= 0)
            {
                health = 0;
                //GAME OVER STUFF HERE
                Debug.Log("I DEAD");
                //PlayTargetAnimation("Death", true);           
                //animator.transform.parent = null; // in order for ragdoll to properly work
                //gameObject.SetActive(false); // could just destroy instead of disabling
            }
            else
            {
                Vector3 direction = action.owner.position - mTransform.position;
                float dot = Vector3.Dot(mTransform.forward, direction);

                if (action.overrideReactAnim)
                {
                    PlayTargetAnimation(action.reactAnim, true);
                }
                else
                {
                    if (dot > 0)
                    {
                        PlayTargetAnimation("Get Hit Front", true);
                    }
                    else
                    {
                        PlayTargetAnimation("Get Hit Back", true);
                    }
                }
            }
        }
    }
    #endregion

    int parryFrameCount;
    public void OpenParryCollider()
    {
        parryFrameCount = 0;
        parryCollider.SetActive(true);
    }

    private void LateUpdate()
    {
        if (parryCollider.activeSelf) //parry collider open for 4 frames
        {
            parryFrameCount++;
            if (parryFrameCount > 16f)
            {
                parryCollider.SetActive(false);

            }
        }
    }

    public void OnParried(Vector3 dir)
    {

    }

    public Transform getTransform()
    {
        return mTransform;
    }

    public void GetParried(Vector3 origin, Vector3 direction)
    {

    }

    public bool canBeParried()
    {
        return false;
    }

    public void GetBackstabbed(Vector3 origin, Vector3 direction)
    {

    }

    public bool canBeBackstabbed()
    {
        return false;
    }

    public void ConsumeItem()
    {
        inventoryManager.ConsumeItemActual();
    }
}

[System.Serializable]
public class Combo
{
    public string animName;
    public AttackInputs inp;
}