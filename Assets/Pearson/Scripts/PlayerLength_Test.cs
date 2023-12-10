using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerLength_Test : NetworkBehaviour
{
    public NetworkVariable<ushort> length = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private List<GameObject> _tails;
    private Transform _lastTail;
    [SerializeField]
    private GameObject tailPrefab;

    private Collider thisCollider;
    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;

        for (int i = 0; i < length.Value - 1; ++i)

            InstantiateTail();

        base.OnNetworkSpawn();
        _tails = new List<GameObject>();
        _lastTail = transform;
        thisCollider = GetComponent<Collider>();
        if(!IsServer) length.OnValueChanged += LengthChanged;
    }

    [ContextMenu("Add Length")]
    //Only called by server
    public void AddLength()
    {
        length.Value += 1;
        InstantiateTail();
    }

    private void LengthChanged(ushort previousValue, ushort newValue)
    {
        Debug.Log("LengthChanged callback");
        InstantiateTail();
    }

    private void InstantiateTail()
    {
        GameObject tail = Instantiate(tailPrefab, transform.position, Quaternion.identity);
        
        if (tail.TryGetComponent(out Tail tailOut))
        {
            tailOut.networkedOwner = transform;
            tailOut.followTransform = _lastTail;
            _lastTail = tail.transform;
            Physics.IgnoreCollision(tail.GetComponent<Collider>(), thisCollider);
        }
        _tails.Add(tail);
    }
}
