// MatchManager.cs
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MatchManager : NetworkBehaviour
{
    public Dictionary<ulong, HealthManager> playerHealthDict = new Dictionary<ulong, HealthManager>();
    private int pOneWinTally;
    private int pTwoWinTally;

    [SerializeField] private float resetTime = 10f, maxResetTime = 10f;
    public ulong loserId;

    public NetworkVariable<bool> isThereWinner = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<bool> isRoundReset = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    // Singleton instance
    public static MatchManager Instance;

    public Transform spawnPosition1;
    public Transform spawnPosition2;

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

    private void Update()
    {
        resetTime -= Time.deltaTime;

        if (playerHealthDict.Count >= 1 && resetTime <= 0f)
        {
            isRoundReset.Value = false;
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

    //updates the player health variable across network using network variable
    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerHealthServerRpc(ulong clientId, int health)
    {
        Debug.Log($"UpdatePlayerHealthServerRpc - ClientID: {clientId}, Health: {health}");

        if (health <= 0)
        {
            if (clientId == 0)
            {
                pTwoWinTally += 1;
                Debug.Log("p2 + 1 point");
            }

            if (clientId == 1)
            {
                pOneWinTally += 1;
                Debug.Log("p1 + 1 point");
            }
            isRoundReset.Value = true;
            resetTime = maxResetTime;
        }
        //update round with winner depending on who loses all HP first. 
        //TODO: make rounds properly work
        //TODO: Create end condition with max rounds to win
        //TODO: return to lobby after match
        //TODO: Update leaderboard???????
        if (pTwoWinTally >= 1)
        {
            isThereWinner.Value = true;
            loserId = clientId;
        }

        if (pOneWinTally >= 1)
        {
            isThereWinner.Value = true;
            loserId = clientId;
        }
    }

}
