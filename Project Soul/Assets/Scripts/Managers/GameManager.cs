using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class GameManager : StatsManager //Stats Manager derives from Monobehaviour so all good.
{
    private StatsManager statsManager;
    private PlayerManager playerManager;

    private float timeSinceStartup = 0.0f;
    private float timeUpdateTimer = 5.0f;
    private float timeSinceLastUpdate = 0.0f;
    
    void Start()
    {
        statsManager = this.GetComponent<StatsManager>();

        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        
        timeSinceStartup = Time.realtimeSinceStartup;
    }

    void Update()
    {
        timeSinceStartup = Time.realtimeSinceStartup;
        timeSinceLastUpdate += Time.deltaTime;

        if(timeSinceLastUpdate >= timeUpdateTimer)  //FOR TESTING PURPOSES
        {
            statsManager.playerPosition = playerManager.playerPosition;

            //Debug.Log("Updating player position: " + playerManager.playerPosition + " +Stats Manager: " + statsManager.playerPosition + " " + timeSinceStartup);

            timeSinceLastUpdate = 0.0f;
        }
    }
}
