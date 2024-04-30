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
    public bool canTeleport;
    public bool isWindPillar;
    [SerializeField] private GameObject firePillar;

    [HideInInspector] public NetworkVariable<bool> playerOn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    HealthManager tempManager;
    NetworkObject networkObject;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private bool isTutorialActionComplete;

    public bool test, testTwo;


    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    public float moveDuration = 5f;
    private bool isMoving = false;

    public GameObject playerPoint;
    // Start is called before the first frame update
    void Start()
    {
        matchManager = MatchManager.Instance;

        endPosition.position = endPosition.position + new Vector3(0f, Random.Range(-50f, 50f), 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            MovePillarClientRpc(pillarDirection.TOEND);
        }

        if (testTwo)
        {
            MovePillarClientRpc(pillarDirection.TOSTART);
        }

    }

    public void CanPlayerTeleport()
    {
        canTeleport = !playerOn.Value;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //get player on pillar information
            playerOn.Value = true;
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

        /*if (other.gameObject.CompareTag("Fireball"))
        {
            firePillar.SetActive(true);
            NetworkedProjectileComponent fb = other.gameObject.GetComponent<NetworkedProjectileComponent>();
            if (fb != null)
            {
                if (playerOn.Value && fb.GetWentThroughWall(elementType.FIRE))
                {

                    tempManager.Health.Value -= 20;
                    matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, tempManager.Health.Value);

                }
                StartCoroutine(TurnOffFirePillar(3f));
            }
        }*/
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
            playerOn.Value = false;
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
        if(playerOn.Value)
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

    [ClientRpc]
    public void MovePillarClientRpc(pillarDirection direction)
    {
        test = false;
        testTwo = false;
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine(direction));
        }
    }

    // Coroutine to handle the movement
    private IEnumerator MoveCoroutine(pillarDirection direction)
    {
        isMoving = true;
        float elapsedTime = 0f;

        Vector3 startPos;
        Vector3 endPos;

        if (direction == pillarDirection.TOSTART)
        {
            startPos = endPosition.position;
            endPos = startPosition.position;
        }
        else
        {
            startPos = startPosition.position;
            endPos = endPosition.position;
        }

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;

        if (matchManager.isGameStarting.Value)
            matchManager.DisableIsGameStartingServerRPC();
    }

    [ClientRpc]
    public void DestroyPillarClientRpc()
    {
        gameObject.SetActive(false);
    }
}
