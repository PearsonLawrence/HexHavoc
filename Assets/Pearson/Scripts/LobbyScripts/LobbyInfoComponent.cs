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

    [SerializeField] private Button selectLobbyButton;
    [SerializeField] private GameLobbyComponent GameLobby;
    [SerializeField] private TMP_Text info;
    [SerializeField] private Image privateIndicator;
    [SerializeField] private Lobby currentLobbyInfo;

    private int playerCount, maxPlayerCount;
    [SerializeField] private string lobbyCode;

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
    public void setLobby(Lobby lobby)
    {
        currentLobbyInfo = lobby;
        maxPlayerCount = currentLobbyInfo.MaxPlayers;
        playerCount = currentLobbyInfo.Players.Count;
        lobbyCode = lobby.LobbyCode;

        if (currentLobbyInfo.IsPrivate)
            privateIndicator.color = new Color(100,0,0);
        else
            privateIndicator.color = new Color(0, 100, 0);

        info.text = lobby.Data["GameMode"].Value + " | " + lobby.Data["Map"].Value + " | " + playerCount + "/" + maxPlayerCount;
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

}
