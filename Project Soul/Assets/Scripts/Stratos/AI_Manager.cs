using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Manager : MonoBehaviour
{
    private int layers = 2; //the first step of creating advanced AI. Layer will be something like groups that manage different AIs.

    private GameManager gameManager;
    private StatsManager statsManager;

    private int MAX_ALLOWED_AGENTS = 20; //for object pooling

    private int nr_activeAgents = 0; //We grab this from game manager in later version.
    //private GameObject[] activeAgents = new GameObject[nr_activeAgents];
    //private AI_Attack[] activeAgentsAtkScripts = new AI_Attack[nr_activeAgents];
    //private AI_Movement[] activeAgentsMovementScripts = new AI_Movement[nr_activeAgents];
    //private AI_Health[] activeAgentsHealthScripts = new AI_Health[nr_activeAgents];
    //private NavMeshAgent[] activeAgentsNavMesh = new NavMeshAgent[nr_activeAgents];
    //private Animator[] activeAgentsAnimator = new Animator[nr_activeAgents];
    //private float[] activeAgentsAttackTimer = new float[nr_activeAgents];
    //private int[] activeAgentsID = new int[nr_activeAgents];
    //private bool[] activeAgentsEnabled = new bool[nr_activeAgents];


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
