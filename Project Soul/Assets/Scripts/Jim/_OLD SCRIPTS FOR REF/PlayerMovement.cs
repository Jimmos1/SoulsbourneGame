using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour  
{
    public PlayerMovement playerMovement { get; set; }
    public InputManager inputManger;

    private NavMeshAgent agent;


    public static float moveAmount;

    public bool shouldPlayerMove = true;

    Vector3 agentPath;
    RaycastHit hit;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.remainingDistance < 0.5f || !shouldPlayerMove)
        {
            agent.ResetPath();
        }
        if (Input.GetMouseButton(0) && shouldPlayerMove)
        {
            Move();
        }
    }
    void LateUpdate()
    {
        if (agent.hasPath)
        {
            moveAmount = Mathf.Clamp(agent.remainingDistance * 0.3f, 0, 2);
        }
        else
        {
            moveAmount = 0;
        }
    }

    void Move()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 35f, layerMask: 9))
            {
                if (hit.collider.gameObject.layer != 10)
                {
                    agent.SetDestination(hit.point);
                }
            }
        }


    }
}