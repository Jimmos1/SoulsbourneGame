using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    //Frankensteined by jim things of things might be broken
    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;

    NavMeshAgent agent;            // the navmesh agent required for the path finding
    Rigidbody currentRigidbody;
    Animator currentAnimator;
    float turnAmount;
    float forwardAmount;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;


        currentAnimator = GetComponent<Animator>();
        currentRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (agent.remainingDistance >= agent.stoppingDistance)
        {
            agent.isStopped = false;

            AnimationMove(agent.desiredVelocity);

        }
        else
        {
            agent.isStopped = true;
            PlayerMovement.moveAmount = 0;
            AnimationMove(Vector3.zero);
        }
    }


    public void AnimationMove(Vector3 move)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }
        move = transform.InverseTransformDirection(move);
        turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z),-1.8f,1.8f);
        forwardAmount = Mathf.Clamp(PlayerMovement.moveAmount,0,2);
        ApplyExtraTurnRotation();

        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        currentAnimator.SetFloat("Forward", forwardAmount, 0.05f, Time.deltaTime);
        currentAnimator.SetFloat("Turn", turnAmount, 0.05f, Time.deltaTime);
        currentAnimator.SetBool("OnGround", true);

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (move.magnitude > 0)
        {
            currentAnimator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            currentAnimator.speed = 1;
        }
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }


    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (Time.deltaTime > 0)
        {
            Vector3 v = (currentAnimator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = currentRigidbody.velocity.y;
            currentRigidbody.velocity = v;
        }
    }
}
