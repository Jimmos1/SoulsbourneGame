using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItem : Item
{
    public string consumeAnimation;
    public string emptyAnimation;
    public abstract void OnConsume(FastStats stats);
}
