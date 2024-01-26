//Created by Mason Smith. Activates teleportation ray originating from hand on trigger pull of controllers.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    //If holding an object, disables teleportation for that controller.
    public InputActionProperty leftCancel;
    public InputActionProperty rightCancel;

    // Update is called once per frame
    void Update()
    {
        //If trigger is pulled on controller, as long as the pull value is >0.1, it activates the teleportation.
        leftTeleportation.SetActive(leftCancel.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);
        rightTeleportation.SetActive(leftCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);
    }
}
