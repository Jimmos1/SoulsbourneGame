using UnityEngine;
using System.Collections;

[System.Serializable]
public class FastStats
{
    public int health = 100;
    public int mana = 100;
    public float stamina = 100;

    public float staminaRefreshRate = 4;
    public float staminaDecreaseRate = 4;
    public float rollCost = 30;
    public float stepbackCost = 10;

    public void HandleHealth()
    {
        if (health > 100)
        {
            health = 100;
        }

        health = Mathf.Clamp(health, 0, 100);
    }

    public void HandleStamina(float delta, bool isSprinting)
    {
        if (!isSprinting)
        {
            stamina += delta / staminaRefreshRate;
        }
        else
        {
            stamina -= delta / staminaDecreaseRate;
        }

        stamina = Mathf.Clamp(stamina, -25f, 100f);
    }

    public void AssignCostsOfAction(ItemActionContainer ac)
    {
        mana -= ac.manaCost;
        stamina -= ac.staminaCost;
    }

    public void AssignRollCost(bool isStepback)
    {
        if (isStepback)
        {
            stamina -= stepbackCost;
        }
        else
        {
            stamina -= rollCost;
        }
    }
}
