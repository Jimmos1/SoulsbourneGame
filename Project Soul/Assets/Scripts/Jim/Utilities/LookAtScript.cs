using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtScript : MonoBehaviour
{
    Canvas canvas;
    Camera playerCamera;

    private void Start()
    {
        canvas = transform.GetComponent<Canvas>();
        playerCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(playerCamera.transform);
    }
}
