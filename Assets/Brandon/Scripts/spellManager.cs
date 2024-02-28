//Author:Brandon
//Purpose: This scirpts job is to manage all spells. It has functions to create and spawn fireballs and walls
//along with a serverRpc that can be called to destroy spells.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public enum SpellType
{
    projectile,
    wall
}

public class SpellManager : NetworkBehaviour
{
    //All projectile prefabs
    [SerializeField] private Transform fireballPrefab;
    [SerializeField] private Transform windBlastPrefab;
    [SerializeField] private Transform waterShotPrefab;
    [SerializeField] private Transform earthSpearPrefab;

    //All wall prefabs
    [SerializeField] private Transform fireWallPrefab;
    [SerializeField] private Transform waterWallPrefab;
    [SerializeField] private Transform windWallPrefab;
    [SerializeField] private Transform earthWallPrefab;

    //Extra needed varibales
    [SerializeField] private List<Transform> castedSpells = new List<Transform>();
    public Transform LeftHandPos, RightHandPos;
    private Transform desiredProjectile;
    private Transform desiredWall;

    elementType elementSpeicalization = elementType.WATER;
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
                SpawnProjectile();
            }
            else
            {
                RequestSpawnProjectileServerRpc();
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
    //Function to be called when spawning a fireball on the network
    public void fireBall(Transform transPos)
    {
        //TODO: Clearify test and refactor 
        RequestSpawnProjectileServerRpc(); //Spawns fireball from rpc over network
        if (IsServer) 
        {
            SpawnProjectile();
        }
        else
        {
            RequestSpawnProjectileServerRpc();
        }
    }

    public void fireWall(Transform transPos)
    {
        //TODO: Clearify test and refactor 
        if (IsServer)
        {
            spawnWall();
        }
        else
        {
            RequestSpawnWallServerRpc();
        }
    }

    //server rpc that handles spawning of networked spells
    [ServerRpc]
    public void RequestSpawnProjectileServerRpc()
    {
        Debug.Log("RPCFIREBALL");
        SpawnProjectile();
    }

    //handles the spawning of fireball spell
    public void SpawnProjectile()
    {
        // Define the distance in front of the player where the fireball will spawn
        float spawnDistance = 2f;

        // Calculate the spawn position based on the player's position and forward direction
        //Vector3 spawnPosition = RightHandPos.position + RightHandPos.forward * spawnDistance;
        Vector3 spawnPosition = Vector3.zero;

        switch (elementSpeicalization)
        {
            case elementType.FIRE:
                desiredProjectile = fireballPrefab;
                break;
            case elementType.WATER:
                desiredProjectile = waterShotPrefab;
                break;
            case elementType.WIND:
                desiredProjectile = windBlastPrefab;
                break;
            case elementType.EARTH:
                desiredProjectile = earthSpearPrefab;
                break;

        }

        // Instantiate the fireball at the calculated spawn position
        NetworkedProjectileComponent projectile = Instantiate(desiredProjectile, spawnPosition, Quaternion.identity).GetComponent<NetworkedProjectileComponent>();
        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Spawn(); // Spawn the object on the network

            projectile.setOwner(this.gameObject); // Now safe to set the owner

            //Debug.Log(this.gameObject);

            // Additional initialization as needed
            Vector3 playerForward = Camera.main.transform.forward;
            //projectile.SetDirection(RightHandPos.forward);
            projectile.SetDirection(Vector3.forward);
        }
        else
        {
            Debug.LogError("NetworkObject not found on the fireball prefab.");
        }

        // Add the fireball to the list of casted spells
        castedSpells.Add(projectile.transform);

        // Set the parent in the fireball component
        NetworkedProjectileComponent Projectile = projectile.GetComponent<NetworkedProjectileComponent>();
        if (Projectile != null)
        {
            Projectile.parent = this;
        }
        else
        {
            Debug.LogError("fireball component not found on the fireball prefab.");
        }
    }


    public void spawnWall()
    {
        Debug.Log("SpawnAttempt");
        float spawnDistance = 4f;
        //Vector3 spawnPosition = LeftHandPos.position + LeftHandPos.forward * spawnDistance;
        Vector3 spawnPosition = Vector3.zero;

        spawnPosition.Set(0, 0, 7);

        switch (elementSpeicalization)
        {
            case elementType.FIRE:
                desiredWall = fireWallPrefab;
                break;
            case elementType.WATER:
                desiredWall = waterWallPrefab;
                break;
            case elementType.WIND:
                desiredWall = windWallPrefab;
                break;
            case elementType.EARTH:
                desiredWall = earthWallPrefab;
                break;

        }

        //NetworkedWallComponent wallSpell = Instantiate(desiredWall, spawnPosition, LeftHandPos.rotation).GetComponent<NetworkedWallComponent>();
        NetworkedWallComponent wallSpell = Instantiate(desiredWall, spawnPosition, Quaternion.identity).GetComponent<NetworkedWallComponent>();

        NetworkObject networkedObject = wallSpell.transform.GetComponent<NetworkObject>();
        NetworkedWallComponent wallComponent = wallSpell.transform.GetComponent<NetworkedWallComponent>();

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

    //server rpc that handles spawning of networked spells

    [ServerRpc]
    private void RequestSpawnWallServerRpc()
    {
        spawnWall();
    }

    [ServerRpc(RequireOwnership = false)]

    //remove a networked rpc from the network
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
