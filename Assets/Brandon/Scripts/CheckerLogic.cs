using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CheckerLogic : NetworkBehaviour
{
    private MatchManager matchManager;
    [SerializeField] private GameObject firePillar;

    //used to know if player is on pillar
    public bool playerOn;

    //store information about the player on the pillar
    HealthManager tempManager;
    NetworkObject networkObject;

    void Start()
    {
        matchManager = MatchManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            playerOn = true;
            networkObject = other.GetComponent<NetworkObject>();
            tempManager = other.GetComponent<HealthManager>();
            firePillar.SetActive(true);
            Debug.Log(networkObject);
            Debug.Log(tempManager);
            tempManager.Health.Value -= 20;
            matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, tempManager.Health.Value);
            StartCoroutine(TurnOffFirePillar(3f));
        }

        if (other.gameObject.CompareTag("Fireball"))
        {
            firePillar.SetActive(true);
            NetworkedProjectileComponent fb = other.gameObject.GetComponent<NetworkedProjectileComponent>();
            if (fb != null) {
                if (playerOn && fb.GetWentThroughWall())
                {

                    tempManager.Health.Value -= 20;
                    matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, tempManager.Health.Value);

                }
                StartCoroutine(TurnOffFirePillar(3f));
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerOn = false;
        }
    }

    private IEnumerator TurnOffFirePillar(float delay)
    {
        yield return new WaitForSeconds(delay);
        firePillar.SetActive(false);
    }
}
