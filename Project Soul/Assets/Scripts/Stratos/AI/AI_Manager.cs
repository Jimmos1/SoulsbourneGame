using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Manager : MonoBehaviour
{
    /*
     * AI manager is an AI that handles all agents deployed in the game.
     * 1) Spawns/Despawns agents according to Game Manager plan.
     * 2) Modifies agent stats when necessary.
     * 3) Changes agent plan when necessary.
     * 4) In future version, applies hierarchical planning to agents.
     * 5) Provides ID and information to agents.
     * 6) Observer pattern through ITaskObserver.
     */

    //private int layers = 2; //The first step of creating advanced AI. Neural networks. Input Layer -> Hidden Layers (Computation) -> Output Layer

    private GameManager gameManager;
    private StatsManager statsManager;

    public bool IsAIManagerActive { get; private protected set; } = true;
    
    private int MAX_ALLOWED_AGENTS = 20; //for object pooling

    private int nr_activeAgents = 0; //We grab this from game manager in later version.

    private int newAgentID = 0;

    //Known intel for our ACTIVE agents. (Cheat factor - PERCEPTION)
    //Player Health, Player Position, Last 3 player moves.

    //Agent gameobject requires Animator, NavMeshAgent, DataProvider(e.g. Knight Class), CombatStats, BackpackComponent and GoalGenerator(e.g. GOAP Agent).
    //Agent gameobject always has a defensive and an offensive action. Also, a number of special actions. Order of execution changes. 
    
    //Agent object-pooling method will be used for Project Soul. Agents are said to include a rigidbody but that's not final. 
    //Agent will have an AI_Attack, AI_Health, AI_Stats(Modification), AI_Goals, AI_Actions and of course AI_Planner.
    //Action scripts will contain all AI mechanics like Attack(Normal, Heavy, Combo), Defense(Guard, Dodge) and Flavour(eat,forge).
    //Movement is handled by a Stack-Based FSM.

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
