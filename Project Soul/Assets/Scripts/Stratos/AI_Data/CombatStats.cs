using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStats : MonoBehaviour
{
    /*
     * This class provides a variety of defensive and offensive statistics that can be modified for every agent.
     * Commented values are for testing-reminder.
     */
    public int healthPoints; //100

    [Header("Defence")]
    public int physical;  //87
    public int magic_res; //30

    [Header("Resistances")]
    public int bleed; //100
    public int poison; //100

    public bool isDisabled = false;
    public bool isSlowed = false;

    public bool isAware = true; //TODO: SET TO FALSE IN LATER VERSION
}
