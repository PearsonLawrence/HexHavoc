//Created by Pearson Lawrence and Mason Smith. Creates player avatar over network so other players can see one another in game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Globalization;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform root;
    public Transform head;
    //public Transform body;
    public Transform leftHand;
    public Transform rightHand;
    public SpellLauncher leftHandSpell;
    public SpellLauncher rightHandSpell;

    public GameObject headObj;

    public Renderer[] meshToDisable;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.Log("WTFFFFFFFFFFFFFFFFFFF");
        if (IsOwner)
        {
            Debug.Log("SHIIIIIIIIIIIIIIT");
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
        PlacePlayers();


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

        if (leftHand) leftHand.position = VRRigReferences.Singleton.leftHand.position;
        if (leftHand) leftHand.rotation = VRRigReferences.Singleton.leftHand.rotation;

        if (rightHand) rightHand.position = VRRigReferences.Singleton.rightHand.position;
        if (rightHand) rightHand.rotation = VRRigReferences.Singleton.rightHand.rotation;
        leftHandSpell.gripProperty = VRRigReferences.Singleton.leftGripProperty;
        rightHandSpell.gripProperty = VRRigReferences.Singleton.rightGripProperty;

        if (MatchManager.Instance.isRoundReset.Value)
        {
            PlacePlayers();
        }

       
    }

    [ServerRpc]
    private void RegisterPlayerOnServerRpc(ulong clientId)
    {
        MatchManager.Instance.RegisterPlayer(clientId);
    }

    public void PlacePlayers()
    {
        if (OwnerClientId == 0)
        {
            VRRigReferences.Singleton.root.position = MatchManager.Instance.spawnPosition1.position;
        }
        if (OwnerClientId == 1)
        {
            VRRigReferences.Singleton.root.position = MatchManager.Instance.spawnPosition2.position;
        }
    }
}


