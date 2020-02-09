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
            damageable.OnDamage(owner.GetActionContainer());
            //Debug.Log(other.name + " was hit.");
        }
    }
}
