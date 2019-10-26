using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : CharacterStateManager
{
    //Specific Class for our Player or enemy (PVP) Players
    //Also our Monobehaviours get updated here but actually control our StateManager
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
    public float frontRayOffset = 0.5f;
    public float movementSpeed = 1.0f;
    public float adaptSpeed = 1.0f;
    public float rotationSpeed = 10.0f;

    [HideInInspector]
    public LayerMask ignoreForGroundCheck;

    public string locomotionId = "locomotion";
    public string attackStateId = "attackState";

    public override void Init()
    {
        base.Init();

        State locomotion = new State(
            new List<StateAction>() //Fixed Update
            {
                new MovePlayerCharacter(this)
            },
            new List<StateAction>() //Update
            {
                new InputControl(this)
            },
            new List<StateAction>() //Late Update
            {

            }
            );

        locomotion.onEnter = DisableRootMotion;

        State attackState = new State(
            new List<StateAction>() //Fixed Update
            {

            },
            new List<StateAction>() //Update
            {
                new MonitorInteractingAnimation(this,"isInteracting", locomotionId),
            },
            new List<StateAction>() //Late Update
            {

            }
            );

        attackState.onEnter = EnableRootMotion;

        RegisterState(locomotionId, locomotion);
        RegisterState(attackStateId, attackState);

        ChangeState(locomotionId);

        ignoreForGroundCheck = ~(1 << 9 | 1 << 10);
    }

    private void FixedUpdate()
    {
        delta = Time.fixedDeltaTime;

        base.FixedTick();
    }

    private void Update()
    {
        delta = Time.deltaTime;

        base.Tick();
    }

    private void LateUpdate()
    {
        base.LateTick();
    }

    #region Lock On
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



    #region State Events
    void DisableRootMotion()
    {
        useRootMotion = false;
    }

    void EnableRootMotion()
    {
        useRootMotion = true;
    }
    
    #endregion
}
