//Author: Pearson Lawrence
//Purpose: This script is meant to move the lobby pillars towards the player after they have selected a lobby.
//This also stores the lobby information to provide a lobby code that players can use to join lobbies.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UIElements;

public class LobbyPillarComponent : MonoBehaviour
{
    //This is the lobby information that is propagated every 15 seconds by unity lobbies system through the LobbyInfoComponent and GameLobbyComponent
    [SerializeField] private LobbyInfoComponent lobby; 

    [SerializeField] private bool isSelected;
    [SerializeField] private bool isOriented;
    [SerializeField] private bool isPosition;
    [SerializeField] private bool isJoined;

    [SerializeField] private LobbyUIManager lobbyManager;

    
    [SerializeField] private float degPos; //Stores the position that the pillar is on around the center parent object set manually
    [SerializeField] private float moveSpeed = 20f;

    //Delay timers for movement
    [SerializeField] private float pauseTime = 3, pauseTimeEventOne = 2, pauseTimeMax = 3;

    //Sets a bool to true that moves the pillar towards the center
    public void beginJoin(bool oriented)
    {
        if(!isOriented && isSelected)
        {
            Debug.Log("Starting");
            //
            isOriented = oriented;
            
        }
    }

    //---Getters and setters---//
    public LobbyInfoComponent getLobbyInfoComponent()
    {
        return lobby;
    }
    public void setLobbyInfoComponent(LobbyInfoComponent lobbyInfo)
    {
        lobby = lobbyInfo;
    }
    public bool getIsSelected()
    {
        return isSelected;
    }
    public void setIsSelected(bool val)
    {
        isSelected = val;
    }
    public bool getIsOriented()
    {
        return isOriented;
    }
    public void setIsOriented(bool val)
    {
        isOriented = val;
    }
    public bool getIsPosition()
    {
        return isOriented;
    }
    public void setIsPosition(bool val)
    {
        isPosition = val;
    }
    public LobbyUIManager getLobbyUIManager()
    {
        return lobbyManager;
    }
    public void setLobbyUIManager(LobbyUIManager manager)
    {
        lobbyManager = manager;
    }
    public float getDegPos()
    {
        return degPos;
    }
    public void setDegPos(float deg)
    {
        degPos = deg;
    }
    //----------------------------//


    // Start is called before the first frame update
    void Start()
    {
        lobby.setPillar(this);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(0, .2f, 1f);

        //If a pillar is selected and has not reached the center and is properly oriented then move the pillar to the center
        if (isOriented && isSelected &&!isPosition)
        {
            pauseTime -= Time.deltaTime;
            if(pauseTime <= pauseTimeEventOne)
            {
                lobby.OpenDoor(); //Breaks the stone to show lobby selection
            }
            if(pauseTime <= 0)
            {
                //Moves towards parents center position based off an offset
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, Time.deltaTime * moveSpeed);
                if (transform.localPosition.z >= .95 && transform.localPosition.z <= 1 && !isPosition)
                {
                    //Once in position it stops.
                    isPosition = true;
                }
            }
        }

       
    }
}
