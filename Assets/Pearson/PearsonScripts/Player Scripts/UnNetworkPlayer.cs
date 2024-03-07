using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnNetworkPlayer : MonoBehaviour
{
    // Start is called before the first frame update\
    public InputActionProperty leftGripProperty;
    public InputActionProperty rightGripProperty;
    public HandInteractableComponent interactleft, interactRight;
    void Start()
    {

    }

    
        

    // Update is called once per frame
    void Update()
    {
        float triggerValue = leftGripProperty.action.ReadValue<float>();
        float triggerValue2 = rightGripProperty.action.ReadValue<float>();

        if (triggerValue > 0.1f)
        {
            interactleft.isSelecting = true;
        }
        else if (triggerValue <= 0.1f )
        {
            interactleft.isSelecting = false;
            interactleft.isHolding = false;
        }
        if (triggerValue2 > 0.1f)
        {
            interactRight.isSelecting = true;
        }
        else if (triggerValue2 <= 0.1f)
        {
            interactRight.isSelecting = false;
            interactRight.isHolding = false;
        }
    }
}
