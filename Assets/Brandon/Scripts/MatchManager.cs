//Author: Brandon Yu
//Purpose: This script will handle everyhting that takes place in the match. It will first register players when they connect via relay. Then it will enable the ready button, followed by enableing the elemental selectors.
//after players have choosen thier element the script will call all the pillars to move back and the jumbotron to come down. Then player healths will be updated within the updateHealthFuntion inside this sciprt. This script will
//track who wins and loses a round along with enableing a rematch system after a player reaches two wins. Finally this script will control the in-game announcer.

using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [HideInInspector] public NetworkVariable<bool> playerOneReplay = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<bool> playerTwoReplay = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [HideInInspector] public NetworkVariable<int> playerOneHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<int> playerTwoHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [HideInInspector] public NetworkVariable<int> joinedPlayerCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [HideInInspector] public NetworkVariable<int> roundCount = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<bool> isGameStarting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public Vector3 earthProjectileDirection;

    public List<TMP_Text> roundNumbers;

    public GameObject playerOneTable;
    public GameObject playerTwoTable;

    private SpellManager playerOneSpellManager;
    private SpellManager playerTwoSpellManager;

    public NetworkPlayer playerOneNetwork;
    public NetworkPlayer playerTwoNetwork;

    public PillarLogic hostPillar, guestPillar;

    public bool playerOneReady, playerTwoReady;
    public bool playerOneRematch, playerTwoRematch;

    private bool playerOneOrb, playerTwoOrb;
    public bool matchGoing = false;

    public AudioSource introAudio, StartAudio, CountAudio;

    [SerializeField] private List<PillarLogic> pillarLogicList;
    [SerializeField] private List<PillarLogic> matchEndPillarLogicList;

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
    public UnNetworkPlayer XRUnNetwork;

    public healthVignetteController vin;
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
        if(resetRound.Value || isGameStarting.Value)
        {
            XRUnNetwork.gestureEP.isElementSpawned = false;
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

        if (clientId == 0)
        {
            playerOneSpellManager = playerOneNetwork.getSpellManager();
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                Debug.Log("Local Player Registered");
                XRUnNetwork.spellmanager = playerOneSpellManager;
                XRUnNetwork.unSpellManager.elementSpeicalization = playerOneSpellManager.elementSpeicalization.Value;
                XRUnNetwork.gestureEP.spellmanager = playerOneSpellManager;
                vin.player1 = true;
            }
            joinedPlayerCount.Value++;
        }
        else if (clientId == 1)
        {
            playerTwoSpellManager = playerTwoNetwork.getSpellManager();
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                XRUnNetwork.spellmanager = playerTwoSpellManager;
                XRUnNetwork.unSpellManager.elementSpeicalization = playerTwoSpellManager.elementSpeicalization.Value;
                XRUnNetwork.gestureEP.spellmanager = playerTwoSpellManager;
                vin.player1 = false;
            }
            joinedPlayerCount.Value++;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DisableIsGameStartingServerRPC()
    {
        isGameStarting.Value = false;

        Debug.Log("Rpc false called");
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartMatchServerRpc(ulong clientId)
    {
        Debug.Log(playerOneSpellManager.GetSetSpecialization() + " : " + playerTwoSpellManager.GetSetSpecialization());
        if (clientId == 0)
        {
            playerOneOrb = true;
            //playerOneSpellManager.DisableChooseOrbsClientRpc();
            TableOffPlayerOneClientRpc();
        }
        if (clientId == 1)
        {
            playerTwoOrb = true;
            //playerTwoSpellManager.DisableChooseOrbsClientRpc();
            TableOffPlayerTwoClientRpc();
        }
        if (playerOneOrb && playerTwoOrb)
        {
            isGameStarting.Value = true;
            Debug.Log("Start Set It True");
            Debug.Log("both set and ready"); // DisableChooseOrbs();
            foreach (PillarLogic t in pillarLogicList)
            {
                t.MovePillarClientRpc(pillarDirection.TOEND);
            }
            tronMove.MoveJumboTronClientRpc();
            matchGoing = true;
            //playerOneNetwork.MovePlayerToStartClientRpc();
            //playerTwoNetwork.MovePlayerToStartClientRpc();
            PlayStartAudioClientRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeclareReadyServerRpc(ulong clientId)
    {

        if (clientId == 0)
        {
            playerOneReady = true;
            Debug.Log("P1");
        }
        if (clientId == 1)
        {
            playerTwoReady = true;
            Debug.Log("P2");
        }

        if (playerOneReady && playerTwoReady)
        {
            playerTwoReady = false;
            playerOneReady = false;

            pOneWinTally = 0;
            pTwoWinTally = 0;
            roundCount.Value = 1;

            playerOneHealth.Value = 100;
            playerTwoHealth.Value = 100;
            //playerOneSpellManager.ActivateChooseOrbsClientRpc();
            //playerTwoSpellManager.ActivateChooseOrbsClientRpc();
            TableOnPlayerOneClientRpc();
            TableOnPlayerTwoClientRpc();

            ReadyButtonOffClientRpc();

            foreach (ReadyButton t in readyButtonList)
            {
                t.matchStarted = true;
            }

            foreach (TMP_Text t in roundNumbers)
            {
                t.text = "round 1";// + roundCount.Value.ToString();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeclareRematchServerRpc(ulong clientId)
    {
        if (clientId == 0)
        {
            playerOneRematch = true;
            Debug.Log("P1");
        }
        if (clientId == 1)
        {
            playerTwoRematch = true;
            Debug.Log("P2");
        }

        if (playerOneRematch && playerTwoRematch)
        {
            isGameStarting.Value = true;
            Debug.Log("Rematch Set It True");
            playerOneRematch = false;
            playerTwoRematch = false;

            pOneWinTally = 0;
            pTwoWinTally = 0;

            playerOneOrb = false;
            playerTwoOrb = false;

            playerOneHealth.Value = 100;
            playerTwoHealth.Value = 100;
            ResetHealthServerRpc();
            ResetRoundCountClientRpc();

            WaitTilTurnOnClientRpc();

            foreach (PillarLogic t in pillarLogicList)
            {
                t.MovePillarClientRpc(pillarDirection.TOSTART);
            }

            foreach (TMP_Text t in roundNumbers)
            {

                t.text = "round 1";
            }
        }
    }

    [ClientRpc]
    private void PlayIntroductionArenaClientRPC()
    {
        introAudio.Play();
    }
    [ClientRpc]
    private void PlayCountDownClientRPC()
    {
        CountAudio.Play();
       
    }
    [ClientRpc]
    private void PlayStartAudioClientRPC()
    {
        StartAudio.Play();
    }
    [ClientRpc]
    private void ReadyButtonOffClientRpc()
    {
        foreach (ReadyButton t in readyButtonList)
        {
            t.gameObject.SetActive(false);
        }
    }
    [ClientRpc]
    private void ReadyButtonOnClientRpc()
    {
        foreach (ReadyButton t in readyButtonList)
        {
            t.gameObject.SetActive(true);
        }
    }
    [ClientRpc]
    private void TableOffPlayerOneClientRpc()
    {
        playerOneTable.gameObject.SetActive(false);
    }
    [ClientRpc]
    private void TableOnPlayerOneClientRpc()
    {
        playerOneTable.gameObject.SetActive(true);
    }
    [ClientRpc]
    private void TableOffPlayerTwoClientRpc()
    {
        playerTwoTable.gameObject.SetActive(false);
    }
    [ClientRpc]
    private void TableOnPlayerTwoClientRpc()
    {
        playerTwoTable.gameObject.SetActive(true);
    }

    //updates the player health variable across network using network variable
    [ServerRpc]
    public void UpdatePlayerHealthServerRpc(ulong clientId, int damage)
    {
        //Debug.Log($"UpdatePlayerHealthServerRpc - ClientID: {clientId}, Health: ");
        Debug.Log("Calling update health");
        if (clientId == 0)
        {
            playerOneHealth.Value -= damage;

        }
        else if (clientId == 1)
        {
            playerTwoHealth.Value -= damage;

        }

        Debug.Log("Player1 Health:" + playerOneHealth.Value + " Player2 Health " + playerTwoHealth.Value);

        if (playerOneHealth.Value <= 0)
        {
            pTwoWinTally += 1;
            if (pTwoWinTally == 2)
            {
                GameOver(2);
            }
            else
            {
                resetRound.Value = true;
                Debug.Log("Round Reseting");
                Debug.Log(playerOneHealth.Value);
                Debug.Log(playerTwoHealth.Value);
                StartCoroutine(delayReset());
                PlayCountDownClientRPC();
                roundCount.Value++;

                CountDownClientRpc();

                foreach (TMP_Text t in roundNumbers)
                {
                    t.text = "round " + roundCount.Value.ToString();
                }
            }

        }

        if (playerTwoHealth.Value <= 0)
        {
            pOneWinTally += 1;
            if (pOneWinTally == 2)
            {
                GameOver(1);
            }
            else
            {
                resetRound.Value = true;
                Debug.Log("Round Reseting");
                Debug.Log(playerOneHealth.Value);
                Debug.Log(playerTwoHealth.Value);
                StartCoroutine(delayReset());

                CountDownClientRpc();

                roundCount.Value++;

                foreach (TMP_Text t in roundNumbers)
                {
                    t.text = "round " + roundCount.Value.ToString();
                }
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

    public void GameOver(ulong clientID)
    {
        ReadyButtonOnClientRpc();

        isRoundReset.Value = true;

        DisplayWinnerClientRpc(clientID);

    }

    public IEnumerator delayReset()
    {
        yield return new WaitForSeconds(6);

        resetRound.Value = false;
        playerOneHealth.Value = 100;
        playerTwoHealth.Value = 100;
    }

    public void ResetRound()
    {
        playerOneNetwork.PlacePlayers();
        playerTwoNetwork.PlacePlayers();
        
        playerOneHealth.Value = 100;
        playerTwoHealth.Value = 100;

        foreach (TMP_Text t in roundNumbers)
        {

            t.text = "round 1";
        }
        foreach (HealthBar t in healthBars)
        {
            //t.UpdateHealthBarServerRpc();
        }
    }

    [ServerRpc]
    public void ResetHealthServerRpc()
    {
        playerOneHealth.Value = 100;
        playerTwoHealth.Value = 100;
    }

    [ClientRpc]
    public void ResetRoundCountClientRpc()
    {
        roundCount.Value = 1;
    }

    [ClientRpc]
    public void WaitTilTurnOnClientRpc()
    {
        StartCoroutine(ButtonOffForBit());
    }

    IEnumerator ButtonOffForBit()
    {
        foreach (ReadyButton t in readyButtonList)
        {
            t.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(5f);

        foreach (ReadyButton t in readyButtonList)
        {
            t.matchStarted = false;
        }

        foreach (ReadyButton t in readyButtonList)
        {
            t.gameObject.SetActive(true);
        }
    }


    [ClientRpc]
    public void DisplayWinnerClientRpc(ulong clientID) {

        if (clientID == 1)
        {
            foreach (TMP_Text t in roundNumbers)
            {

                t.text = "player1 wins";
            }
        }

        if (clientID == 2)
        {
            foreach (TMP_Text t in roundNumbers)
            {
                t.text = "player2 wins";
            }
        }
    }


    [ClientRpc]
    public void CountDownClientRpc()
    {
        StartCoroutine(CountDown());
    }


    IEnumerator CountDown()
    {
        if(!CountAudio.isPlaying)
        {
            CountAudio.Play();
        }
        for (int i = 6; i > 0; i--)
        {
            foreach (TMP_Text t in roundNumbers)
            {
                t.text = i.ToString();
            }
            yield return new WaitForSeconds(1f); 
        }

        foreach (TMP_Text t in roundNumbers)
        {
            t.text = "go";
        }
        yield return new WaitForSeconds(1f);

        foreach (TMP_Text t in roundNumbers)
        {
            t.text = "round " + roundCount.Value.ToString();
        }
    }
}
