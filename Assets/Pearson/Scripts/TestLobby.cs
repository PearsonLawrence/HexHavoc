using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer = 1.1f;
    public string playerName;
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "TestName" + UnityEngine.Random.Range(0, 1000);
        Debug.Log(playerName);
    }

    private string tempCode;
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
    private async void CreateLobby()
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
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, "Arena1", DataObject.IndexOptions.S2) }
                }

            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            setLobbyCode(lobby.LobbyCode);
            
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode) ;

            hostLobby = lobby;
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private void setLobbyCode(string code)
    {
        tempCode = code;
    }
    private async void ListLobbies()
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
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);

            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
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
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
            };
            Lobby lobby =  await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            PrintPlayers(joinedLobby);
            Debug.Log("joined lobby with code " + lobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void handleLobbyHeartbeat()
    {
        if(hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
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
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
        }
    }

    private async void LeaveLobby()
    {
        try
        { 
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
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
            PrintPlayers();
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
public TMP_Text textTemp;
    public TMP_InputField IP;
    bool isCreated = false;
    private void Update()
    {
        handleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
        textTemp.text = tempCode;
        if (Input.GetKey(KeyCode.Comma))
        {
            if (Input.GetKeyDown(KeyCode.C) && !isCreated)
            {
                CreateLobby();
                isCreated = true;
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                ListLobbies();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                JoinLobbyByCode(IP.text);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                PrintPlayers();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                UpdateLobbyGameMode("TesterMode");
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                string tempName = "newName" + Random.Range(0, 100);
                UpdatePlayerName(tempName);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LeaveLobby();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                KickPlayer(1);
            }
        }
       
    }

}
