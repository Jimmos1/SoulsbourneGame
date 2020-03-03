using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    static ObjectPoolAsset _poolAsset;

    public static ObjectPoolAsset objectPool {
        get {

            if(_poolAsset == null)
            {
                _poolAsset = Resources.Load("ObjectPool") as ObjectPoolAsset;
                _poolAsset.Init();
            }
            
            return _poolAsset;
        }
    }

    public static GameObject GetObject(string id)
    {
        return objectPool.GetObject(id);
    }
}
