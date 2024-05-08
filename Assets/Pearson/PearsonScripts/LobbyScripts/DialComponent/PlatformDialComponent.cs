//Author: Pearson Lawrence
//Purpose: This component is responsible for rotating based off of directional movement. The lobby pillars are attached as children to this gameobject, so that they rotate around it as it rotates.
//This handles rotating the selected pillar to the correct position based off the pillar components degree around the center.
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformDialComponent : MonoBehaviour
{
    [SerializeField] private GameObject platformRotateObject;
    [SerializeField] private DialFingerPointComponent dialFingerPoint;

    //Directional bools to determine how to rotate and move
    [SerializeField] private bool isMoving, isRight, isLeft, isUp, isDown;
    [SerializeField] private float RotSpeed = 100, pointRotSpeed = 5;

    [SerializeField] private LobbyPillarComponent selectedPillar;
    [SerializeField] bool isLobbySelected;

    //--- Getters and setters ---
    public void setSelectedPillar(LobbyPillarComponent pillar)
    {
        selectedPillar = pillar;
    }
    public void setIsLobbySelected(bool val)
    {
        isLobbySelected = val;
    }
    public bool getIsMoving()
    {
        return isMoving;
    }
    public void setIsMoving(bool val)
    {
        isMoving = val;
    }
    public bool getIsRight()
    {
        return isRight;
    }
    public void setIsRight(bool val)
    {
        isRight = val;
    }
    public bool getIsLeft()
    {
        return isLeft;
    }
    public void setIsLeft(bool val)
    {
        isLeft = val;
    }
    public bool getIsUp()
    {
        return isUp;
    }
    public void setIsUp(bool val)
    {
        isUp = val;
    }
    public bool getIsDown()
    {
        return isDown;
    }
    public void setIsDown(bool val)
    {
        isDown = val;
    }
    //--------------------------------//

    // Update is called once per frame
    void Update()
    {
        //If anymovement bool then ismoving
        if (isRight || isLeft || isUp || isDown) isMoving = true;
        else isMoving = false;

        Vector3 rot = platformRotateObject.gameObject.transform.eulerAngles;
        
        //If a pillar has been selected rotate pillar to 0 degree position
        if(selectedPillar)
        {
            if (selectedPillar.getIsSelected())
            {
                rot.y = selectedPillar.getDegPos();

                //If Pillar is in position then start beginJoin so that pillar moves towards center.
                if (platformRotateObject.gameObject.transform.eulerAngles.y <= selectedPillar.getDegPos() + .1f && platformRotateObject.gameObject.transform.eulerAngles.y >= selectedPillar.getDegPos() - .1f)
                {
                    selectedPillar.beginJoin(true);
                }
            }
        }
        //If it is moving aka the joystick has been moved then rotate left or right
        else if (isMoving)
        {
            if(isRight)
            {
                rot = new Vector3(platformRotateObject.gameObject.transform.eulerAngles.x,
                                          platformRotateObject.gameObject.transform.eulerAngles.y + (RotSpeed * Time.deltaTime),
                                          platformRotateObject.gameObject.transform.eulerAngles.z);
            }
            else if(isLeft)
            {
                rot = new Vector3(platformRotateObject.gameObject.transform.eulerAngles.x,
                                  platformRotateObject.gameObject.transform.eulerAngles.y - (RotSpeed * Time.deltaTime),
                                  platformRotateObject.gameObject.transform.eulerAngles.z);
                
            }
        }
        platformRotateObject.gameObject.transform.eulerAngles = Vector3.MoveTowards(platformRotateObject.gameObject.transform.eulerAngles, rot, Time.deltaTime * pointRotSpeed);

       
        
        //platformRotateObject.gameObject.transform.eulerAngles = new Vector3(0, platformRotateObject.gameObject.transform.eulerAngles.y, 0);
    }
}
