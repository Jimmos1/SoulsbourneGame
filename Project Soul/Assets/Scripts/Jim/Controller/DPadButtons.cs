using UnityEngine;
using System.Collections;

public class DPadButtons : MonoBehaviour
{
    public static bool IsLeft, IsRight, IsUp, IsDown;
    private float _LastX, _LastY;

    private void Update()
    {
        float x = Input.GetAxis("DPad X");
        float y = Input.GetAxis("DPad Y");

        IsLeft = false;
        IsRight = false;
        IsUp = false;
        IsDown = false;

        if (_LastX != x)
        {
            if (x == -1)
                IsLeft = true;
            else if (x == 1)
                IsRight = true;
        }

        if (_LastY != y)
        {
            if (y == -1)
                IsDown = true;
            else if (y == 1)
                IsUp = true;
        }

        _LastX = x;
        _LastY = y;
    }
}
