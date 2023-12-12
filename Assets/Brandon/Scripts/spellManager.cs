using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum SpellType
{
    fireball,
    wall
}

public class SpellManager : NetworkBehaviour
{
    [SerializeField] private Transform fireballPrefab;
    [SerializeField] private Transform wallPrefab;
    [SerializeField] private List<Transform> castedSpells = new List<Transform>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (IsServer)
            {
                SpawnFireball();
            }
            else
            {
                RequestSpawnFireballServerRpc();
            }
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            if (IsServer)
            {
                spawnWall();
            }
            else
            {
                RequestSpawnWallServerRpc();
            }
        }
    }

    [ServerRpc]
    private void RequestSpawnFireballServerRpc()
    {
        SpawnFireball();
    }

    private void SpawnFireball()
    {
        // Define the distance in front of the player where the fireball will spawn
        float spawnDistance = 2f;

        // Calculate the spawn position based on the player's position and forward direction
        Vector3 spawnPosition = transform.position + Camera.main.transform.forward * spawnDistance;

        // Instantiate the fireball at the calculated spawn position
        fireball Fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity).GetComponent<fireball>();
        NetworkObject networkObject = Fireball.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Spawn(); // Spawn the object on the network

            Fireball.setOwner(this.gameObject); // Now safe to set the owner

            // Additional initialization as needed
            Vector3 playerForward = Camera.main.transform.forward;
            Fireball.SetDirection(playerForward);
        }
        else
        {
            Debug.LogError("NetworkObject not found on the fireball prefab.");
        }

        // Add the fireball to the list of casted spells
        castedSpells.Add(Fireball.transform);

        // Set the parent in the fireball component
        fireball fireballComponent = Fireball.GetComponent<fireball>();
        if (fireballComponent != null)
        {
            fireballComponent.parent = this;
        }
        else
        {
            Debug.LogError("fireball component not found on the fireball prefab.");
        }
    }


    private void spawnWall()
    {
        float spawnDistance = 4f;
        Vector3 spawnPosition = transform.position + Camera.main.transform.forward * spawnDistance;

        WallSpell wallSpell = Instantiate(wallPrefab, spawnPosition, Quaternion.identity).GetComponent<WallSpell>();

        NetworkObject networkedObject = wallSpell.transform.GetComponent<NetworkObject>();
        WallSpell wallComponent = wallSpell.transform.GetComponent<WallSpell>();

        // Check if the components are not null before proceeding
        if (networkedObject != null && wallComponent != null)
        {
            castedSpells.Add(wallSpell.transform);

            // Spawn the networked object
            networkedObject.Spawn(true);

            // Set the parent in WallSpell
            wallComponent.parent = this;

            // Set the owner after the wall has been spawned
            wallComponent.setOwner(this.gameObject);
        }
        else
        {
            Debug.LogError("NetworkObject or WallSpell component not found on the wall prefab.");
        }
    }


    [ServerRpc]
    private void RequestSpawnWallServerRpc()
    {
        spawnWall();
    }

    [ServerRpc(RequireOwnership = false)]

    public void DestroyServerRpc()
    {
        if (castedSpells.Count > 0)
        {
            Transform toDestroy = castedSpells[0];
            castedSpells.Remove(toDestroy);
            toDestroy.GetComponent<NetworkObject>().Despawn();
            Destroy(toDestroy);
        }
        else
        {
            Debug.LogWarning("Trying to destroy a spell, but the list of casted spells is empty.");
        }
    }
}
