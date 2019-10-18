using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform playerTransform; //Cannot modify transform directly because its a struct so adding vectors as well.

    public Vector3 playerPosition;
    public Quaternion playerRotation;

    void Start()
    {
        playerTransform = this.GetComponent<Transform>();
        playerPosition = playerTransform.position;
        playerRotation = playerTransform.rotation;
    }

    void Update()
    {
       //playerPosition.x++; 
    }
}
