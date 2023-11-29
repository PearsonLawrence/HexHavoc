using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class food : NetworkBehaviour
{
    public GameObject prefab;
    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!NetworkManager.Singleton.IsServer) return;

        if (other.TryGetComponent(out PlayerLength_Test playerlength))
        {
            playerlength.AddLength();
        }
        else if (other.TryGetComponent(out Tail tail))
        {
            tail.networkedOwner.GetComponent<PlayerLength_Test>().AddLength();
        }

        NetworkObject.Despawn();
    }
}
