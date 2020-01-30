using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour, ILockable, IDamageable
{
    new Rigidbody rigidbody;
    Animator animator;
    NavMeshAgent agent;
    AnimatorHook animatorHook;
    Transform mTransform;

    public int health = 100;

    public float fovRadius = 20;
    public float rotationSpeed = 1;
    public float moveSpeed = 2;
    public float recoveryTimer;
    public int hardcodeAction = -1;
    LayerMask detectionLayer;

    Controller currentTarget;
    bool isInteracting = false;
    bool actionFlag;
    ActionSnapshot currentSnapshot;

    public ActionSnapshot[] actionSnapshots;

    public ActionSnapshot GetAction(float distance, float angle)
    {

        if(hardcodeAction != -1)
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

        agent.stoppingDistance = 1f;
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = animator.GetBool("isInteracting");

        if (isHit) // Invinsibility
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
                mTransform.position = agent.transform.position; // floaty patch :C needs fixing
            }

            Vector3 relativeDirection = mTransform.InverseTransformDirection(agent.desiredVelocity);
            relativeDirection.Normalize();

            if (!isInteracting)
            {
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

                if(dot < 0)
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
                Vector3 lookPosition = currentTarget.mTransform.position;
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

                if(currentSnapshot != null)
                {
                    currentSnapshot.damageCollider.SetActive(animatorHook.openDamageCollider);
                }

            }


        }
    }

    private void FixedUpdate()
    {

        //Move with Root Motion 
        Vector3 targetVel = animatorHook.deltaPosition * moveSpeed;
        rigidbody.velocity = targetVel;
    }

    private void LateUpdate()
    {
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
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
        if (angle > 5)
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

    public void OnDamage()
    {
        if (!isHit)
        {
            isHit = true;
            hitTimer = 1f;
            PlayTargetAnimation("Damage 1", true);

            health -= 20;
            if(health <= 0)
            {
                PlayTargetAnimation("Death", true);
                animator.transform.parent = null;
                gameObject.SetActive(false);
            }
        }
    }

    public bool IsAlive()
    {
        return health > 0;
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
}
