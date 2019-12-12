using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Knight : Warrior
{
    /**
	 * Our only goal will ever be to make tools.
	 * The ForgeTooldAction will be able to fulfill this goal.
	 */
    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        //goal.Add(new KeyValuePair<string, object>("protectArea", true));
        goal.Add(new KeyValuePair<string, object>("killEnemy", true));
        return goal;
    }
}
