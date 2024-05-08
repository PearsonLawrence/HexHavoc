//Author: Pearson Lawrence
//Purpose: This script is used to assign move values to the PlatformDialComponent utilizing a gameobject drag system
//to determine which direction to rotate the pillars. This script acts as a joystick.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialFingerPointComponent : MonoBehaviour
{
    private bool isTouching = false;

    //Takes in a platform Dial component to directly modify
    [SerializeField] private PlatformDialComponent dial; 

    //Holding Point to calculate direction
    [SerializeField] private GameObject parent;

    //Movement Indicators
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isMoving, isRight, isLeft, isUp, isDown;

    //Getter
    public bool getIsTouching()
    {
        return isTouching;
    }

    //Setter
    public void setIsTouching(bool val)
    {
        isTouching = val;
    }

    //Reset to inital point with local position
    public void doReset()
    {
        transform.localPosition = Vector3.zero;
        dial.setIsRight(false);
        dial.setIsLeft(false);
        dial.setIsUp(false);
        dial.setIsDown(false);

    }

    //If the player is grabbing this call this function that tracks where the ball has moved relative local to the parents position
    //Modifies the dial component
    public void doPointHold()
    {
        //If it has been moved
        if (transform.localPosition != Vector3.zero)
        {
            //If it is dragged right
            if (transform.localPosition.x > .025f)
            {
                dial.setIsRight(true);
                dial.setIsLeft(false);
            }
            //If it is dragged Left
            else if (transform.localPosition.x < -.025f)
            {
                dial.setIsRight(false);
                dial.setIsLeft(true);
            }
            //If it is in the center
            else
            {
                dial.setIsRight(false);
                dial.setIsLeft(false);
            }

            //If it is dragged up
            if (transform.localPosition.z > .05f)
            {
                dial.setIsUp(false);
                dial.setIsDown(true);
            }
            //If it is dragged down
            else if (transform.localPosition.z < -.05f)
            {
                dial.setIsUp(true);
                dial.setIsDown(false);
            }
            //If it is center
            else
            {
                dial.setIsUp(false);
                dial.setIsDown(false);
            }


        }
        //If not holding
        else
        {

            dial.setIsRight(false);
            dial.setIsLeft(false);
            dial.setIsUp(false);
            dial.setIsDown(false);

        }
        //Debug.Log(dial.getIsDown() + " : " + dial.getIsUp() + " : " + dial.getIsRight() + " : " + dial.getIsLeft());
    }
    public void Update()
    {
       // MouseTester();
    }

}
