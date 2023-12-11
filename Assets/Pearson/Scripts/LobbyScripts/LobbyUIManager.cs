using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    public GameLobbyComponent gameLobby;
    public GameObject gameLobbyParent;
    public GameObject defaultScreen, joinLobbyScreen, lobbyScreen;
   public void selectCreateLobby()
    {
        gameLobby.CreateLobby();
        defaultScreen.SetActive(false);
        joinLobbyScreen.SetActive(false);
        lobbyScreen.SetActive(true);
    }
    public void selectJoinLobby()
    {
        defaultScreen.SetActive(false);
        joinLobbyScreen.SetActive(true);
        lobbyScreen.SetActive(false);
    }
    public void lobbyPlayStart()
    {
        if (gameLobby.getJoinedLobby().HostId == gameLobby.getPlayerID())
        {
            gameLobby.StartGame();
            gameLobbyParent.gameObject.SetActive(false);
        }


    }
    public void QuitLobby()
    {
        gameLobby.quitLobby();
        defaultScreen.SetActive(true);
        joinLobbyScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }
    public void joinSelectedLobby(int type)
    {
        defaultScreen.SetActive(false);
        joinLobbyScreen.SetActive(false);
        lobbyScreen.SetActive(true);
        switch(type)
        {
            case 0:
                gameLobby.JoinLobbyByCodeInputUI();
                break;
            case 1:
                gameLobby.QuickJoin();
                break;
            case 2:
                gameLobby.JoinSelectedLobby();
                break;
        }
    }
    private void Update()
    {
        if(gameLobby.getIsLobbyStart())
        {
            this.gameObject.SetActive(false);
        }
    }
}
