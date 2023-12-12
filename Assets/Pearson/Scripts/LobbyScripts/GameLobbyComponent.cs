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
    public string playerName;
    private string tempCode;

    [SerializeField] private GameObject listContainer;
    [SerializeField] private GameObject XR_Player;
    [SerializeField] private GameObject lobbyListContainer;
    [SerializeField] private List<GameObject> lobbyUIList;
    [SerializeField] private List<GameObject> playerLobbyUIList;
    [SerializeField] private GameObject lobbyInfoPrefab;
    [SerializeField] private GameObject playerLobbyInfoPrefab;
    [SerializeField] private LobbyInfoComponent selectedLobby;
    [SerializeField] private gameRelayComponent currentRelay;

    public TMP_Text textTemp;
    public TMP_Text lobbyCountText;
    public TMP_InputField CodeInputField;
    private bool isLobbyStart;
    private string playerID;
    bool isCreated = false;
    public bool getIsLobbyStart()
    {
        return isLobbyStart;
    }
    public Lobby getJoinedLobby()
    {
        return joinedLobby;
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
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            playerID = AuthenticationService.Instance.PlayerId;
            Debug.Log("Signed in " + playerID);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "TestName" + UnityEngine.Random.Range(0, 1000);
        Debug.Log(playerName);
    }

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

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "Duel", DataObject.IndexOptions.S1) },
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, "Arena1", DataObject.IndexOptions.S2) },
                    { "StartGame", new DataObject(DataObject.VisibilityOptions.Member, "0", DataObject.IndexOptions.S3) },
                }

            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            setLobbyCode(lobby.LobbyCode);

            
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);

            hostLobby = lobby;
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void StartGame()
    {
        Debug.Log("Run");
        Debug.Log(joinedLobby.HostId);
        Debug.Log(playerID);
        if (joinedLobby.HostId == playerID)
        {
            Debug.Log("IsHost");
            try
            {
                if (XR_Player)
                    XR_Player.SetActive(false);
                Debug.Log("StartGame");
                string relayCode = await currentRelay.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { "StartGame", new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });
                joinedLobby = lobby;
                isLobbyStart = true;

            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                isLobbyStart = false;
            }
        }
    }

    private void setLobbyCode(string code)
    {
        tempCode = code;
    }

    public void Refresh()
    {
        while (lobbyUIList.Count > 0)
        {
            GameObject temp = lobbyUIList[0];
            lobbyUIList.Remove(temp);
            Destroy(temp);
        }
        
        ListLobbies();
    }
    public void RefreshLobby()
    {
        while (playerLobbyUIList.Count > 0)
        {
            GameObject temp = playerLobbyUIList[0];
            playerLobbyUIList.Remove(temp);
            Destroy(temp);
        }
        if (joinedLobby == null) return;

        setupLobby(joinedLobby);
        lobbyCountText.text = playerLobbyUIList.Count + "/" + joinedLobby.MaxPlayers;
    }
    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
            },
                Order = new List<QueryOrder>
            {
                new QueryOrder(false, QueryOrder.FieldOptions.Created)
            }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);

            // Iterate through each lobby in the response
            foreach (Lobby lobby in queryResponse.Results)
            {
                // Handle cases where the lobby code is not available
                string lobbyCode = string.IsNullOrEmpty(lobby.LobbyCode) ? "Not Available" : lobby.LobbyCode;
                Debug.Log("Lobby Name: " + lobby.Name + ", Lobby Code: " + lobbyCode);

                // Instantiate and setup the LobbyInfoComponent
                LobbyInfoComponent temp = Instantiate(lobbyInfoPrefab, listContainer.transform).GetComponent<LobbyInfoComponent>();
                temp.setLobby(lobby);
                lobbyUIList.Add(temp.gameObject);
                temp.setGameLobby(this);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Error querying lobbies: " + e);
        }


    }

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
    public void LobbyJoin()
    {
        JoinLobbyByCode(selectedLobby.getLobbyCode());
    }public void JoinLobbyByCodeInputUI()
    {
        JoinLobbyByCode(CodeInputField.text);
    }

    public void setupLobby(Lobby lobbyJoinedInfo)
    {
       // Debug.Log("Players in lobby " + lobbyJoinedInfo.Name + " " + lobbyJoinedInfo.Data["GameMode"].Value + " " + lobbyJoinedInfo.Data["Map"].Value);
        foreach (Player player in lobbyJoinedInfo.Players)
        {
            PlayerInfoCardComponent temp = Instantiate(playerLobbyInfoPrefab, lobbyListContainer.transform).GetComponent<PlayerInfoCardComponent>();
            temp.setPlayerInfo(player);
            playerLobbyUIList.Add(temp.gameObject);
            temp.setGameLobby(this);
        }
    }
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            PrintPlayers(joinedLobby);
            Debug.Log("joined lobby with code " + lobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public void JoinSelectedLobby()
    {
        JoinLobbyByLobby(selectedLobby.getCurrentLobby());
    }
    private async void JoinLobbyByLobby(Lobby lobby)
    {
        Player player = GetPlayer();

        Lobby tempLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions { Player = player });
        joinedLobby = tempLobby;
        PrintPlayers(joinedLobby);
        Debug.Log("joined lobby with code " + joinedLobby.LobbyCode);


    }

    private async void handleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    public void QuickJoin()
    {
        QuickJoinLobby();
    }

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
    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 2f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;

                if (joinedLobby.Data["StartGame"].Value != "0")
                {
                    if (playerID != lobby.HostId)
                    {
                        isLobbyStart = await currentRelay.JoinRelay(joinedLobby.Data["StartGame"].Value);
                        
                    }

                    joinedLobby = null;

                }

                RefreshLobby();
            }
        }
    }
    public void quitLobby()
    {
        LeaveLobby();
    }
    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            joinedLobby = null;
            hostLobby = null;
            Refresh();
            RefreshLobby();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

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
        HandleLobbyPollForUpdates();
        textTemp.text = tempCode;
       
    }

}
