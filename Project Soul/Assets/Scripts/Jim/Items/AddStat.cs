using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Items/AddStat")]
public class AddStat : ConsumableItem
{
    public int addValue; // generic value
    public bool isMana; // dirty flag

    public override void OnConsume(FastStats stats)
    {
        Debug.Log("Add value: " + addValue);

        if (isMana)
        {
            stats.mana += addValue;
        }
        else
        {
            stats.health += addValue;
        }

        //find stats and add value
        //Controller controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
        //controller.health += addValue;
    }
}
