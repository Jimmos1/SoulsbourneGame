using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject movePlatform;

    public bool upOrDown = true; //true = up , false = down

    public float up_elevatorContraintY;
    public float down_elevatorContraintY;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (upOrDown)
            {
                movePlatform.transform.position += movePlatform.transform.up * Time.deltaTime;
                if (movePlatform.transform.position.y >= up_elevatorContraintY) //HARDCODED FOR FUNCTIONALITY
                {
                    movePlatform.transform.position = new Vector3(movePlatform.transform.position.x, up_elevatorContraintY, movePlatform.transform.position.z);
                }
            }
            else
            {
                movePlatform.transform.position -= movePlatform.transform.up * Time.deltaTime;
                if (movePlatform.transform.position.y <= down_elevatorContraintY) //HARDCODED FOR FUNCTIONALITY
                {
                    movePlatform.transform.position = new Vector3(movePlatform.transform.position.x, down_elevatorContraintY, movePlatform.transform.position.z);
                }
            }
        }          
    }
    public void RevertDirection()
    {
        upOrDown = !upOrDown;
    }
    public void BoostDown()
    {
        movePlatform.transform.position = new Vector3(movePlatform.transform.position.x, 19f, movePlatform.transform.position.z);
    }
}
