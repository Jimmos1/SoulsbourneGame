using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Utilities/ObjectPooler")]
public class ObjectPoolAsset : ScriptableObject
{
    public Pool[] pools;
    [System.NonSerialized]
    Dictionary<string, Pool> poolDict = new Dictionary<string, Pool>();
    [System.NonSerialized]
    Transform parentTransform;
    public bool prewarm;

    public void Init()
    {
        parentTransform = new GameObject("Object Pool").transform;

        for (int i = 0; i < pools.Length; i++)
        {
            poolDict.Add(pools[i].poolName, pools[i]);

            if (prewarm)
            {
                pools[i].PrewarmObject(parentTransform);
            }
        }
    }

    public GameObject GetObject(string id)
    {
        poolDict.TryGetValue(id, out Pool value);

        return value.GetObject(parentTransform);
    }
}

[System.Serializable]
public class Pool
{
    public string poolName;
    public GameObject prefab;
    public int budget;

    [System.NonSerialized]
    List<GameObject> createdObjects = new List<GameObject>();
    [System.NonSerialized]
    int index;

    public GameObject GetObject(Transform parent)
    {
        GameObject retVal = null;

        if(createdObjects.Count < budget)
        {
            GameObject tempObject = GameObject.Instantiate(prefab) as GameObject;
            tempObject.transform.SetParent(parent);
            createdObjects.Add(tempObject);
            retVal = tempObject;
        }
        else
        {
            retVal = createdObjects[index];
            index++;
            if(index > createdObjects.Count - 1)
            {
                index = 0;
            }
        }

        if(retVal != null)
        {
            retVal.SetActive(false);
        }

        return retVal;
    }

    public void PrewarmObject(Transform parent)
    {
        for (int i = 0; i < budget; i++)
        {
            GameObject tempObject = GameObject.Instantiate(prefab) as GameObject;
            tempObject.SetActive(false);
            tempObject.transform.SetParent(parent);
            createdObjects.Add(tempObject);
        }
    }

}