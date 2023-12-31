//Author: Brandon
//Purpose: This is a parent class that sll other spells will inherit from. This class has the functions to set
//and get who spawmed a spell along with getting what type of spell was spawned.

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpellComponent : NetworkBehaviour
{
    public SpellType spellType;

    [SerializeField]
    private NetworkVariable<NetworkObjectReference> networkedOwner = new NetworkVariable<NetworkObjectReference>(default,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public GameObject getOwner()
    {
        if (networkedOwner.Value.TryGet(out NetworkObject ownerNetworkObject))
        {
            return ownerNetworkObject.gameObject;
        }
        return null;
    }

    public void setOwner(GameObject temp)
    {
        if (this.NetworkObject != null && this.NetworkObject.IsSpawned)
        {
            if (temp.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
            {
                networkedOwner.Value = networkObject;
                //Debug.Log("Owner set successfully.");
            }
            else
            {
                Debug.LogError("Failed to get NetworkObject component from the provided GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Attempting to set owner before the object is spawned on the network. IsSpawned: " + (this.NetworkObject != null ? this.NetworkObject.IsSpawned.ToString() : "null"));
        }
    }


    public SpellType GetSpellType()
    {
        return spellType;
    }
}
