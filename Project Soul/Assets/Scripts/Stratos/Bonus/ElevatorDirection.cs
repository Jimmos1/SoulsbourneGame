using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDirection : MonoBehaviour
{
    public GameObject elevatorBorders;
    public GameObject otherDirectionCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            elevatorBorders.GetComponent<Elevator>().RevertDirection();//upOrDown = !upOrDown;
            otherDirectionCollider.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
