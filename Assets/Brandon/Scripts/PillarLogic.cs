using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public enum pillarDirection
{
    TOSTART,
    TOEND
}
public class PillarLogic : NetworkBehaviour
{
    private MatchManager matchManager;
    private bool canTeleport;
    [SerializeField] private GameObject firePillar;
    public bool playerOn;
    HealthManager tempManager;
    NetworkObject networkObject;

    Transform startPosition;
    Transform endPosition;

    // Start is called before the first frame update
    void Start()
    {
        matchManager = MatchManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CanPlayerTeleport()
    {
        canTeleport = !playerOn;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //get player on pillar information
            playerOn = true;
            networkObject = other.GetComponent<NetworkObject>();
            tempManager = other.GetComponent<HealthManager>();

            //test code
            //SpawnFirePillarRpc();
            StartCoroutine(ActivateFirePillar(3f));
            //firePillar.SetActive(true);
            //tempManager.Health.Value -= 20;
            //matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, tempManager.Health.Value);
            //StartCoroutine(TurnOffFirePillar(3f));
        }

        if (other.gameObject.CompareTag("Fireball"))
        {
            firePillar.SetActive(true);
            NetworkedProjectileComponent fb = other.gameObject.GetComponent<NetworkedProjectileComponent>();
            if (fb != null)
            {
                if (playerOn && fb.GetWentThroughWall(elementType.FIRE))
                {

                    tempManager.Health.Value -= 20;
                    matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, tempManager.Health.Value);

                }
                StartCoroutine(TurnOffFirePillar(3f));
            }
        }
    }

    [ServerRpc]
    public void SpawnFirePillarServerRpc()
    {
        StartCoroutine(ActivateFirePillar(3f));
    }

    [ClientRpc/*(RequireOwnership = false)*/]
    public void DespawnFirePillarClientRpc()
    {
        StartCoroutine(TurnOffFirePillar(3f));
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

    private IEnumerator ActivateFirePillar(float delay)
    {
        yield return new WaitForSeconds(delay);
        firePillar.SetActive(true);
        if(playerOn)
        {
            if (IsOwner)
            {
                DealDamageServerRpc();
            }
        }

        DespawnFirePillarClientRpc();

        //yield return new WaitForSeconds (delay);
        //firePillar.SetActive(false);
    }

    [ServerRpc]
    public void DealDamageServerRpc()
    {
        Debug.Log("idk");
        
        Debug.Log("seomth");
        matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 20);
        Debug.Log("gdsh");
    }

    public void movePillar(pillarDirection pillardirection)
    {
        if(pillardirection == pillarDirection.TOEND)
        {

        }
        else if(pillardirection == pillarDirection.TOSTART)
        {

        }
    }
}
