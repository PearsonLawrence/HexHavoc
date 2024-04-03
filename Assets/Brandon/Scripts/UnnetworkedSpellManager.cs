using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class UnNetworkedSpellManager : MonoBehaviour
{
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

    private elementType playerSpecialzation = elementType.EARTH;
    private Transform desieredProjectile;
    private Transform desiredWall;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Y))
        {
            SpawnProjectile();
            Debug.Log("further back");
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            SpawnWall();
        }
    }

    // Method for spawning a projectile
    public void SpawnProjectile()
    {
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
        Vector3 spawnPosition = Vector3.zero;
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

    public void SpawnWall()
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

        Vector3 spawnPosition = new Vector3(0, 0, 10);
        WallComponent wall = Instantiate(desiredWall, spawnPosition, Quaternion.identity).GetComponent<WallComponent>();

        if(wall != null)
        {
            wall.setOwner(this.gameObject);
        }
    }

}
