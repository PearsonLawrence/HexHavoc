using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MatchManager : NetworkBehaviour
{
    private Dictionary<ulong, HealthManager> playerHealthDict = new Dictionary<ulong, HealthManager>();

    // Singleton instance
    public static MatchManager Instance;

    void Awake()
    {
        // Ensure there is only one instance of the MatchManager
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Called to register a player
    public void RegisterPlayer(ulong clientId)
    {
        // You can perform additional logic here based on the spawned player
        Debug.Log($"Player registered: {clientId}");

        // Instantiate the HealthManager and store it in the dictionary
        GameObject player = new GameObject($"Player_{clientId}");
        HealthManager healthManager = player.AddComponent<HealthManager>();
        playerHealthDict[clientId] = healthManager;

        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Local Player Registered");
        }
        else
        {
            Debug.Log("Remote Player Registered");
        }
    }

    // Called when a player's health changes
    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerHealthServerRpc(ulong clientId, int health)
    {
        Debug.Log(clientId);
        Debug.Log($"{health}");
        if (health <= 0)
        {
            ulong losingPlayerClientId = clientId;
            DeclareWinnerServerRpc(losingPlayerClientId);
        }
        
    }

    // ServerRpc to declare a winner
    [ServerRpc]
    private void DeclareWinnerServerRpc(ulong winningPlayerClientId)
    {
        Debug.Log($"Player {winningPlayerClientId} wins!");

        // Add logic here for what happens when a player wins, e.g., end the match, display a victory screen, etc.
    }
}
