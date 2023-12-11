using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private Transform SpawnedObjectPrefab;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 56,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    // Update is called once per frame
    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousVal, MyCustomData newVal) =>
        {
            Debug.Log(OwnerClientId + "; " + newVal._int + "; " + newVal._bool + "; " + newVal.message);
        };
        base.OnNetworkSpawn(); 
    }
    
    void Update()
    {
        if (!IsOwner) return;
        
        if(Input.GetKeyDown(KeyCode.T))
        {
            Transform spwanedObjectTransform = Instantiate(SpawnedObjectPrefab);
            spwanedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            
            
            //TestClientRpc(new ClientRpcParams {  Send = new ClientRpcSendParams {TargetClientIds = new List<ulong> { 1 } } });
            //TestServerRpc(new Se
            //rverRpcParams());
            /*randomNumber.Value = new MyCustomData
            {
                _int = Random.Range(0, 100),
                _bool = false,
                message = "Muahahahaha!",
            };*/

        }

        if(Input.GetKeyDown(KeyCode.Y))
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
    }

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRpc " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("testClientRpc");
    }
}
