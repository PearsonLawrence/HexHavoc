//Created by Mason Smith. Used to check for mouse left click inputs in Unity on PC. Not necessary to gameplay.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputs : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse 0 - Left Click");
        }
    }
}
