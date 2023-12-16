//Author: Pearson Lawrence
//Obsolete, used for tutorial purposes of understanding unity netcode. Tutorial provided by samyam on youtube
using UnityEngine;
using Unity.Netcode;

public class FoodComponent : NetworkBehaviour
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
        else if(other.TryGetComponent(out Tail tail))
        {
            tail.networkedOwner.GetComponent<PlayerLength_Test>().AddLength();
        }
        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
        if (NetworkObject.IsSpawned) NetworkObject.Despawn();
    }
}
