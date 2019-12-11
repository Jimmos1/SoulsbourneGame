using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILockable
{
    Transform GetLockOnTarget(Transform from);
}
