using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Items/AddStat")]
public class AddStat : ConsumableItem
{
    public int addValue; // generic value

    public override void OnConsume()
    {
        Debug.Log("Add value: " + addValue);
        //find stats and add value
        Controller controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
        controller.health += addValue;
    }
}
