using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILockable
{
    bool IsAlive();
    Transform GetLockOnTarget(Transform from);
}

public interface IDamageable
{
    void OnDamage(ActionContainer action);
}

public interface IDamageEntity
{
    ActionContainer GetActionContainer();
}

public interface IParryable
{
    void OnParried(Vector3 dir);
    Transform getTransform();
    void GetParried(Vector3 origin, Vector3 direction);
    bool canBeParried();
    void GetBackstabbed(Vector3 origin, Vector3 direction);
    bool canBeBackstabbed();
}
