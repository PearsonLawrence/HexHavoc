//Developed by Mason Smith. Made to test if mouse clicks work. Script is Obsolete.
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
