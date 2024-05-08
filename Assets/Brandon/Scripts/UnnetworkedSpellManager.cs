//Author: Brandon
//Purpose: This script will spawn unnetwokred local projectiles and walls based on which hand called the spell

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class UnNetworkedSpellManager : MonoBehaviour
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
    public Transform LeftHandPos, RightHandPos;
    public Transform desiredProjectile;
    private Transform desiredWall;
    public GameObject ShieldSpawnPoint;
    //private bool setSpecialization = false;

    public List<Transform> chooseOrbs;

    public elementType elementSpeicalization;
    public elementType lastSpecialization;
    public bool setSpecialization = false;
    public bool setIsOrbDisabled = false;
    public float spawnWallDistance = 2;
    public float spawnProjectileDistance = 2;

    private int earthShotCount = 0;

    AudioManager audioManager;
    public Vector3 spellDirection;
    public GestureEventProcessor gestureEp;
    private MatchManager matchInstance;
    public Material earthMat, airMat, fireMat, waterMat;
    public SkinnedMeshRenderer handLeft, handRight;
    public void SetElementType(elementType elementType)
    {
        Debug.Log("In set");
        elementSpeicalization = elementType;
        setSpecialization = true;
        //DisableChooseOrbsServerRpc();
        MatchManager.Instance.StartMatchServerRpc(GetComponent<NetworkObject>().OwnerClientId);
    }

    public bool GetSetSpecialization()
    {
        return setSpecialization;
    }

    private void Start()
    {
        matchInstance = MatchManager.Instance;
        //ActivateChooseOrbs();
        audioManager = AudioManager.Instance;
    }

    void Update()
    {
        if(lastSpecialization != elementSpeicalization)
        {
            lastSpecialization = elementSpeicalization;
            switch (elementSpeicalization)
            {
                case elementType.FIRE:
                    handLeft.material = fireMat;
                    handRight.material = fireMat;
                    break;
                case elementType.WATER:
                    handLeft.material = waterMat;
                    handRight.material = waterMat;
                    break;
                case elementType.WIND:
                    handLeft.material = airMat;
                    handRight.material = airMat;
                    break;
                case elementType.EARTH:
                    handLeft.material = earthMat;
                    handRight.material = earthMat;
                    break;

            }

        }
    }


    public void SpawnProjectile()
    {
        // Define the distance in front of the player where the fireball will spawn
        float spawnDistance = 2f;

        // Calculate the spawn position based on the player's position and forward direction
        //Vector3 spawnPosition = LeftHandPos.position + LeftHandPos.forward * spawnDistance;
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

        desiredProjectile = windBlastPrefab;

        if (desiredProjectile == earthSpearPrefab)
        {
            if (earthShotCount == 4)
            {
                earthShotCount = 0;
            }
            else
            {
                earthShotCount++;
            }
        }

        // Instantiate the fireball at the calculated spawn position


        ProjectileComponent projectile = Instantiate(desiredProjectile, spawnPosition, Quaternion.identity).GetComponent<ProjectileComponent>();

        switch (elementSpeicalization)
        {
            case elementType.FIRE:
                SpellSFX(0);
                break;
            case elementType.WATER:
                SpellSFX(0);
                break;
            case elementType.WIND:
                SpellSFX(0);
                break;
            case elementType.EARTH:
                SpellSFX(0);
                break;

        }

        projectile.SetDirection(Vector3.forward);
        if (projectile)
        {
            
            projectile.setOwner(this.gameObject); // Now safe to set the owner

            
            //Debug.Log(this.gameObject);

            // Additional initialization as needed
            Vector3 playerForward = Camera.main.transform.forward;
            if (desiredProjectile != earthSpearPrefab)
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
        ProjectileComponent Projectile = projectile.GetComponent<ProjectileComponent>();
        if (Projectile != null)
        {
            //Projectile.parent = this;
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

        switch (elementSpeicalization)
        {
            case elementType.WATER:
                desiredProjectile = waterShotPrefab;
                break;
            case elementType.EARTH:
                desiredProjectile = earthSpearPrefab;
                if (earthShotCount == 4)
                {
                    earthShotCount = 0;
                }
                else
                {
                    earthShotCount++;
                }
                break;
        }


        ProjectileComponent projectile = Instantiate(desiredProjectile, spawnPosition, gestureEp.CurrentElement.transform.rotation).GetComponent<ProjectileComponent>();

        projectile.SetDirection(spellDirection);
       
            //projectile.SetDirection(Vector3.forward);
        
        // Add the fireball to the list of casted spells
        castedSpells.Add(projectile.transform);

    }
    public void SpawnLeftProjectile()
    {
        // Define the distance in front of the player where the fireball will spawn
        float spawnDistance = 2f;

        // Calculate the spawn position based on the player's position and forward direction
        Vector3 spawnPosition = LeftHandPos.position + LeftHandPos.forward * spawnProjectileDistance;
        //Vector3 spawnPosition = Vector3.zero;

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

        if (desiredProjectile == earthSpearPrefab)
        {
            if (earthShotCount == 4)
            {
                earthShotCount = 0;
            }
            else
            {
                earthShotCount++;
            }
        }

        // Instantiate the fireball at the calculated spawn position


        ProjectileComponent projectile = Instantiate(desiredProjectile, spawnPosition, LeftHandPos.rotation).GetComponent<ProjectileComponent>();

        projectile.SetDirection(LeftHandPos.forward);

       
        /*if (networkObject != null)
        {
            networkObject.Spawn(); // Spawn the object on the network

            projectile.setOwner(this.gameObject); // Now safe to set the owner

            projectile.earthShot.Value = earthShotCount.Value;

            projectile.handToFollow = LeftHandPos;

            //Debug.Log(this.gameObject);

            // Additional initialization as needed
            Vector3 playerForward = Camera.main.transform.forward;
            projectile.SetDirection(RightHandPos.forward);
            //projectile.SetDirection(Vector3.forward);
        }
        else
        {
            Debug.LogError("NetworkObject not found on the fireball prefab.");
        }*/

        // Add the fireball to the list of casted spells
        castedSpells.Add(projectile.transform);

        // Set the parent in the fireball component
        ProjectileComponent Projectile = projectile.GetComponent<ProjectileComponent>();
        if (Projectile != null)
        {
            //Projectile.parent = this;
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
        ProjectileComponent projectile = Instantiate(desiredProjectile, spawnPosition, RightHandPos.rotation).GetComponent<ProjectileComponent>();

        projectile.SetDirection(RightHandPos.forward);
        
        /*if (networkObject != null)
        {
            networkObject.Spawn(); // Spawn the object on the network

            projectile.setOwner(this.gameObject); // Now safe to set the owner

            projectile.earthShot = earthShotCount;

            projectile.handToFollow = RightHandPos;

            //Debug.Log(this.gameObject);

            // Additional initialization as needed
            Vector3 playerForward = Camera.main.transform.forward;
            projectile.SetDirection(RightHandPos.forward);
            // projectile.SetDirection(Vector3.forward);
        }
        else
        {
            Debug.LogError("NetworkObject not found on the fireball prefab.");
        }*/

        // Add the fireball to the list of casted spells
        castedSpells.Add(projectile.transform);

        // Set the parent in the fireball component
        ProjectileComponent Projectile = projectile.GetComponent<ProjectileComponent>();
        if (Projectile != null)
        {
            //Projectile.parent = this;
        }
        else
        {
            Debug.LogError("fireball component not found on the fireball prefab.");
        }
    }
    public void spawnWall()
    {
        Debug.Log("SpawnAttempt");
        float spawnDistance = 2f;
        Vector3 spawnPosition = LeftHandPos.position + LeftHandPos.forward * spawnDistance;
        //Vector3 spawnPosition = Vector3.zero;

       
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

        Vector3 tempRot = new Vector3(0, LeftHandPos.rotation.eulerAngles.y, 0);
        Quaternion tempRotation = Quaternion.Euler(tempRot);

        WallComponent wallSpell = Instantiate(desiredWall, spawnPosition, Camera.main.transform.rotation).GetComponent<WallComponent>();
        WallComponent wallComponent = wallSpell.transform.GetComponent<WallComponent>();

        // Check if the components are not null before proceeding
        if (wallComponent != null)
        {
            castedSpells.Add(wallSpell.transform);

            // Set the parent in WallSpell
            //wallComponent.parent = this;

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
        Vector3 spawnPosition = Camera.main.gameObject.transform.position + Camera.main.gameObject.transform.forward * spawnWallDistance;
        //Vector3 spawnPosition = Vector3.zero;
        //spawnPosition.y = Camera.main.gameObject.transform.position.y;

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

        WallComponent wallSpell = Instantiate(desiredWall, spawnPosition, Camera.main.transform.rotation).GetComponent<WallComponent>();
        //WallComponent wallSpell = Instantiate(desiredWall, spawnPosition, Quaternion.identity).GetComponent<WallComponent>();

        WallComponent wallComponent = wallSpell.transform.GetComponent<WallComponent>();

        // Check if the components are not null before proceeding
        if (wallComponent != null)
        {
            castedSpells.Add(wallSpell.transform);

            // Set the parent in WallSpell
            ////wallComponent.parent = this;

            // Set the owner after the wall has been spawned
            wallComponent.setOwner(this.gameObject);
        }
        else
        {
            Debug.LogError("NetworkObject or WallSpell component not found on the wall prefab.");
        }
    }


    public void spawnRightWall()
    {
        Debug.Log("SpawnAttempt");
        float spawnDistance = 4f;
        Vector3 spawnPosition = Camera.main.gameObject.transform.position + Camera.main.gameObject.transform.forward * spawnWallDistance;
        //Vector3 spawnPosition = Vector3.zero;
        //Vector3 spawnPosition = Vector3.zero;

        //spawnPosition.Set(0, 0, 7);

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

        Vector3 tempRot = new Vector3(0, Camera.main.gameObject.transform.rotation.eulerAngles.y, 0);

        WallComponent wallSpell = Instantiate(desiredWall, spawnPosition, Camera.main.transform.rotation).GetComponent<WallComponent>();

        WallComponent wallComponent = wallSpell.transform.GetComponent<WallComponent>();

        // Check if the components are not null before proceeding
        if (wallComponent != null)
        {
            castedSpells.Add(wallSpell.transform);

            // Set the parent in WallSpell
            //wallComponent.parent = this;

            // Set the owner after the wall has been spawned
            wallComponent.setOwner(this.gameObject);
        }
        else
        {
            Debug.LogError("NetworkObject or WallSpell component not found on the wall prefab.");
        }
    }

    //server rpc that handles spawning of networked spells
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
