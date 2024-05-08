//Author: Pearson Lawrence
//Purpose: This script displays all the information of players once they join a lobby
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
//TODO: Dispaly steam name instead of this
public class PlayerInfoCardComponent : MonoBehaviour
{
    [SerializeField] private Button selectLobbyButton;
    [SerializeField] private GameLobbyComponent GameLobby;
    [SerializeField] private TMP_Text info;
    [SerializeField] private Image privateIndicator;
    [SerializeField] private Lobby lobbyInfo;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject rockExplode;

    private int playerCount, maxPlayerCount;
    [SerializeField] private string lobbyCode;

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
   
        //Stores the player information
    public void setPlayerInfo(Player player)
    {
        if (player == null)
        {
            info.text = "no player";
            return;
        }
        rock.SetActive(false);
        rockExplode.SetActive(true);

        info.text = player.Data["PlayerName"].Value;
    }
   
    public void setTextDisplay(string lobbyInfo)
    {
        info.text = lobbyInfo;
    }
}
