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
    [SerializeField] private List<LobbyInfoComponent> lobbyInfoComponents;
    [SerializeField] private List<Lobby> lobbies = new List<Lobby>();
    [SerializeField] private int currentLobbyCount;
    [SerializeField] private int maxDisplayLobbies;
    [SerializeField] private GameObject player;
    [SerializeField] private PillarLogic HostPillar, GuestPillar;
    [SerializeField] private GameObject tpPos1;
    [SerializeField] private PlatformDialComponent platDial;
    [SerializeField] private float lobbyUpdateTime = 5;
    private float lobbyUpdateTimer;
    bool isJoin, isClient;
    public PlatformDialComponent getPlatformDial()
    {
        return platDial;
    }
    //called when user selects create button
    //

    public List<LobbyInfoComponent> getLobbyInfoComponents()
    {
        return lobbyInfoComponents;
    }

    public void setSelectedLobby(LobbyInfoComponent lobby)
    {
        gameLobby.setSelectedLobby(lobby);
    }

    public void resetLobbyList()
    {
        if (lobbies == null || lobbies.Count == 0) return;

        foreach(LobbyInfoComponent lobby in lobbyInfoComponents)
        {
            lobby.clearInfo();
            lobbies.Clear();
            if(currentLobbyCount > 0)
                currentLobbyCount--;
        }
    }
    public void updateLobbyList(QueryResponse queryResponse)
    {
        resetLobbyList();

        currentLobbyCount = lobbyInfoComponents.Count;

        // Iterate through each lobby in the response
        foreach (Lobby lobby in queryResponse.Results)
        {
            lobbies.Add(lobby);
            if (lobby.Players.Count < lobby.MaxPlayers)
            {
                // Handle cases where the lobby code is not available
                string lobbyCode = string.IsNullOrEmpty(lobby.LobbyCode) ? "Not Available" : lobby.LobbyCode;
                Debug.Log("Lobby Name: " + lobby.Name + ", Lobby Code: " + lobbyCode);

                bool foundIsNotActive = false;
                for(int i = 0; !foundIsNotActive && i < lobbyInfoComponents.Count; i++)
                {
                    if (lobbyInfoComponents[i].isActive == false)
                    {
                        foundIsNotActive = true;
                        lobbyInfoComponents[i].setLobby(lobby);
                        lobbyInfoComponents[i].setGameLobby(gameLobby);
                        lobbyInfoComponents[i].isActive = true;
                        lobbyInfoComponents[i].activateLobby();
                        print(lobbyInfoComponents[i].lobbyName.text);
                        Debug.Log(lobby.LobbyCode);
                        //print(lobby.Players[0].Data["PlayerName"].Value);

                        lobbyInfoComponents[i].lobbyName.text = lobby.Name;
                    }
                }

                // Instantiate and setup the LobbyInfoComponent
               // LobbyInfoComponent temp = Instantiate(lobbyInfoPrefab, listContainer.transform).GetComponent<LobbyInfoComponent>(); //TODO: Refactor out
                //temp.setLobby(lobby);
                //lobbyUIList.Add(temp.gameObject);
                //temp.setGameLobby(this);
            }
        }
    }
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
                gameLobby.JoinSelectedLobby(player.GetComponent<UnNetworkPlayer>());
                break;
        }
    }
    
    public void doBeginGame()
    {
        gameLobby.StartGame();
    }

    public void doJoin()
    {
        player.transform.position = tpPos1.transform.position;
        player.transform.rotation = tpPos1.transform.rotation;
        gameLobby.JoinSelectedLobby(player.GetComponent<UnNetworkPlayer>());
        isJoin = true;
        isClient = true;
    }

    private void Update()
    {
        lobbyUpdateTimer -= Time.deltaTime;

        if(gameLobby.getIsLobbyStart()) //if lobby started then disable this UI related to lobby
        {
            this.gameObject.SetActive(false);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;
        Physics.Raycast(ray, out hitPoint, 1000);

        if (hitPoint.collider != null)
        {
            LobbyInfoComponent tempComp = hitPoint.collider.gameObject.GetComponent<LobbyInfoComponent>();
            if (tempComp && Input.GetMouseButtonDown(0))
            {
                setSelectedLobby(tempComp);
                platDial.setSelectedPillar(tempComp.getPillar());
                platDial.setIsLobbySelected(true);
                tempComp.getPillar().setIsSelected(true);
                //
            }

            if (tempComp)
            {
                if (tempComp.getPillar().getIsPosition() && Input.GetMouseButtonDown(0))
                {
                    player.transform.position = tpPos1.transform.position;
                    gameLobby.JoinSelectedLobby(player.GetComponent<UnNetworkPlayer>());
                }
            }
            
        }
        if(Input.GetKeyDown(KeyCode.J) && gameLobby.getSelectedLobby())
        {
            gameLobby.JoinSelectedLobby(player.GetComponent<UnNetworkPlayer>());
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            gameLobby.CreateLobby();
            player.transform.position = HostPillar.playerPoint.transform.position;
            player.GetComponent<UnNetworkPlayer>();
            isJoin = true;
        }
        if (lobbyUpdateTimer <= 0 && !isJoin)
        {
            gameLobby.RefreshLobbyList();
            lobbyUpdateTimer = lobbyUpdateTime;
        }
        if(gameLobby.getIsJoined() && isClient)
        {
            player.GetComponent<UnNetworkPlayer>().currentPillar = GuestPillar;
            player.GetComponent<UnNetworkPlayer>().isTeleported = true;
        }

    }
    public void doCreate()
    {
        gameLobby.CreateLobby();
        player.transform.position = HostPillar.playerPoint.transform.position;
        UnNetworkPlayer playerController = player.GetComponent<UnNetworkPlayer>();
        playerController.currentPillar = HostPillar;
        playerController.currentPillar = HostPillar;
        isJoin = true;
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
