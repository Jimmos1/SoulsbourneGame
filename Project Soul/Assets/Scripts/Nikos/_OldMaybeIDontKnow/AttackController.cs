using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public List<GameObject> hitGameObjects;

    public bool isAttacking;

    void Awake()
    {
        isAttacking = false;
        hitGameObjects = new List<GameObject>();
    }

    void Update()
    {
        if (isAttacking && hitGameObjects.Count > 0)
        {
            Attack();
        }
    }

    void Attack()
    {
        for(int i=0;i < hitGameObjects.Count; i++)
        {
            // Damage
            hitGameObjects[i].GetComponent<HPController>().GotHit(50 + UnityEngine.Random.Range(-20, 20)); // Random Damage

            // Remove Enemy from List
            if (hitGameObjects.Contains(hitGameObjects[i])){
                hitGameObjects.RemoveAt(i);
            }
        }
    }
}