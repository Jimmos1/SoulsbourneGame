using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorHelper : MonoBehaviour
{
    public GameObject elevatorBorders;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            elevatorBorders.GetComponent<Elevator>().BoostDown();
        }
    }
}
