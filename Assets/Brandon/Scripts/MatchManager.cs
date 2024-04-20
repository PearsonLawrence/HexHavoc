//Author: Brandon Yu
//Purpose: This script will keep track of teh clients/players that are in the game along with their respective 
//health manager. Keeps track of winner and loser of rounds along with storeing intial spawn locations

using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MatchManager : NetworkBehaviour
{
    public Dictionary<ulong, HealthManager> playerHealthDict = new Dictionary<ulong, HealthManager>();
    private int pOneWinTally;
    private int pTwoWinTally;
    public bool testJumbo = false;

    [SerializeField] private float resetTime = 10f, maxResetTime = 10f;
    public ulong loserId;

    [HideInInspector] public NetworkVariable<bool> isThereWinner = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<bool> isRoundReset = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [HideInInspector] public NetworkVariable<bool> resetRound = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [HideInInspector] public NetworkVariable<int> playerOneHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<int> playerTwoHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<int> joinedPlayerCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> isGameStarting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private SpellManager playerOneSpellManager;
    private SpellManager playerTwoSpellManager;

    public NetworkPlayer playerOneNetwork;
    public NetworkPlayer playerTwoNetwork;

    public PillarLogic hostPillar, guestPillar;

    private bool playerOneReady, playerTwoReady;
    private bool playerOneOrb, playerTwoOrb;
    public bool matchGoing = false;

    [SerializeField] private List<PillarLogic> pillarLogicList;
    [SerializeField] private List<ReadyButton> readyButtonList;
    [SerializeField] private List<HealthBar> healthBars;

    [SerializeField] private TronMove tronMove;

    // Singleton instance
    public static MatchManager Instance;
    private List<GameObject> players = new List<GameObject>();

    public Transform playerBody;
    public Transform spawnPosition2;

    public Transform gameSpawnPosition1;
    public Transform gameSpawnPosition2;

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
        if (testJumbo)
        {
            testJumbo = false;
            tronMove.MoveJumboTronClientRpc();
        }
    }

    // Called to register a player
    public void RegisterPlayer(ulong clientId, SpellManager spellManager, NetworkPlayer playerNetwork)
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

        if(clientId == 0)
        {
            playerOneSpellManager = spellManager;
            playerOneNetwork = playerNetwork;
            joinedPlayerCount.Value++; 
        }
        else if (clientId == 1)
        {
            playerTwoSpellManager = spellManager;
            playerTwoNetwork = playerNetwork;
            joinedPlayerCount.Value++; 
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DisableIsGameStartingServerRPC()
    {
        if(isGameStarting.Value)
        {
            isGameStarting.Value = false;
        } 
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartMatchServerRpc(ulong clientId)
    {
        Debug.Log(playerOneSpellManager.GetSetSpecialization() + " : " + playerTwoSpellManager.GetSetSpecialization());
        if (clientId == 0)
        {
            playerOneOrb = true;
            playerOneSpellManager.DisableChooseOrbsClientRpc();
        }
        if(clientId == 1)
        {
            playerTwoOrb = true;
            playerTwoSpellManager.DisableChooseOrbsClientRpc();
        }
        if(playerOneOrb && playerTwoOrb)
        {
            isGameStarting.Value = true;
            Debug.Log("both set and ready"); // DisableChooseOrbs();
            foreach (PillarLogic t in pillarLogicList)
            {
                
                t.MovePillarClientRpc(pillarDirection.TOEND);
            }
            tronMove.MoveJumboTronClientRpc();
            matchGoing = true;
            playerOneNetwork.MovePlayerToStartClientRpc();
            playerTwoNetwork.MovePlayerToStartClientRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeclareReadyServerRpc(ulong clientId)
    {
        if(clientId == 0)
        {
            playerOneReady = true;
            Debug.Log("P1");
        }
        if(clientId == 1)
        {
            playerTwoReady = true;
            Debug.Log("P2");
        }

        if(playerOneReady && playerTwoReady)
        {
            playerOneSpellManager.ActivateChooseOrbsClientRpc();
            playerTwoSpellManager.ActivateChooseOrbsClientRpc();

            ReadyButtonOffClientRpc();
        }

    }

    [ClientRpc]
    private void ReadyButtonOffClientRpc()
    {
        foreach (ReadyButton t in readyButtonList)
        {
            t.gameObject.SetActive(false);
        }
    }
    
    //updates the player health variable across network using network variable
    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerHealthServerRpc(ulong clientId, int damage)
    {
        //Debug.Log($"UpdatePlayerHealthServerRpc - ClientID: {clientId}, Health: ");
        Debug.Log("Calling update health");
        if (clientId == 0)
        {
            playerOneHealth.Value -= damage;
        }
        else if(clientId == 1)
        {
            playerTwoHealth.Value -= damage;
        }

        Debug.Log("Player1 Health:" + playerOneHealth.Value + " Player2 Health " + playerTwoHealth.Value);

        if (playerOneHealth.Value <= 0)
        {
            pTwoWinTally += 1;
            if(pTwoWinTally == 2)
            {
                DeclareWinner(clientId);
            }
            else
            {
                resetRound.Value = true;
                Debug.Log("Round Reseting");
                playerOneHealth.Value = 100;
                playerTwoHealth.Value = 100;
                Debug.Log(playerOneHealth.Value);
                Debug.Log(playerTwoHealth.Value);
            }

        }

        if (playerTwoHealth.Value <= 0)
        {
            pOneWinTally += 1;
            if (pOneWinTally == 2)
            {
                DeclareWinner(clientId);
            }
            else
            {
                resetRound.Value = true;
                Debug.Log("Round Reseting");
                playerOneHealth.Value = 100;
                playerTwoHealth.Value = 100;
                Debug.Log(playerOneHealth.Value);
                Debug.Log(playerTwoHealth.Value);
            }
        }

        //isRoundReset.Value = true;
        resetTime = maxResetTime;

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

    public void DeclareWinner(ulong loserClientId)
    {

    }

    public void ResetRound()
    {
        playerOneNetwork.PlacePlayers();
        playerTwoNetwork.PlacePlayers();

        playerOneHealth.Value = 100;
        playerTwoHealth.Value = 100;

        foreach (HealthBar t in healthBars)
        {
            //t.UpdateHealthBarServerRpc();
        }
    }
}
