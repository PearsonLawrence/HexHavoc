using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class LobbyPillarComponent : MonoBehaviour
{
    [SerializeField] private LobbyInfoComponent lobby;
    [SerializeField] private bool isSelected;
    [SerializeField] private bool isOriented;
    [SerializeField] private bool isPosition;
    [SerializeField] private bool isJoined;
    [SerializeField] private LobbyUIManager lobbyManager;
    [SerializeField] private float degPos;
    [SerializeField] private float moveSpeed = 20f;

    public void beginJoin(bool oriented)
    {
        if(!isOriented && isSelected)
        {
            Debug.Log("Starting");
            //
            isOriented = oriented;
            
        }
    }
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

    // Start is called before the first frame update
    void Start()
    {
        lobby.setPillar(this);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(0, .2f, 1f);
        if (isOriented && isSelected &&!isPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, Time.deltaTime * moveSpeed);
            if(transform.localPosition.z >= .95 && transform.localPosition.z <= 1 && !isPosition)
            {
                isPosition = true;
            }
        }

       
    }
}
