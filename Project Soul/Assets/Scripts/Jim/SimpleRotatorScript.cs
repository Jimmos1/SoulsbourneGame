using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotatorScript : MonoBehaviour
{
    //for funsies
    public float speed = 0.36f;

    public Vector3 pointA;
    public Vector3 pointB;


    void Start()
    {
        //Get current position then add 90 to its Y axis
        pointA = transform.eulerAngles + new Vector3(0f,0f, 70f);

        //Get current position then substract -90 to its Y axis
        pointB = transform.eulerAngles + new Vector3(0f, 0f, -10f);
    }

    void Update()
    {
        //PingPong between 0 and 1
        float time = Mathf.PingPong(Time.time * speed, 1);
        transform.eulerAngles = Vector3.Lerp(pointA, pointB, time);
    }
}
