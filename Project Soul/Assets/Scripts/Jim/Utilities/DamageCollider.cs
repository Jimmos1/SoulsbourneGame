using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    IDamageEntity owner;

    private void Start()
    {
        owner = GetComponentInParent<IDamageEntity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if(damageable != null)
        {
            Debug.Log(owner.GetActionContainer().owner.name + " hit: " + other.name);


            damageable.OnDamage(owner.GetActionContainer());
            
        }
    }
}
