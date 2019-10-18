using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour  //This class provides save system integrity and can be used to access most useful info without referencing one billion managers.
{
    //The type or member can be accessed by any code in the assembly in which it is declared, or from within a derived class in another assembly.
    //We use this to let changes happen only from Game Manager which derives from this class.
    protected internal Vector3 playerPosition = Vector3.one; //test
    protected internal Quaternion playerRotation;

    protected internal string imStupid = "imStupid";

    //TODO: Add more vars + Create Inventory Class.

    //GETTERS
    public Vector3 GetPlayerPos()
    {
        return playerPosition;
    }
    public Quaternion GetPlayerRot()
    {
        return playerRotation;
    }

    //SAVE-LOAD OPS
    protected internal bool InitSave() //Start the saving process
    {
        //ToDo some weird stuff to lock everything - Then call Save method.
        return true;
    }    
}
