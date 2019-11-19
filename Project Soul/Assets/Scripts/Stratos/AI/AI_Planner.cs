using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Planner : MonoBehaviour
{
    /*
     * One Job.
     * Create an experience.
     */

    private int agentID;

    private bool isGOAP_Agent = true; //Goal Oriented Action Planning

    private int agentLayers = 2; //One layer centered arround Goal and another layer centered arround UX.

    private Transform agentTransform;
    private Vector3 agentPosition;


    private string[] agentGoals = new string[4] { "StayAlive", "DefendSomething", "AttackSomething", "KillPlayer" };
    //Other potential goal candidates can and will be considered in future versions, although these 4 are more than enough for this project.
    private string currentGoal;
           

    [SerializeField]
    private List<string> agentPlans;
    private List<string> agentActions;

    private string worldState = "Unknown";
    private string currentAgentState;

    private string currentPlan;

    private AI_Manager aiManager;

    //CONSTRUCTORS DO NOT WORK ON MONOBEHAVIOUR INHERITANCE.
    //public AI_Planner(AI_Manager AIcontroller) //Constructor to initiate a handler for this AI.
    //{
    //    this.aiManager = AIcontroller;
    //}

    //public AI_Planner()  
    //{
    //    //Getting Manager reference on construction of this class.
    //    aiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<AI_Manager>(); 
    //    if(aiManager == null)
    //    {
    //        Debug.Log("AI_MANAGER NOT FOUND IN TARGET MANAGER OBJECT, INITIATING SOLO SURVIVAL PLAN");
    //    }
    //}

    private void Awake() //Getting AI_Manager & NEW ID
    {
        this.aiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<AI_Manager>();
        if (aiManager == null)
        {
            agentID = Random.Range(0, 101); //Init a random ID 0-100 if no Manager found to provide a seed for later on.
            Debug.Log("AI_MANAGER NOT FOUND IN TARGET MANAGER-TAGGED OBJECT, INITIATING SOLO SURVIVAL PLAN");

        }
        else if (aiManager != null && aiManager.IsAIManagerActive)  //if we find aiManager & its active
        {
            agentID = aiManager.RegisterNewAgent(this.gameObject);  //...register new entity and get ID
            Debug.Log("New agent joins the party with ID:" + agentID);
        }
    }
    void Start() //Setting up behaviour (Goals, Actions, refs)
    {
        this.agentTransform = this.transform;
        this.agentPosition = this.agentTransform.position;

        if(aiManager != null && aiManager.IsAIManagerActive)
        {
            if (isGOAP_Agent)
            {
                //TODO: Setup layers in future version(s). Early versions will have 2 layers.

                currentGoal = agentGoals[aiManager.ProvideGoalForAgent(this.agentID)]; //Stay alive as default - Change in future version
                switch (currentGoal)
                {
                    case "StayAlive": break;

                    default: break;
                }
            }
        }
        else if (aiManager == null) //TODO: Manage solo survival plan.
        {
            if (isGOAP_Agent)
            {
                currentGoal = this.agentGoals[1];
            }
        }
    }

    void Update()
    {
        this.agentPosition = this.GetComponent<Transform>().position;
    }
}
