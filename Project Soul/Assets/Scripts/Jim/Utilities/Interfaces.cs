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
    void OnDamage();
}
