using UnityEngine;
using System.Collections;

public class ParryCollider : MonoBehaviour
{
    IParryable owner;

    private void Start()
    {
        owner = transform.GetComponentInParent<IParryable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IParryable parryable = other.transform.GetComponentInParent<IParryable>();

        if (parryable != null)
        {
            if (parryable != owner)
            {
                parryable.OnParried(owner.getTransform().position - parryable.getTransform().position);
            }
        }
    }
}
