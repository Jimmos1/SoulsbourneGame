using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour, ILockable, IDamageable, IDamageEntity, IParryable
{
    public FastStats stats;
    new Rigidbody rigidbody;
    Animator animator;
    NavMeshAgent agent;
    AnimatorHook animatorHook;
    Transform mTransform;
    Vector3 lookPosition;

    //public int health = 100;

    public float fovRadius = 20;
    public float rotationSpeed = 1;
    public float moveSpeed = 2;
    public float recoveryTimer;
    public int hardcodeAction = -1;
    public bool isInInterruption;
    public bool isInSpecialState = false;
    public bool openToBackstab = true;
    LayerMask detectionLayer;

    Controller currentTarget;
    bool isInteracting = false;
    bool actionFlag;

    ActionSnapshot currentSnapshot;

    public ActionSnapshot[] actionSnapshots;

    public ActionSnapshot GetAction(float distance, float angle)
    {

        if (hardcodeAction != -1)
        {
            int index = hardcodeAction;
            hardcodeAction = -1;
            return actionSnapshots[index];
        }

        int maxScore = 0;
        for (int i = 0; i < actionSnapshots.Length; i++)
        {
            ActionSnapshot a = actionSnapshots[i];

            if (distance <= a.maxDist && distance >= a.minDist)
            {
                if (angle <= a.maxAngle && angle >= a.minAngle)
                {
                    maxScore += a.score;
                }
            }
        }

        int rand = Random.Range(0, maxScore + 1);
        int temp = 0;

        for (int i = 0; i < actionSnapshots.Length; i++)
        {
            ActionSnapshot a = actionSnapshots[i];

            if (a.score == 0)
                continue;

            if (distance <= a.maxDist && distance >= a.minDist)
            {
                if (angle <= a.maxAngle && angle >= a.minAngle)
                {
                    temp += a.score;
                    if (temp > rand)
                    {
                        return a;
                    }
                }
            }
        }

        return null;
    }

    private void Start()
    {
        detectionLayer = (1 << 8);
        mTransform = this.transform;
        rigidbody = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponentInChildren<NavMeshAgent>();
        rigidbody.isKinematic = false;
        animatorHook = GetComponentInChildren<AnimatorHook>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        isInInterruption = animator.GetBool("interrupted");
        isInteracting = animator.GetBool("isInteracting");

        if (isHit) // Invincibility
        {
            if (hitTimer > 0)
            {
                hitTimer -= delta;
            }
            else
            {
                isHit = false;
            }
        }

        if (currentTarget == null)
        {
            HandleDetection();
        }
        else
        {
            if (agent.isActiveAndEnabled)
            {
                agent.SetDestination(currentTarget.mTransform.position);
                //mTransform.position = agent.transform.position; // floaty patch :C needs fixing
            }

            Vector3 relativeDirection = mTransform.InverseTransformDirection(agent.desiredVelocity);
            relativeDirection.Normalize();

            if (!isInteracting)
            {
                openToBackstab = true;

                if (actionFlag)
                {
                    recoveryTimer -= delta;
                    if (recoveryTimer <= 0)
                    {
                        actionFlag = false;
                    }
                }

                animator.SetFloat("movement", relativeDirection.z, 0.1f, delta);

                Vector3 dir = currentTarget.mTransform.position - mTransform.position;
                dir.y = 0;
                dir.Normalize();

                float dis = Vector3.Distance(mTransform.position, currentTarget.mTransform.position);
                float angle = Vector3.Angle(mTransform.forward, dir);
                float dot = Vector3.Dot(mTransform.right, dir);

                if (dot < 0)
                {
                    angle *= -1;
                }

                currentSnapshot = GetAction(dis, angle);
                if (currentSnapshot != null && !actionFlag)
                {
                    PlayTargetAnimation(currentSnapshot.anim, true);
                    actionFlag = true;
                    recoveryTimer = currentSnapshot.recoveryTime;
                }
                else
                {
                    animator.SetFloat("sideways", relativeDirection.x, 0.1f, delta);
                }
            }

            if (!isInteracting)
            {
                agent.enabled = true;
                mTransform.rotation = agent.transform.rotation;

                //HeadIK
                lookPosition = currentTarget.mTransform.position;
                lookPosition.y += 1.2f;
                animatorHook.lookAtPosition = lookPosition;

            }

            if (isInteracting)
            {
                if (animatorHook.canRotate)
                {
                    HandleRotation(delta);
                }

                agent.enabled = false;

                animator.SetFloat("movement", 0f, 0.1f, delta);
                animator.SetFloat("sideways", 0f, 0.1f, delta);

                if (currentSnapshot != null)
                {
                    currentSnapshot.damageCollider.SetActive(animatorHook.openDamageCollider);
                }

            }


            //HeadIK
            lookPosition = currentTarget.mTransform.position;
            lookPosition.y += 1.2f;
            animatorHook.lookAtPosition = lookPosition;

            //Move with Root Motion 
            Vector3 targetVel = animatorHook.deltaPosition * moveSpeed;
            rigidbody.velocity = targetVel;
        }
    }

    private void LateUpdate()
    {
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;
    }

    public void PlayTargetAnimation(string targetAnim, bool toBeInteracting, float crossfadeTime = 0.2f, bool playInstantly = false)
    {
        animator.SetBool("isInteracting", toBeInteracting);

        if (!playInstantly)
        {
            animator.CrossFadeInFixedTime(targetAnim, crossfadeTime);
        }
        else
        {
            animator.Play(targetAnim);
        }
    }

    void HandleRotation(float delta)
    {
        Vector3 dir = currentTarget.mTransform.position - mTransform.position;
        dir.y = 0;
        dir.Normalize();

        if (dir == Vector3.zero)
        {
            dir = mTransform.forward;
        }

        float angle = Vector3.Angle(dir, mTransform.forward);
        if (angle > 5f)
        {
            animator.SetFloat("sideways", Vector3.Dot(dir, mTransform.right), 0.1f, delta);
        }
        else
        {
            animator.SetFloat("sideways", 0f, 0.1f, delta);
        }

        Quaternion targetRot = Quaternion.LookRotation(dir);
        mTransform.rotation = Quaternion.Slerp(mTransform.rotation, targetRot, delta / rotationSpeed);
    }

    void HandleDetection()
    {
        Collider[] cols = Physics.OverlapSphere(mTransform.position, fovRadius, detectionLayer);

        for (int i = 0; i < cols.Length; i++)
        {
            Controller controller = cols[i].transform.GetComponentInParent<Controller>();
            if (controller != null)
            {
                currentTarget = controller;
                animatorHook.hasLookAtTarget = true;
                return;
            }
        }
    }

    public Transform lockOnTarget;
    public Transform GetLockOnTarget(Transform from)
    {
        return lockOnTarget;
    }

    bool isHit;
    float hitTimer;

    public void OnDamage(ActionContainer action)
    {
        if (action.owner == mTransform)
            return;

        if (!isHit)
        {
            //Sound
            SoundManager.PlaySound(SoundManager.Sound.EnemyHit, mTransform.position);

            isHit = true;
            hitTimer = 0.2f;
            stats.health -= action.damage;
            animatorHook.CloseDamageCollider(); //for safety
            //Debug.Log("Enemy received " + action.damage + " new health is " + health);


            //VFX
            GameObject blood = ObjectPool.GetObject("BloodFX");
            blood.transform.position = mTransform.position + Vector3.up * 1f;
            blood.transform.rotation = mTransform.rotation;
            blood.transform.SetParent(mTransform);
            blood.SetActive(true);

            if (stats.health <= 0)
            {
                PlayTargetAnimation("Death", true);
                animator.transform.parent = null; // in order for ragdoll to properly work
                gameObject.SetActive(false); // could just destroy instead of disabling
            }
            else
            {
                Vector3 direction = action.owner.position - mTransform.position;
                float dot = Vector3.Dot(mTransform.forward, direction);

                if (action.overrideReactAnim)
                {
                    PlayTargetAnimation(action.reactAnim, true);
                }
                else if (!isInSpecialState)
                {
                    if (dot > 0)
                    {
                        PlayTargetAnimation("Get Hit Front", true, 0f, true);
                    }
                    else
                    {
                        PlayTargetAnimation("Get Hit Back", true, 0f, true);
                    }
                }
            }

            isInSpecialState = false;

        }
    }

    public bool IsAlive()
    {
        return stats.health > 0;
    }

    public ActionContainer GetActionContainer()
    {
        return lastAction;
    }

    public void OnParried(Vector3 dir)
    {
        if (animatorHook.canBeParried && tag != "Dragon") //dragon doesn't have animations
        {
            if (!isInInterruption)
            {
                animatorHook.CloseDamageCollider(); //for safety

                dir.Normalize(); // to rotate agent to look at us
                dir.y = 0;
                mTransform.rotation = Quaternion.LookRotation(dir);

                PlayTargetAnimation("Attack Interrupt", true, 0f, true);
            }
        }
    }

    public Transform getTransform()
    {
        return mTransform;
    }

    public float parriedDistance = 1.5f;
    public void GetParried(Vector3 origin, Vector3 direction)
    {
        isInSpecialState = true;


        mTransform.position = origin + direction * parriedDistance;
        mTransform.rotation = Quaternion.LookRotation(-direction);
        PlayTargetAnimation("Getting Parried", true, 0f, true);
    }

    public bool canBeParried()
    {
        return isInInterruption;
    }

    public bool canBeBackstabbed()
    {
        return openToBackstab;
    }

    public void GetBackstabbed(Vector3 origin, Vector3 direction)
    {
        isInSpecialState = true;
        openToBackstab = false;

        mTransform.position = origin + direction * parriedDistance;
        mTransform.rotation = Quaternion.LookRotation(direction);
        PlayTargetAnimation("Getting Backstabbed", true, 0f, true);
    }

    public FastStats GetStats()
    {
        return stats;
    }

    ActionContainer _lastAction;
    public ActionContainer lastAction {
        get {
            if (_lastAction == null)
            {
                _lastAction = new ActionContainer();
            }

            _lastAction.owner = mTransform; //For directional attacks
            _lastAction.damage = currentSnapshot.damage;
            _lastAction.overrideReactAnim = currentSnapshot.overrideReactAnim;
            _lastAction.reactAnim = currentSnapshot.reactAnim;

            return _lastAction;
        }
    }
}

[System.Serializable]
public class ActionSnapshot
{
    // Class intended as a substitute for attacks for AI
    public string anim;
    public int score = 5;
    public float recoveryTime;
    public float minDist = 2f;
    public float maxDist = 5f;
    public float minAngle = -35f;
    public float maxAngle = 35f;
    public GameObject damageCollider;
    public int damage = 10;
    public bool overrideReactAnim;
    public string reactAnim;
}
