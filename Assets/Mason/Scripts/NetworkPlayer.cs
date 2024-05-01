//Authoer: Mason Smith and Pearson Lawrence
//Purpose: this script is used for adjusting the body that the player posseses and replicates it over the netwrok. Ensures that the two players can see eachothers body and gestures.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Globalization;
using Unity.XR.CoreUtils;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform root;
    public Transform head;
    //public Transform body;
    public Transform leftHand;
    public Transform rightHand;
    public SpellLauncher leftHandSpell;
    public SpellLauncher rightHandSpell;
    public HandInteractableComponent left, right;
    public XROrigin xr;
    public GameObject headObj;
    public PillarLogic currentPillar;
    public Renderer[] meshToDisable;
    public UnNetworkPlayer playerXr;
    public bool isMoving;
    public float moveDuration = 5;

    [SerializeField] private SpellManager spellManager;
    [SerializeField] private PlayerNetwork playerNetwork;

    public SpellManager getSpellManager()
    {
        return spellManager;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            foreach (var item in meshToDisable)
            {
                Debug.Log("poo");
                item.enabled = false;
            }
           
            //GameObject temp = Camera.main.gameObject;
            //temp.transform.parent = headObj.transform;
            //temp.transform.position = Vector3.zero;
            //temp.transform.rotation = Quaternion.identity;
        }
        //PlacePlayers();


    }
    
    public void Start()
    {
        if (IsLocalPlayer)
        {
            RegisterPlayerOnServerRpc(OwnerClientId);
        }
        PlacePlayers();
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
       

        if (root) root.position = VRRigReferences.Singleton.root.position;
        if (root) root.rotation = VRRigReferences.Singleton.root.rotation;

        if (head) head.position = VRRigReferences.Singleton.head.position;
        if (head) head.rotation = VRRigReferences.Singleton.head.rotation;

        //body.position = VRRigReferences.Singleton.body.position;
        //body.rotation = VRRigReferences.Singleton.body.rotation;

        if (leftHand && left) leftHand.position = left.gameObject.transform.position;
        if (leftHand && left) leftHand.forward = left.gameObject.transform.forward;

        if (rightHand && right) rightHand.position = right.gameObject.transform.position;
        if (rightHand && right) rightHand.forward = right.gameObject.transform.forward;

        //leftHandSpell.gripProperty = VRRigReferences.Singleton.leftGripProperty;
        //rightHandSpell.gripProperty = VRRigReferences.Singleton.rightGripProperty;

        if (MatchManager.Instance.isRoundReset.Value)
        {
            PlacePlayers();
        }

       
    }

    [ServerRpc]
    private void RegisterPlayerOnServerRpc(ulong clientId)
    {
        if (GetComponent<NetworkObject>().OwnerClientId == 0)
        {
            MatchManager.Instance.playerOneNetwork = this;
        }
        else if(GetComponent<NetworkObject>().OwnerClientId == 1)
        {
            MatchManager.Instance.playerTwoNetwork = this;
        }
        MatchManager.Instance.RegisterPlayer(clientId);

    }
    public void PlacePlayers()
    {
        //if (IsOwner)  transform.position = MatchManager.Instance.playerBody.position;
        if (OwnerClientId == 0 && IsOwner)
        {
            cameraManager tempCam = Camera.main.gameObject.GetComponent<cameraManager>();
            if (tempCam) xr = tempCam.xr;


            if (MatchManager.Instance.matchGoing)
            {
                transform.position = xr.transform.position;
            }
            playerXr = xr.GetComponent<UnNetworkPlayer>();
            playerXr.currentPillar = playerXr.matchPillar;
            playerXr.spellmanager = GetComponent<SpellManager>();
            playerXr.gestureEP.spellmanager = GetComponent<SpellManager>();
            left = playerXr.interactleft;
            right = playerXr.interactRight;
            left.parentObj = this;
            right.parentObj = this;
            // playerXr.setSpellManagerProcessors();

            playerXr.isJoining = false;
            playerXr.isConnected = true;
            playerXr.isTeleported = true;
            playerXr.TPRealm.SetActive(false);
            //currentPillar.playerOn.Value = true;
            //transform.position = MatchManager.Instance.playerBody.position;
        }
        if (OwnerClientId == 1 && IsOwner)
        {
            cameraManager tempCam = Camera.main.gameObject.GetComponent<cameraManager>();
            if (tempCam) xr = tempCam.xr;

            if (MatchManager.Instance.matchGoing)
            {
                transform.position = xr.transform.position;
            }
            playerXr = xr.GetComponent<UnNetworkPlayer>();
            playerXr.currentPillar = playerXr.matchPillar;
            playerXr.spellmanager = GetComponent<SpellManager>();
            playerXr.gestureEP.spellmanager = GetComponent<SpellManager>();
            // playerXr.setSpellManagerProcessors();
            left = playerXr.interactleft;
            right = playerXr.interactRight;
            left.parentObj = this;
            right.parentObj = this;

            playerXr.isJoining = false;
            playerXr.isConnected = true;
            playerXr.isTeleported = true;
            playerXr.TPRealm.SetActive(false);
            //xr.Origin.transform.position = currentPillar.playerPoint.transform.position;
            //currentPillar.playerOn.Value = true;
        }


    }
    /*
    [ClientRpc]
    public void MovePlayerToStartClientRpc()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCorutine());
        }
    }*/

    IEnumerator MoveCorutine()
    {
        isMoving = true;
        float elapsedTime = 0f;

        Vector3 startPos;
        Vector3 endPos;

        startPos = MatchManager.Instance.playerBody.position;
        endPos = MatchManager.Instance.spawnPosition2.position;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            xr.Origin.transform.position = currentPillar.playerPoint.transform.position;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }
}


