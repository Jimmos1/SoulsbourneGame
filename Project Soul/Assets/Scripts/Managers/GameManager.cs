using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Custom Inspector
[RequireComponent(typeof(StatsManager))]
public class GameManager : MonoBehaviour //Stats Manager derives from Monobehaviour so all good.
{
    public static GameManager instance;
    private StatsManager statsManager;
    private PlayerManager playerManager;

    public string sceneName;
    public WayPoint lastVisitedWaypoint;
    public PlayerArea currentPlayerArea;

    private bool isNewGame = false; //todo: change from Stats_Manager in the future - AVOID MAKING IT TRUE UNTIL STATENEWGAME IS COMPLETE.

    #region GameManagerEnums
    //Use these enums to make comparison checks in our program.
    public enum GameState
    {
        newGame,
        loadGame,
        developerShitShow //for the memes
    }
    private enum GameFlowState
    {
        isPlaying,
        isPaused,
        isLoading
    }
    public enum WayPoint //replace when design is ready with actual waypoint names
    {
        zero, //start game waypoint
        one,
        two,
        three,
        four,
        five,
        six,
        custom
    }
    public enum PlayerArea
    {
        level01,
        level02,
        level03,
        secretArea,
        custom
    }
    private enum CombatStatus
    {
        outOfCombat,
        inCombat,
        other
    }
    #endregion
    //TIMERS
    private float timeSinceStartup;
    private float timeUpdateTimer = 5.0f;
    private float timeSinceLastUpdate = 0.0f;

    void Awake()
    {
        MakeSingleton();
        SetUpAudio(); //Audio works the same for all scenes so its meant to be here.
    }

    void Start()
    {
        //setting up refs
        statsManager = this.GetComponent<StatsManager>();

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        SetupScene(SceneManager.GetActiveScene().name); //this sets up all entities in our scene to behave as they should.


        timeSinceStartup = Time.realtimeSinceStartup;

        
    }

    void Update()
    {
        timeSinceStartup = Time.realtimeSinceStartup;
        timeSinceLastUpdate += Time.deltaTime;
        
        if(timeSinceLastUpdate >= timeUpdateTimer)  //FOR TESTING PURPOSES
        {

            timeSinceLastUpdate = 0.0f;
        }
    }
    private void SetupScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Scene1":
                if (isNewGame) 
                {
                    lastVisitedWaypoint = WayPoint.zero;
                }
                else
                {
                    lastVisitedWaypoint = statsManager.GetLastVisitedWaypoint();
                }
                currentPlayerArea = PlayerArea.level01;
                break; 
            case "Scene2":
                lastVisitedWaypoint = statsManager.GetLastVisitedWaypoint();
                currentPlayerArea = PlayerArea.level02;
                break;
            case "Scene3":
                lastVisitedWaypoint = statsManager.GetLastVisitedWaypoint();
                currentPlayerArea = PlayerArea.level03;
                break;
            case "AI_Showcase":
                lastVisitedWaypoint = WayPoint.custom;
                currentPlayerArea = PlayerArea.custom;
                break;
                //Feel free to add your custom scene case here...
            default: break;
        }
        SetUpPlayer(lastVisitedWaypoint);
        SetUpAI(currentPlayerArea);
        SetUpTriggers(currentPlayerArea);
        SetUpUI();
    }

    private void SetUpPlayer(WayPoint spawnPoint)
    {
        //playerManager stuff
    }
    private void SetUpAI(PlayerArea area)
    {
        AI_Manager ai_M = this.GetComponent<AI_Manager>();
        ai_M.RespawnAgents(area);
        //TODO: Add AI Manager as task listener.
    }
    private void SetUpTriggers(PlayerArea area)
    {
        Debug.Log("Set up triggers complete");
    }
    private void SetUpUI()
    {
        //UIManager.Init();
    }
    private void SetUpAudio()
    {
        SoundManager.Initialize(); //init the sound manager
    }
    private void SetUpObjectives()
    {
        //Set up objectives
        Debug.Log("Objectives set");
    }

    public void SaveGame()
    {
        statsManager.InitSave();
    }
    public void LoadGame(StatsManager savefile)
    {
        //savefile.getcurrentwaypoint
        //----APPLY IT TO A SINGLETON----
        //SceneManager.LoadScene(savefile.GetCurrentSceneName);
        //----GAME MANAGER GETS -LOADGAME- INFO
        //----GAME MANAGER
    }
    public void YouDied() //Happens when player clicks on UI element --Respawn-- from last visited bonfire
    {
        //statsManager.deaths ++;
        //statsManager.getlatestbonfire();
        //----APPLY IT TO A SINGLETON----
        //SceneManager.LoadScene(statsManager.GetCurrentSceneName);
    }
    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}