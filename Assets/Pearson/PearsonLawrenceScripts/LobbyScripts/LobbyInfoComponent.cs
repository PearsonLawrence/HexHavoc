//Author:Pearson Lawrence
//The purpose of this script was used to display lobbies and their information inside of the lobbies list that was available when searching for games. This script allows for all the information to be stored so a lobby can be joined easily.
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;

public class LobbyInfoComponent : MonoBehaviour
{
    //UI elements
    [SerializeField] private Button selectLobbyButton;
    [SerializeField] private TMP_Text info;
    [SerializeField] private Image privateIndicator;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject Rocks;
    //Lobby elements
    [SerializeField] private GameLobbyComponent GameLobby; //stores all of the information of the lobby listed
    [SerializeField] private LobbyPillarComponent Pillar; //stores all of the information of the lobby listed
    [SerializeField] private Lobby currentLobbyInfo; // Current selected lobby
    [SerializeField] private Renderer doorRender; // Current selected lobby
    [SerializeField] private bool isJoined; // Current selected lobby

    public TMP_Text lobbyName;

    private int playerCount, maxPlayerCount;
    [SerializeField] private string lobbyCode; //This is the lobby code that will update the users desired join lobby
    public bool isActive = false;

    public void OpenDoor()
    {
        door.SetActive(false);
        Rocks.SetActive(true);
    }
    public string getLobbyName()
    {
        return lobbyName.text;
    }
    public GameLobbyComponent getGameLobby()
    {
        return GameLobby;
    }
    public LobbyPillarComponent getPillar()
    {
        return Pillar;
    }
    public void setPillar(LobbyPillarComponent p)
    {
        Pillar = p;
    }

    public void clearInfo()
    {
        //info.text = "";
        currentLobbyInfo = null;
        playerCount = 0;
        maxPlayerCount = 0;
        lobbyCode = "";
        isActive = false;
        //doorRender.enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
    public void activateLobby()
    {
        //doorRender.enabled = true;
        this.gameObject.GetComponent<Collider>().enabled = true;
    }
    public Lobby getCurrentLobby()
    {
        return currentLobbyInfo;
    }
    public int getPlayerCount()
    {
        return playerCount;
    }
    public void setPlayerCount(int count)
    {
        playerCount = count;
    }
    public int getMaxPlayerCount()
    {
        return maxPlayerCount;
    }
    public void setGameLobby(GameLobbyComponent gamelobby)
    {
        GameLobby = gamelobby;
    }
    public void setGameLobbyJoinCode()
    {
        GameLobby.setSelectedLobby(this);
    }

    //updates the lobby information of the displayed (this) lobby in the lobby list.
    public void setLobby(Lobby lobby)
    {
        currentLobbyInfo = lobby;
        maxPlayerCount = currentLobbyInfo.MaxPlayers;
        playerCount = currentLobbyInfo.Players.Count;
        lobbyCode = lobby.LobbyCode;

        //TODO: Get to work and allow private matches
        //if (currentLobbyInfo.IsPrivate)
            //privateIndicator.color = new Color(100,0,0);
        //else
            //privateIndicator.color = new Color(0, 100, 0);

        //info.text = lobby.Data["GameMode"].Value + " | " + lobby.Data["Map"].Value + " | " + playerCount + "/" + maxPlayerCount; //Displaying info
        //TODO: Make pretty, and display lobby name
    }
    public void setMaxPlayerCount(int count)
    {
        maxPlayerCount = count;
    }
    public void setTextDisplay(string lobbyInfo)
    {
        info.text = lobbyInfo;
    }
    public string getLobbyCode()
    {
        return lobbyCode;
    }
    public void setLobbyCount(string code)
    {
        lobbyCode = code;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("LobbySelect"))
        {
            Debug.Log("test");
            Pillar.getLobbyUIManager().setSelectedLobby(this);
            Pillar.getLobbyUIManager().getPlatformDial().setSelectedPillar(Pillar);
            Pillar.getLobbyUIManager().getPlatformDial().setIsLobbySelected(true);
            Pillar.setIsSelected(true);
        }
    }
}
