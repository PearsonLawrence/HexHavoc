//Author: Pearson Lawrence, Brandon Yu
//Purpose: established network logic for player characters. Managed the players game states depending on which networked player has won the round. 
//Utilized Code Monkeys tutorial from youtube to build basic structure and understanding before expanding
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;
//TODO: REFACTOR
public class PlayerNetwork : NetworkBehaviour
{
    
    [SerializeField] private Transform SpawnedObjectPrefab;

    private MatchManager matchManager;
    [SerializeField] private SpellManager spellManager;
    [SerializeField] private PlayerNetwork playerNetwork;


    //This is an instantiation of a network variable. Used for understanding the network variable concept (Obsolete)
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 56,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    //This is an instantiation of a network variable. Used for understanding the network variable concept (Obsolete)
    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message; //Determines how much data is sent over the network
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

 

    // Update is called once per frame
    //When this is spawned on the network
    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousVal, MyCustomData newVal) =>
        {
            Debug.Log(OwnerClientId + "; " + newVal._int + "; " + newVal._bool + "; " + newVal.message);
        };
        base.OnNetworkSpawn();
    }
    private void Start()
    {
        if (IsLocalPlayer)
        {
            RegisterPlayerOnServerRpc(OwnerClientId);
        }
         PlacePlayers();
    }
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            Transform spwanedObjectTransform = Instantiate(SpawnedObjectPrefab);
            spwanedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(SpawnedObjectPrefab.gameObject);
        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            moveDir.z = +1f;

        }

        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;


        if (MatchManager.Instance.isRoundReset.Value)
        {
            //PlacePlayers();
        }
        else
        {
            //float moveSpeed = 3f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        if (MatchManager.Instance.isThereWinner.Value)
        {
            //DeclareWinner(MatchManager.Instance.loserId);
        }

    }
    [ServerRpc]
    private void RegisterPlayerOnServerRpc(ulong clientId)
    {
        MatchManager.Instance.RegisterPlayer(clientId, spellManager, playerNetwork);
    }

    public void PlacePlayers()
    {
        //if (IsOwner)  transform.position = MatchManager.Instance.playerBody.position;
        if (OwnerClientId == 0)
        {
           Vector3 newPos = new Vector3(-MatchManager.Instance.spawnPosition2.position.x, MatchManager.Instance.spawnPosition2.position.y, MatchManager.Instance.spawnPosition2.position.z);
            transform.position = MatchManager.Instance.playerBody.position;
        }
        if (OwnerClientId == 1)
        {
            transform.position = MatchManager.Instance.playerBody.position;
        }
    }

    public void DeclareWinner(ulong clientId)
    {

        Debug.Log($"Local Player ID: {OwnerClientId}, Winner ID: {clientId}");

        if (clientId == OwnerClientId)
        {
           // CanvasManager.Instance.ShowWinnerCanvas();
        }
        else
        {
           // CanvasManager.Instance.ShowLosserCanvas();
        }

        MatchManager.Instance.isThereWinner.Value = true;
    }


}
