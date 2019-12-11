using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : CharacterStateManager
{
    [Header("Inputs")]
    public float mouseX;
    public float mouseY;
    public float moveAmount;
    public Vector3 rotateDirection;


    [Header("References")]
    public new Transform camera;
    public Cinemachine.CinemachineFreeLook normalCamera;
    public Cinemachine.CinemachineFreeLook lockOnCamera;


    [Header("Movement Stats")]
    public float frontRayOffset = .5f;
    public float movementSpeed = 1;
    public float adaptSpeed = 1;
    public float rotationSpeed = 10;
    public float attackRotationSpeed = 3;

    [HideInInspector]
    public LayerMask ignoreForGroundCheck;

    public string locomotionId = "locomotion";
    public string attackStateId = "attackId";



    public override void Init()
    {
        base.Init();

        MovePlayerCharacter movePlayerCharacter = new MovePlayerCharacter(this);

        State locomotion = new State(
            new List<StateAction>() //Fixed Update
            {
                movePlayerCharacter,
            },
            new List<StateAction>() //Update
            {
                new InputControl(this),
            },
            new List<StateAction>()//Late Update
            {
            }
            );

        locomotion.onEnter = DisableRootMotion;
        locomotion.onEnter += DisableCombo;

        State attackState = new State(
            new List<StateAction>() //Fixed Update
            {
                new HandleRotationHook(this, movePlayerCharacter),
            },
            new List<StateAction>() //Update
            {
                new MonitorInteractingAnimation(this,"isInteracting", locomotionId),
                new InputsForCombo(this),
            },
            new List<StateAction>()//Late Update
            {
            }
            );

        attackState.onEnter = EnableRootMotion;
        attackState.onEnter += DisableComboVariables;

        RegisterState(locomotionId, locomotion);
        RegisterState(attackStateId, attackState);

        ChangeState(locomotionId);

        ignoreForGroundCheck = ~(1 << 9 | 1 << 10);

        weaponHolderManager.Init();
        weaponHolderManager.LoadWeaponOnHook(leftWeapon, true);
        weaponHolderManager.LoadWeaponOnHook(rightWeapon, false);
        UpdateItemActionsWithCurrent();
    }

    private void FixedUpdate()
    {
        delta = Time.fixedDeltaTime;

        base.FixedTick();
    }

    public bool debugLock;

    private void Update()
    {
        delta = Time.deltaTime;
        base.Tick();
    }

    private void LateUpdate()
    {
        base.LateTick();
    }

    #region Lock on 
    public override void OnAssignLookOverride(Transform target)
    {
        base.OnAssignLookOverride(target);
        if (lockOn == false)
            return;

        normalCamera.gameObject.SetActive(false);
        lockOnCamera.gameObject.SetActive(true);
        lockOnCamera.m_LookAt = target;
    }

    public override void OnClearLookOverride()
    {
        base.OnClearLookOverride();
        normalCamera.gameObject.SetActive(true);
        lockOnCamera.gameObject.SetActive(false);
    }



    #endregion

    public override void PlayTargetItemAction(AttackInputs attackInput)
    {
        canRotate = false;

        ItemActionContainer iac = GetItemActionContainer(attackInput, itemActions);

        iac.ExecuteItemAction(this);
    }

    public override void DoCombo()
    {
        currentItemAction.ExecuteItemAction(this);
        ChangeState(attackStateId);
    }

    #region State Events
    void DisableRootMotion()
    {
        useRootMotion = false;
    }

    void EnableRootMotion()
    {
        useRootMotion = true;
    }

    void DisableComboVariables()
    {
        canDoCombo = false;
    }

    #endregion

}