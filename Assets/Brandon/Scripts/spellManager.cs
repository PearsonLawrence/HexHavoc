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

[GenerateSerializationForType(typeof(SpellManager))]

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

    [SerializeField] private Transform chooseEarthPrefab;
    [SerializeField] private Transform chooseFirePrefab;
    [SerializeField] private Transform chooseWaterPrefab;
    [SerializeField] private Transform chooseWindPrefab;

    [SerializeField] private Transform selectPoint1;
    [SerializeField] private Transform selectPoint2;
    [SerializeField] private Transform selectPoint3;
    [SerializeField] private Transform selectPoint4;

    //Extra needed varibales
    [SerializeField] private List<Transform> castedSpells = new List<Transform>();
    public Transform LeftHandPos, RightHandPos, HeadPos;
    public Transform desiredProjectile;
    private Transform desiredWall;
    //private bool setSpecialization = false;

    public List<Transform> chooseOrbs;

    [HideInInspector] public NetworkVariable<bool> setSpecialization = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [HideInInspector] public NetworkVariable<bool> setIsOrbDisabled = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [HideInInspector] public NetworkVariable<Vector3> spellDirection = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<elementType> elementSpeicalization = new NetworkVariable<elementType>(elementType.WIND, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<int> earthShotCount = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);

    AudioManager audioManager;

    MatchManager matchManager;

    public float spawnProjectileDistance = 2, spawnWallDistance = 2;
    public GestureEventProcessor gestureEp;
    public void SetElementType(elementType elementType)
    {
        Debug.Log("In set");
        elementSpeicalization.Value = elementType;
        setSpecialization.Value = true;
        //DisableChooseOrbsServerRpc();
        MatchManager.Instance.StartMatchServerRpc(GetComponent<NetworkObject>().OwnerClientId);
    }

    public bool GetSetSpecialization()
    {
        return setSpecialization.Value;
    }

    private void Start()
    {
        //ActivateChooseOrbs();
        audioManager = AudioManager.Instance;

        matchManager = MatchManager.Instance;
        gestureEp = matchManager.XRUnNetwork.gestureEP;
    }

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
        if(setIsOrbDisabled.Value)
        {
            //DisableChooseOrbsClientRpc();
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

    //server rpc that handles spawning of networked spells
    [ServerRpc]
    public void RequestSpawnProjectileServerRpc()
    {
        Debug.Log("RPCFIREBALL");
        SpawnProjectile();
    }
    [ServerRpc]
    public void RequestSpawnLeftProjectileServerRpc()
    {
        Debug.Log("RPCFIREBALL");
        SpawnLeftProjectile();
    }
    [ServerRpc]
    public void RequestSpawnRightProjectileServerRpc()
    {
        Debug.Log("RPCFIREBALL");
        SpawnRightProjectile();
    }
    [ServerRpc]
    public void RequestSpawnHitProjectileServerRpc()
    {

        Debug.Log("RPCFIREHIT");
        SpawnHitProjectile();
    }

    public void SpawnProjectile()
    {
        // Define the distance in front of the player where the fireball will spawn
        float spawnDistance = 2f;

        // Calculate the spawn position based on the player's position and forward direction
        //Vector3 spawnPosition = LeftHandPos.position + LeftHandPos.forward * spawnDistance;
        Vector3 spawnPosition = Vector3.zero;

        switch (elementSpeicalization.Value)
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

        desiredProjectile = fireballPrefab;

        if (desiredProjectile == earthSpearPrefab)
        {
            if (earthShotCount.Value == 4)
            {
                earthShotCount.Value = 0;
            }
            else
            {
                earthShotCount.Value++;
            }
        }

        // Instantiate the fireball at the calculated spawn position


        NetworkedProjectileComponent projectile = Instantiate(desiredProjectile, spawnPosition, Quaternion.identity).GetComponent<NetworkedProjectileComponent>();

        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Spawn(); // Spawn the object on the network

            projectile.setOwner(this.gameObject); // Now safe to set the owner

            projectile.earthShot.Value = earthShotCount.Value;

            //Debug.Log(this.gameObject);

            // Additional initialization as needed
            Vector3 playerForward = Camera.main.transform.forward;
            if(desiredProjectile != earthSpearPrefab)
            {
                projectile.SetDirection(Vector3.forward);
            }
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

    public void SpawnHitProjectile()
    {
        // Define the distance in front of the player where the fireball will spawn
        float spawnDistance = 2f;

        // Calculate the spawn position based on the player's position and forward direction
        Vector3 spawnPosition = gestureEp.CurrentElement.transform.position;
        //Vector3 spawnPosition = Vector3.zero;

        switch (elementSpeicalization.Value)
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
        NetworkedProjectileComponent projectile = Instantiate(desiredProjectile, spawnPosition, gestureEp.CurrentElement.transform.rotation).GetComponent<NetworkedProjectileComponent>();

        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Spawn(); // Spawn the object on the network

            projectile.setOwner(this.gameObject); // Now safe to set the owner

            projectile.earthShot.Value = earthShotCount.Value;

           // projectile.handToFollow = spellDirection;

            //Debug.Log(this.gameObject);

            // Additional initialization as needed
            Vector3 playerForward = Camera.main.transform.forward;


            if (elementSpeicalization.Value == elementType.EARTH)
            {
                projectile.SetDirection(spellDirection.Value);
            }

            // projectile.SetDirection(Vector3.forward);
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

    public void SpawnLeftProjectile()
    {
        // Define the distance in front of the player where the fireball will spawn
        float spawnDistance = 2f;

        // Calculate the spawn position based on the player's position and forward direction
        Vector3 spawnPosition = LeftHandPos.position + LeftHandPos.forward * spawnProjectileDistance;

        //Vector3 spawnPosition = Vector3.zero;

        switch (elementSpeicalization.Value)
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
                spawnPosition = LeftHandPos.position;
                break;

        }

        if (desiredProjectile == earthSpearPrefab)
        {
            if(earthShotCount.Value == 4)
            {
                earthShotCount.Value = 0;
            }
            else
            {
                earthShotCount.Value++;
            }
        }

        // Instantiate the fireball at the calculated spawn position

        NetworkedProjectileComponent projectile = Instantiate(desiredProjectile, spawnPosition, LeftHandPos.rotation).GetComponent<NetworkedProjectileComponent>();


        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();

        projectile.SetDirection(LeftHandPos.forward);

        if (networkObject != null)
        {
            networkObject.Spawn(); // Spawn the object on the network

            projectile.setOwner(this.gameObject); // Now safe to set the owner

            projectile.earthShot.Value = earthShotCount.Value;

            projectile.handToFollow = LeftHandPos;

            //Debug.Log(this.gameObject);

            // Additional initialization as needed

            //projectile.SetDirection(Vector3.forward);
            if (elementSpeicalization.Value == elementType.EARTH)
            {
                projectile.SetDirection(spellDirection.Value);
            }
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

    //handles the spawning of fireball spell
    public void SpawnRightProjectile()
    {
        // Define the distance in front of the player where the fireball will spawn
        float spawnDistance = 2f;

        // Calculate the spawn position based on the player's position and forward direction
        Vector3 spawnPosition = RightHandPos.position + RightHandPos.forward * spawnProjectileDistance;
        //Vector3 spawnPosition = Vector3.zero;

        switch (elementSpeicalization.Value)
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
                spawnPosition = RightHandPos.position;
                break;

        }

        // Instantiate the fireball at the calculated spawn position
        NetworkedProjectileComponent projectile = Instantiate(desiredProjectile, spawnPosition, RightHandPos.rotation).GetComponent<NetworkedProjectileComponent>();

        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();
        projectile.SetDirection(RightHandPos.forward);
        if (networkObject != null)
        {
            networkObject.Spawn(); // Spawn the object on the network

            projectile.setOwner(this.gameObject); // Now safe to set the owner

            projectile.earthShot.Value = earthShotCount.Value;

            projectile.handToFollow = RightHandPos;

            //Debug.Log(this.gameObject);

            // Additional initialization as needed
            Vector3 playerForward = Camera.main.transform.forward;


            if (elementSpeicalization.Value == elementType.EARTH)
            {
                projectile.SetDirection(spellDirection.Value);
            }

            // projectile.SetDirection(Vector3.forward);
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
        Vector3 spawnPosition = LeftHandPos.position + LeftHandPos.forward * spawnDistance;
        //Vector3 spawnPosition = Vector3.zero;


        switch (elementSpeicalization.Value)
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

        Vector3 tempRot = new Vector3(0, LeftHandPos.rotation.eulerAngles.y, 0);
        Quaternion tempRotation = Quaternion.Euler(tempRot);

        NetworkedWallComponent wallSpell = Instantiate(desiredWall, spawnPosition, tempRotation).GetComponent<NetworkedWallComponent>();
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


    public void spawnLeftWall()
    {
        Debug.Log("SpawnAttempt");
        float spawnDistance = 4f;
        Vector3 spawnPosition = HeadPos.position + HeadPos.forward * spawnWallDistance;
        //Vector3 spawnPosition = Vector3.zero;


        switch (elementSpeicalization.Value)
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
        NetworkedWallComponent wallSpell = Instantiate(desiredWall, spawnPosition, HeadPos.rotation).GetComponent<NetworkedWallComponent>();
        //WallComponent wallSpell = Instantiate(desiredWall, spawnPosition, Quaternion.identity).GetComponent<WallComponent>();

        NetworkedWallComponent wallComponent = wallSpell.transform.GetComponent<NetworkedWallComponent>();

        NetworkObject networkedObject = wallSpell.transform.GetComponent<NetworkObject>();
        
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

    //Function to be called when spawning a fireball on the network
    public void fireRightProjectile()
    {
        //TODO: Clearify test and refactor 
        //RequestSpawnRightProjectileServerRpc(); //Spawns fireball from rpc over network
        if (IsServer)
        {
            SpawnRightProjectile();
        }
        else
        {
            RequestSpawnRightProjectileServerRpc();
        }
    }//Function to be called when spawning a fireball on the network
    public void fireLeftProjectile()
    {
        //TODO: Clearify test and refactor 
        //RequestSpawnLeftProjectileServerRpc(); //Spawns fireball from rpc over network
        if (IsServer)
        {
            SpawnLeftProjectile();
        }
        else
        {
            RequestSpawnLeftProjectileServerRpc();
        }
    }
    public void fireHitProjectile()
    {
        //TODO: Clearify test and refactor 
        Debug.Log("Fire hit projectile");
        //RequestSpawnHitProjectileServerRpc(); //Spawns fireball from rpc over network
        if (IsServer)
        {
            SpawnHitProjectile();
        }
        else
        {
            RequestSpawnHitProjectileServerRpc();
        }
    }

    public void fireLeftWall()
    {
        //TODO: Clearify test and refactor 
        if (IsServer)
        {
            spawnLeftWall();
        }
        else
        {
            RequestSpawnLeftWallServerRpc();
        }
    }
    public void fireRightWall()
    {
        //TODO: Clearify test and refactor 
        if (IsServer)
        {
            spawnRightWall();
        }
        else
        {
            RequestSpawnRightWallServerRpc();
        }
    }
    public void spawnRightWall()
    {
        Debug.Log("SpawnAttempt");
        float spawnDistance = 4f;
        Vector3 spawnPosition = HeadPos.position + HeadPos.forward * spawnWallDistance;
        //Vector3 spawnPosition = Vector3.zero;


        switch (elementSpeicalization.Value)
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
        NetworkedWallComponent wallSpell = Instantiate(desiredWall, spawnPosition, HeadPos.rotation).GetComponent<NetworkedWallComponent>();

        NetworkedWallComponent wallComponent = wallSpell.transform.GetComponent<NetworkedWallComponent>();

        NetworkObject networkedObject = wallSpell.transform.GetComponent<NetworkObject>();
        
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
    public void RequestSpawnLeftWallServerRpc()
    {
        spawnLeftWall();
    }

    [ServerRpc]
    public void RequestSpawnRightWallServerRpc()
    {
        spawnRightWall();
    }

    [ServerRpc]
    public void RequestSpawnWallServerRpc()
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

    [ClientRpc]
    public void ActivateChooseOrbsClientRpc()
    {
        Vector3 startSpawn = transform.position;
        Transform Earth = Instantiate(chooseEarthPrefab, selectPoint1.position, Quaternion.identity);
        chooseOrbs.Add(Earth);
        Transform Fire = Instantiate(chooseFirePrefab, selectPoint2.position, Quaternion.identity);
        chooseOrbs.Add(Fire);
        Transform Water = Instantiate(chooseWaterPrefab, selectPoint3.position, Quaternion.identity);
        chooseOrbs.Add(Water);
        Transform Wind = Instantiate(chooseWindPrefab, selectPoint4.position, Quaternion.identity);
        chooseOrbs.Add(Wind);
    }

    [ClientRpc]
    public void DisableChooseOrbsClientRpc()
    {
        
        foreach (Transform t in chooseOrbs)
        {
            t.gameObject.SetActive(false);
            
        }
        setIsOrbDisabled.Value = false;
    }
    

    public void SpellSFX(int soundNumber)
    {
        switch (soundNumber)
        {
            case 0:
                audioManager.PlayFireballSound();
                break;
            case 1:
                audioManager.PlayEarthSpearSound();
                break;
            case 2:
                //audioManager.PlayEarthSpearThrow();
                break;
        }
    }
}
