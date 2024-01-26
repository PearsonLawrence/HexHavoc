//Author: Pearson Lawrence
//Purpose: This script handles the player interaction to Unity lobby execution. It gives players the tools to join or create whatever game they want. This is the main component that handles the corraspondance between the user and lobby.
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

    //called when user selects create button 
   public void selectCreateLobby()
    {
        gameLobby.CreateLobby(); //Creates game lobby from GameLobbyComponent Class

        //Disable and enable correct UI
        defaultScreen.SetActive(false);
        joinLobbyScreen.SetActive(false);
        lobbyScreen.SetActive(true);
    }

    //Called when user presses join button
    public void selectJoinLobby()
    {
        //Takes user to lobby list UI
        defaultScreen.SetActive(false);
        joinLobbyScreen.SetActive(true);
        lobbyScreen.SetActive(false);
    }

    //Called when player hits start button. Starts the game
    public void lobbyPlayStart()
    {
        if (gameLobby.getJoinedLobby().HostId == gameLobby.getPlayerID()) //if the user pressing the button is the host then start
        {                                                                 //TODO: Remove button option from players who are not the host
            gameLobby.StartGame(); //Starts the game and connects relay
            gameLobbyParent.gameObject.SetActive(false); //Turns off menu UI and removes it from the world so game can start
        }


    }

    //Button press that returns player to the main menu when they are connected in a lobby
    //TODO: If host quits migrate host. If all players quit then delete lobby
    public void QuitLobby()
    {
        gameLobby.quitLobby();
        defaultScreen.SetActive(true);
        joinLobbyScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }

    //Calles the appropriate join function depending on what button is pressed.
    public void joinSelectedLobby(int type)
    {
        defaultScreen.SetActive(false);
        joinLobbyScreen.SetActive(false);
        lobbyScreen.SetActive(true);

        //organize which join was pressed
        //TODO: switch to enum type
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
        if(gameLobby.getIsLobbyStart()) //if lobby started then disable this UI related to lobby
        {
            this.gameObject.SetActive(false);
        }

    }
    //Turn on basic create or join UI
    public void enableLobbyOptions(){
        defaultScreen.SetActive(true);
        joinLobbyScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }
    //Return to main menu
    public void disableLobbyOptions(){
        defaultScreen.SetActive(false);
        joinLobbyScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }
}
