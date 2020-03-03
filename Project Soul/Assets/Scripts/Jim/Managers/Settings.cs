using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static LayerMask interactionsLayer;

    static ResourcesManager _resourcesManager;
    public static ResourcesManager resourcesManager {
        get {
            if(_resourcesManager == null)
            {
                _resourcesManager = Resources.Load("ResourcesManager") as ResourcesManager;
                _resourcesManager.Init();
            }
            return _resourcesManager;
        }
    }
}
