using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Custom Inspector
[RequireComponent(typeof(StatsManager))]
public class GameManager : MonoBehaviour //Stats Manager derives from Monobehaviour so all good.
{
    private StatsManager statsManager;
    private PlayerManager playerManager;

    private StateMachine stateMachine = new StateMachine();
    private bool isNewGame = false; //todo: change from Stats_Manager in the future - AVOID MAKING IT TRUE UNTIL STATENEWGAME IS COMPLETE.

    //Use these enums to make comparison checks in our program.
    private enum gameFlowState
    {
        isPlaying,isPaused,isLoading
    }
    private enum playerArea
    {
        level01,level02,level03,secretArea
    }
    private enum combatStatus
    {
        outOfCombat,inCombat,other
    }

    //TIMERS
    private float timeSinceStartup = 0.0f;
    private float timeUpdateTimer = 5.0f;
    private float timeSinceLastUpdate = 0.0f;

    void Awake()
    {
        
    }

    void Start()
    {
        statsManager = this.GetComponent<StatsManager>();

        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

        if (isNewGame)
        {
            stateMachine.ChangeState(new StateNewGame(this));
        }
        
        timeSinceStartup = Time.realtimeSinceStartup;
    }

    void Update()
    {
        timeSinceStartup = Time.realtimeSinceStartup;
        timeSinceLastUpdate += Time.deltaTime;

        stateMachine.Update();

        if(timeSinceLastUpdate >= timeUpdateTimer)  //FOR TESTING PURPOSES
        {
            //statsManager.playerPosition = playerManager.playerPosition;

            //Debug.Log("Updating player position: " + playerManager.playerPosition + " +Stats Manager: " + statsManager.playerPosition + " " + timeSinceStartup);

            timeSinceLastUpdate = 0.0f;
        }
    }
}