using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int pHealth = 100;
    private int MAX_PLAYER_HEALTH = 100; //player will be able to improve max health.

    private bool isImmune = false; //for dodge 

    private bool canHeal = false; //can heal when less than max health

    void Start()
    {
        
    }
    void Update()
    {
        if (pHealth < MAX_PLAYER_HEALTH) //TODO: remove from update add to methods.
        {
            canHeal = true;
        }
        else if (pHealth == MAX_PLAYER_HEALTH)
        {
            canHeal = false;
        }
    }

    public bool takeDamage(int amount) //returns a bool to let attacker know if damage went through.
    {
        if (!isImmune)
        {
            pHealth -= amount;
            if (pHealth <= 0)
            {
                pHealth = 0;
                //Do death stuff (sound,anims,gameover screen)
                Debug.Log("Player died.");
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool healPlayer(int amount)
    {
        if (canHeal)
        {
            pHealth += amount;
            if(pHealth >= MAX_PLAYER_HEALTH)
            {
                pHealth = MAX_PLAYER_HEALTH;
            }
            return true;
        }
        else return false;
    } 
    public int getPlayerHealth()
    {
        return pHealth;
    }
    public void setPlayerHealth(int newPlayerHealth)
    {
        pHealth = newPlayerHealth;
    }
}
