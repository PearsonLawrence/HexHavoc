//Author: Pearson
//Purpose: Utilized Unity lobby and UI elements to allow players to create lobbies, and join them. This script contains the main functionality behind managing networked lobbies and opening relays to begin matches.
//Utilized basic tutorial from code monkey to understand fundamentals and then added on and created entire UI system from scratch.
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class GameLobbyComponent : MonoBehaviour
{
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer = 1.1f;
    private string tempCode;

    public string playerName;

    [SerializeField] private GameObject listContainer;
    [SerializeField] private GameObject XR_Player;
    [SerializeField] private GameObject lobbyListContainer;
    [SerializeField] private List<GameObject> lobbyUIList;
    [SerializeField] private List<PlayerInfoCardComponent> playerLobbyUIList;
    [SerializeField] private GameObject lobbyInfoPrefab;
    [SerializeField] private GameObject playerLobbyInfoPrefab;
    [SerializeField] private LobbyInfoComponent selectedLobby;
    [SerializeField] private LobbyUIManager lobbyManager;
    [SerializeField] private gameRelayComponent currentRelay;
    [SerializeField] private float lobbyHeartbeatTimerMax = 15f;
    [SerializeField] private float lobbyUpdateTimerMax = 2f;
    [SerializeField] private bool isJoined = false;

    public TMP_Text textTemp;
    public TMP_Text lobbyCountText;
    public TMP_InputField CodeInputField;
    
    private bool isLobbyStart;
    private string playerID;
    //bool isCreated = false;
    public bool getIsLobbyStart()
    {
        return isLobbyStart;
    }
    public Lobby getJoinedLobby()
    {
        return joinedLobby;
    }
    public bool getIsJoined()
    {
        return isJoined;
    }

    public string getPlayerID()
    {
        return playerID;
    }

    public LobbyInfoComponent getSelectedLobby()
    {
        return selectedLobby;
    }

    public void setSelectedLobby(LobbyInfoComponent select)
    {
        selectedLobby = select;
    }

    //Async start when lobby is created
    private async void Start()
    {
        //await for initialize from unity services
        await UnityServices.InitializeAsync();

        //Get user signed in authentication (Return player id not name)
        AuthenticationService.Instance.SignedIn += () =>
        {
            playerID = AuthenticationService.Instance.PlayerId;
            Debug.Log("Signed in " + playerID);
        };

        //signs the user in as an anonymous user (Without going through steam authentication)
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //assign player random name (Temporary) TODO: implement steam names or custom names
        playerName = "TestName" + UnityEngine.Random.Range(0, 1000);
        Debug.Log(playerName);
    }

    //debug to output current players in lobby
    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value + " " + lobby.Data["Map"].Value);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    private void PrintPlayers()
    {
        PrintPlayers(joinedLobby);
    }

    //Create instance of lobby over unity lobbies that people can find
    public async void CreateLobby()
    {
        try
        {
            string lobbyName = playerName + "'s Arena"; //TODO: Custom lobby name input instead of mylobby
            int maxPlayers = 4; //TODO: User sets value to max of game requirements


            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "Duel", DataObject.IndexOptions.S1) }, //TODO: Allow player to choose mode
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, "Arena1", DataObject.IndexOptions.S2) }, //TODO: Allow player to choose MAP
                    { "StartGame", new DataObject(DataObject.VisibilityOptions.Member, "0", DataObject.IndexOptions.S3) }, //TODO: Remember what this is (Relay code?)
                }

            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions); //wait for lobby to be created
            setLobbyCode(lobby.LobbyCode); //Creates lobby code and stores it in class variable

            
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);

            //sync up host and joined lobby across clients in lobby
            hostLobby = lobby;
            joinedLobby = hostLobby;
            //TODO: Reactivate
            //PrintPlayers(hostLobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //When start is clicked connect lobby clients to relay
    public async void StartGame()
    {
        Debug.Log("Run");
        Debug.Log(joinedLobby.HostId);
        Debug.Log(playerID);

        //if not Host of lobby (So host only leave the lobby when non hosts join the game)
        if (joinedLobby.HostId == playerID) 
        {
            Debug.Log("IsHost");
            try
            {

                Debug.Log("StartGame");
                string relayCode = await currentRelay.CreateRelay(); //Create a relay to connect clients to host 

                //Update lobby options for all players. Update relay code to allow users to connect to correct relay
                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { "StartGame", new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });

                XR_Player.GetComponent<UnNetworkPlayer>().isConnected = true;
                joinedLobby = lobby;
                isLobbyStart = true; //If true this will intiate connecting to host relay on next tick

            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                isLobbyStart = false;
            }
        }
    }

    //Sets the lobby code variable to store and display
    private void setLobbyCode(string code)
    {
        tempCode = code;
    }

    //Updates the list of open lobbies that players can join that are not full
    public void RefreshLobbyList()
    {
        //remove lobbies from list to update
        while (lobbyUIList.Count > 0)
        {
            //GameObject temp = lobbyUIList[0];
            //lobbyUIList.Remove(temp);
            //Destroy(temp);
        }
        
        ListLobbies();//Re-add lobbies to list
    }
    //Create ui elements with players joined in lobby
    public void setupLobby(Lobby lobbyJoinedInfo)
    {
        // Debug.Log("Players in lobby " + lobbyJoinedInfo.Name + " " + lobbyJoinedInfo.Data["GameMode"].Value + " " + lobbyJoinedInfo.Data["Map"].Value);
        for (int i = 0; i < lobbyJoinedInfo.Players.Count; i++)// Player player in lobbyJoinedInfo.Players)
        {
            if (i < playerLobbyUIList.Count)
                playerLobbyUIList[i].setPlayerInfo(lobbyJoinedInfo.Players[i]);
          /* PlayerInfoCardComponent temp = Instantiate(playerLobbyInfoPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerInfoCardComponent>();
           temp.setPlayerInfo(player);
           playerLobbyUIList.Add(temp.gameObject);
           temp.setGameLobby(this);*/
        }
    }

    //Updates list of current players connected to lobby
    //TODO: Refactor out into sperate class that manages the lobby once connected
    public void RefreshPlayerList()
    {
        //if we have not joined a lobby do nothing
        if (joinedLobby == null) return;
        //while there are players in the lobby, update
        //while (playerLobbyUIList.Count > 0)
        for(int i = 0; i < playerLobbyUIList.Count; i++)
        {
           playerLobbyUIList[0].setPlayerInfo(null);
           // playerLobbyUIList.Remove(temp);
           // Destroy(temp);
        }

        //add players to UI list
        setupLobby(joinedLobby);

        //Display how many players are connected
        //lobbyCountText.text = playerLobbyUIList.Count + "/" + joinedLobby.MaxPlayers;
    }
    public async void ListLobbies()
    {
        try
        {
            //Gets lobbies that are open with available slots
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
            },
                Order = new List<QueryOrder> //TODO: Remember what dis do
            {
                new QueryOrder(false, QueryOrder.FieldOptions.Created)
            }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions); //Getting the query to all available lobbies

            Debug.Log("Lobbies found: " + queryResponse.Results.Count); //shows how many lobbies are found
            lobbyManager.updateLobbyList(queryResponse);
            /*
            // Iterate through each lobby in the response
            foreach (Lobby lobby in queryResponse.Results)
            {
                // Handle cases where the lobby code is not available
                string lobbyCode = string.IsNullOrEmpty(lobby.LobbyCode) ? "Not Available" : lobby.LobbyCode;
                Debug.Log("Lobby Name: " + lobby.Name + ", Lobby Code: " + lobbyCode);

                // Instantiate and setup the LobbyInfoComponent
                LobbyInfoComponent temp = Instantiate(lobbyInfoPrefab, listContainer.transform).GetComponent<LobbyInfoComponent>(); //TODO: Refactor out
                temp.setLobby(lobby);
                lobbyUIList.Add(temp.gameObject);
                temp.setGameLobby(this);
            }*/
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Error querying lobbies: " + e);
        }


    }

    //returns player data
    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    {
                        { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) },
                    }
        };
    }
    //Function to be called on button click event to join selected lobby
    public void LobbyJoin()
    {
        JoinLobbyByCode(selectedLobby.getLobbyCode());
    }

    //TODO: Create VR keyboard to allow joining by code
    public void JoinLobbyByCodeInputUI() // manually joins by entered code
    {
        JoinLobbyByCode(CodeInputField.text);
    }

    //Connects client to desired lobby by specified code passed into function
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
            }; //Passes in the player to be connected to lobby

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions); //Actually connect via unity lobby services on await
            joinedLobby = lobby;
            PrintPlayers(joinedLobby);
            Debug.Log("joined lobby with code " + lobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    //Join lobby that the user selects in the list
    public bool JoinSelectedLobby(UnNetworkPlayer P)
    {
        Debug.Log(selectedLobby.getCurrentLobby());
        JoinLobbyByLobby(selectedLobby.getCurrentLobby(), P);
        return true;
    }
    public async void JoinLobbyByLobby(Lobby lobby, UnNetworkPlayer P)
    {
        //get player info that is joining
        Player player = GetPlayer();

        //await and connect user to desired lobby passed into function
        Lobby tempLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions { Player = player });
        joinedLobby = tempLobby;
        PrintPlayers(joinedLobby);
        isJoined = true;
        Debug.Log("joined lobby with code " + joinedLobby.LobbyCode);


    }

    //This updates the lobby infoirmation with frequency of lobbyHeartbeatTimerMax variable
    private async void handleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                heartbeatTimer = lobbyHeartbeatTimerMax;
                Debug.Log("Beat: " + joinedLobby.Players.Count);
                if(joinedLobby.Players.Count > 1)
                {
                    StartGame();
                }

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);

            }
        }
    }

    //Join random lobby is current found lobbies
    public void QuickJoin()
    {
        QuickJoinLobby();
    }

    //Quickjoin non private open lobby 
    //TODO: Test and implement and refactor
    private async void QuickJoinLobby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //update gamemode of lobby
    //TODO: Implement other gamemodes? If not no need
    private async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });
            joinedLobby = hostLobby;
            PrintPlayers();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Updates the name of player
    //TODO: Have custom names? Or steam name?
    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Updates list of players in the lobby
    private async void HandleCurrentLobbyPollForUpdates()
    {
        if (joinedLobby != null) //If you are connected to a lobby
        {
            RefreshPlayerList();
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                lobbyUpdateTimer = lobbyUpdateTimerMax;
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;

                if (joinedLobby.Data["StartGame"].Value != "0") //if game has not started
                {

                    RefreshPlayerList();

                    if (playerID != lobby.HostId) //user is not the host
                    {
                        isLobbyStart = await currentRelay.JoinRelay(joinedLobby.Data["StartGame"].Value); //update lobby start
                        XR_Player.GetComponent<UnNetworkPlayer>().isConnected = true;
                    }
                    joinedLobby = null;

                }

            }
        }
    }
    public void quitLobby() //Quit lobby UI event button
    {
        LeaveLobby();
    }

    //disconnect from lobby
    //TODO: Fix bug that prevents user from joining new lobby
    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            joinedLobby = null;
            hostLobby = null;
            RefreshLobbyList();
            RefreshPlayerList();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //TODO: Test and give host option to kick players
    private async void KickPlayer(int playerIndex)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[playerIndex].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //TODO: Test
    private async void MigrateLobbyHost()
    {
        try
        {

            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id,
            });
            joinedLobby = hostLobby;
            //PrintPlayers();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Removes lobby from being findable
    //TODO: Implement to fix bug of lobby not being deleted
    private void DeleteLobby()
    {
        try
        {
            LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void Update()
    {
        handleLobbyHeartbeat();
        HandleCurrentLobbyPollForUpdates();
        //textTemp.text = tempCode; //update the lobby code UI
       
    }

}
