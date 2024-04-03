using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnnetworkedSpellManager : MonoBehaviour
{
    [SerializeField] private Transform fireballPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Y))
        {
            SpawnProjectile();
        }
    }

    public void SpawnProjectile()
    {
        // Define the distance in front of the player where the fireball will spawn
        float spawnDistance = 2f;

        // Calculate the spawn position based on the player's position and forward direction
        //Vector3 spawnPosition = LeftHandPos.position + LeftHandPos.forward * spawnDistance;
        Vector3 spawnPosition = Vector3.zero;


        // Instantiate the fireball at the calculated spawn position

        NetworkedProjectileComponent projectile = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity /*LeftHandPos.rotation*/).GetComponent<NetworkedProjectileComponent>();
        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Spawn(); // Spawn the object on the network

            projectile.setOwner(this.gameObject); // Now safe to set the owner

            //Debug.Log(this.gameObject);

            // Additional initialization as needed
            Vector3 playerForward = Camera.main.transform.forward;
            projectile.SetDirection(/*RightHandPos.*/Vector3.forward);
            // projectile.SetDirection(Vector3.forward);
        }
        else
        {
            Debug.LogError("NetworkObject not found on the fireball prefab.");
        }

        // Add the fireball to the list of casted spells
        //castedSpells.Add(projectile.transform);

        // Set the parent in the fireball component
        NetworkedProjectileComponent Projectile = projectile.GetComponent<NetworkedProjectileComponent>();
        if (Projectile != null)
        {
            Projectile.unNetworkedParent = this;
        }
        else
        {
            Debug.LogError("fireball component not found on the fireball prefab.");
        }
    }
}
