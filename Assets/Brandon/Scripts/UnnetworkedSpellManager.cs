using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class UnNetworkedSpellManager : MonoBehaviour
{
    public GameObject cheese;

    [SerializeField] private Transform fireballPrefab;
    [SerializeField] private Transform windBlastPrefab;
    [SerializeField] private Transform waterShotPrefab;
    [SerializeField] private Transform earthSpearPrefab;

    [SerializeField] private Transform fireWallPrefab;
    [SerializeField] private Transform waterWallPrefab;
    [SerializeField] private Transform windWallPrefab;
    [SerializeField] private Transform earthWallPrefab;

    // Collection of spawned spells for potential future reference or management
    private List<Transform> castedSpells = new List<Transform>();

    public elementType playerSpecialzation = elementType.WATER;
    private Transform desieredProjectile;
    private Transform desiredWall;
    [SerializeField] private Transform Lhand;
    [SerializeField] private Transform Rhand;
    public float spawnDistance = 2;
    public void setPlayerSpecialization(elementType var)
    {
        playerSpecialzation = var;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Y))
        {
            SpawnProjectile(false);
            Debug.Log("further back");
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            SpawnWall(false);
        }

        if( Input.GetKeyUp(KeyCode.Z))
        {
            SpawnTheProjectile();
        }

        if(Input.GetKeyUp (KeyCode.W))
        {
            cheese.SetActive(true);
        }
    }

    public void SpawnTheProjectile()
    {
        Vector3 spawnPosition = new Vector3(-5,15,-8);

        switch (playerSpecialzation)
        {
            case elementType.EARTH:
                desieredProjectile = earthSpearPrefab.transform;
                break;
            case elementType.FIRE:
                desieredProjectile = fireballPrefab.transform;
                break;
            case elementType.WIND:
                desieredProjectile = windBlastPrefab.transform;
                break;
            case elementType.WATER:
                desieredProjectile = waterShotPrefab.transform;
                break;
        }
        Debug.Log("in spawn");
        var projectile = Instantiate(desieredProjectile, spawnPosition, Quaternion.identity).GetComponent<ProjectileComponent>();
        if (projectile != null)
        {
            projectile.setOwner(this.gameObject);
            castedSpells.Add(projectile.transform);
            projectile.SetDirection(Vector3.forward);
        }
        else
        {
            Debug.LogError("ProjectileComponent not found on the instantiated prefab.");
        }
    }

    // Method for spawning a projectile
    public void SpawnProjectile(bool isLeft)
    {

        Vector3 spawnPosition = (isLeft) ? Lhand.position + Lhand.forward * spawnDistance : Rhand.position + Rhand.forward * spawnDistance;

        Quaternion spawnRotation = (isLeft) ? Lhand.rotation : Rhand.rotation;

        switch (playerSpecialzation)
        {
            case elementType.EARTH:
                desieredProjectile = earthSpearPrefab.transform;
                break;
            case elementType.FIRE:
                desieredProjectile = fireballPrefab.transform;
                break;
            case elementType.WIND:
                desieredProjectile = windBlastPrefab.transform;
                break;
            case elementType.WATER:
                desieredProjectile = waterShotPrefab.transform;
                break;
        }
        Debug.Log("in spawn");
        var projectile = Instantiate(desieredProjectile, spawnPosition, spawnRotation).GetComponent<ProjectileComponent>();
        if (projectile != null)
        {
            projectile.setOwner(this.gameObject);
            castedSpells.Add(projectile.transform); 
            projectile.SetDirection(Vector3.forward);
        }
        else
        {
            Debug.LogError("ProjectileComponent not found on the instantiated prefab.");
        }
    }

    public void SpawnWall(bool isLeft)
    {
        switch (playerSpecialzation)
        {
            case elementType.EARTH:
                desiredWall = earthWallPrefab.transform;
                break;
            case elementType.FIRE:
                desiredWall = fireWallPrefab.transform;
                break;
            case elementType.WIND:
                desiredWall = windWallPrefab.transform;
                break;
            case elementType.WATER:
                desiredWall = waterWallPrefab.transform;
                break;
        }

        Vector3 spawnPosition = (isLeft) ? Lhand.position + Lhand.forward * spawnDistance : Rhand.position + Rhand.forward * spawnDistance;
        Quaternion spawnRotation = (isLeft) ? Lhand.rotation : Rhand.rotation;
        WallComponent wall = Instantiate(desiredWall, spawnPosition, spawnRotation).GetComponent<WallComponent>();

        if(wall != null)
        {
            wall.setOwner(this.gameObject);
        }
    }

}
