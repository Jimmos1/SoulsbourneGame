using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Manager : MonoBehaviour
{
    private int layers = 2; //the first step of creating advanced AI. Layer will be something like groups that manage different AIs.

    private GameManager gameManager;
    private StatsManager statsManager;

    public bool IsAIManagerActive { get; private protected set; } = true;
    
    private int MAX_ALLOWED_AGENTS = 20; //for object pooling

    private int nr_activeAgents = 0; //We grab this from game manager in later version.

    private int newAgentID = 0;

    //private GameObject[] activeAgents = new GameObject[nr_activeAgents];
    //private AI_Attack[] activeAgentsAtkScripts = new AI_Attack[nr_activeAgents];
    //private AI_Movement[] activeAgentsMovementScripts = new AI_Movement[nr_activeAgents];
    //private AI_Health[] activeAgentsHealthScripts = new AI_Health[nr_activeAgents];
    //private NavMeshAgent[] activeAgentsNavMesh = new NavMeshAgent[nr_activeAgents];
    //private Animator[] activeAgentsAnimator = new Animator[nr_activeAgents];
    //private float[] activeAgentsAttackTimer = new float[nr_activeAgents];
    //private int[] activeAgentsID = new int[nr_activeAgents];
    //private bool[] activeAgentsEnabled = new bool[nr_activeAgents];

    //Resource Assignment Algorithm
    //1.Assignment Scoring for active Agents -GOAP
    // Black Knights->  Protect the Area -> On low HP Stay Alive -> Kill player    -BT

    //Known intel for our ACTIVE agents. (Cheat factor)
    //Player Health, Player Position, Last 3 player moves.

    //Agent kit contains always a defensive move and an attack. Also, a number of special abilities. Order of execution changes. -GOAP
    //Agent stats, abilities and custom stuff are contained in a separate script that is loaded on spawn. 
    //Remove old stats component -> Add New Stats Component. (Will be drawn directly from Recources folder)

    //Behaviour control will go through this manager but actions are taken from the agent. Agent can be stand-alone without AIManager controller.
    //TODO: Add 2 layers on Agent. One will go through the process with puppet master (AIManager in our case) and the other standalone based on 
    //self-survival plan. Add AIReligion templates as well for fun.

    //Agent object-pooling method will be used for Project Soul. Agents are said to include a rigidbody but that's not final. 
    //Agent will have an AI_Attack, AI_Health, AI_Stats(Modification), AI_Goals, AI_Actions and of course AI_Planner.
    //Action scripts will contain all AI mechanics like Movement(Patrol, Idle, Chase), Attack(Normal, Heavy, Combo), Defense(Guard, Dodge) and Flavour(eat,forge).

    //Goal-Oriented Action Planning method is used OVER Behaviour Tree for random factor and UX. This method will help the team achieve
    //the best looking gameplay product.

    //Actions + Goals + WorldState -> Planner(A*) -> Plan -> FSM(MOVE <-> PERFORM)
    //This might create a hard unoptimized experience in the beginning of the systems that will go through optimization cycles to achieve the best quality.


    void Start()
    {
        gameManager = this.GetComponent<GameManager>();
        statsManager = this.GetComponent<StatsManager>();
    }

    void Update()
    {        
       if(nr_activeAgents > 0)
        {
            //foreach ai in aiList blablabla
        }        
    }
    public int RegisterNewAgent(GameObject rookie) //Registers new agent in the system & returns requested ID.
    {
        //TODO: REGISTER PROCESS
        newAgentID++;       //tricks
        return newAgentID-1;
    }
    public int ProvideGoalForAgent(int agentID)
    {
        //TODO: Decide desirable goal for specific agentID.
        int goalID = 1; //DUMMY
        return goalID;
    }
    public void RespawnAgents(int areaID) //Called usually when player dies.
    {
        switch (areaID) 
        {
            case 1: break; //after ops complete we should have a callback to let game manager know of ops progress.
            case 2: break;
            case 3: break;
            case 4: break;
        }
    }
    public void RespawnSingleAgent(int agentID, Vector3 spawnPoint)
    {
        if(nr_activeAgents < MAX_ALLOWED_AGENTS)
        {
            //stuff
        }
    }
    //public void AddAIAsEdge(GameObject zombo)
    //{
    //    //applying references for optimal behaviour/control.
    //    activeAgentsPositions[zombosSpawned] = zombo.transform.position;
    //    activeAgentsAtkScripts[zombosSpawned] = zombo.GetComponent<ZomboAttack>();
    //    activeAgentsHealthScripts[zombosSpawned] = zombo.GetComponent<ZomboHealth>();
    //    activeAgentsMovementScripts[zombosSpawned] = zombo.GetComponent<ZomboMovement>();
    //    activeAgentsNavMesh[zombosSpawned] = zombo.GetComponent<NavMeshAgent>();
    //    activeAgentsAnimator[zombosSpawned] = zombo.GetComponent<Animator>();
    //    activeAgentsAttackTimer[zombosSpawned] = defaultZomboAttackTimer;

    //    activeAgentsID[zombosSpawned] = zombosSpawned; //Assigning ID
    //    activeAgentsEnabled[zombosSpawned] = true; //Toggled off from ZomboDeath().        
    //    activeAgentsMovementScripts[zombosSpawned].ChangeMyID(zombosSpawned); //ID is on zombo as well

    //    if (currentDifficulty) //MEANS WE ARE ON HARD DIFFICULTY
    //    {
    //        int atkDifficultyFormula = 4 + currentWave;
    //        activeAgentsAtkScripts[zombosSpawned].SetZomboAtkDmg(atkDifficultyFormula);

    //        int healthDifficultyFormula = 95 + currentWave * 5;
    //        activeAgentsHealthScripts[zombosSpawned].SetZomboHealth(healthDifficultyFormula);
    //    }
    //}
}
