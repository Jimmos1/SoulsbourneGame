using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public List<GameObject> hitGameObjects;

    void Awake()
    {
        hitGameObjects = new List<GameObject>();
    }

    void Update()
    {

        if (hitGameObjects.Count > 0)
        {
            Attack();
        }
    }

    void Attack()
    {
        Debug.Log("Attack");

        for(int i=0;i < hitGameObjects.Count; i++)
        {
            hitGameObjects[i].GetComponent<HPController>().hp -= 2;
        }
    }
}