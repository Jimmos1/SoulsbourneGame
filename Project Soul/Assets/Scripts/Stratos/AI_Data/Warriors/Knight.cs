using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Knight : Warrior
{
    /**
	 * We have one default goal which changes on demand
	 * from GoapCore to kill Enemy.
	 */

    //public int goalGeneratorID = 1;

    public override HashSet<KeyValuePair<string, object>> createGoalState(int goalGeneratorID)
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        if (goalGeneratorID == 1)
        {
            goal.Add(new KeyValuePair<string, object>("protectArea", true));
        }
        else if (goalGeneratorID == 2)
        {
            goal.Add(new KeyValuePair<string, object>("killEnemy", true));
        }
        else
        {
            goal.Add(new KeyValuePair<string, object>("killEnemy", true)); //default plan?
        }
        return goal;
    }
}
