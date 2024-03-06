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

    [SerializeField] private float resetTime = 10f, maxResetTime = 10f;
    public ulong loserId;

    [HideInInspector] public NetworkVariable<bool> isThereWinner = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<bool> isRoundReset = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [HideInInspector] public NetworkVariable<int> playerOneHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<int> playerTwoHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private SpellManager playerOneSpellManager;
    private SpellManager playerTwoSpellManager;

    private bool playerOneReady, playerTwoReady;

    [SerializeField] private List<PillarLogic> pillarLogicList;
    [SerializeField] private List<ReadyButton> readyButtonList;

    // Singleton instance
    public static MatchManager Instance;
    private List<GameObject> players = new List<GameObject>();

    public Transform playerBody;
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
    public void RegisterPlayer(ulong clientId, SpellManager spellManager)
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
        }
        else if (clientId == 1)
        {
            playerTwoSpellManager = spellManager;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartMatchServerRpc()
    {
        if(playerOneSpellManager.GetSetSpecialization() && playerTwoSpellManager.GetSetSpecialization())
        {
            Debug.Log("both set and ready");
            foreach(PillarLogic t in pillarLogicList)
            {
                t.MovePillarClientRpc(pillarDirection.TOEND);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeclareReadyServerRpc(ulong clientId)
    {
        if(clientId == 0)
        {
            playerOneReady = true;
        }
        if(clientId == 1)
        {
            playerTwoReady = true;
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
       
        if (clientId == 0)
        {
            playerOneHealth.Value -= damage;
            //Debug.Log(playerOneHealth.Value + " : " + playerTwoHealth.Value);
            if (clientId == 0)
            {
                pTwoWinTally += 1;
                //Debug.Log("p2 + 1 point");
            }

            if (clientId == 1)
            {
                pOneWinTally += 1;
                //Debug.Log("p1 + 1 point");
            }
            //isRoundReset.Value = true;
            resetTime = maxResetTime;
        }
        else if(clientId == 1)
        {
            playerTwoHealth.Value -= damage;
        }

        Debug.Log("Player Health: " + playerOneHealth.Value /*+ " : " + playerTwoHealth.Value*/);

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
