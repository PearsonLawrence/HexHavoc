using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform root;
    public Transform head;
    //public Transform body;
    public Transform leftHand;
    public Transform rightHand;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
        {
            Debug.Log("update");
            root.position = VRRigReferences.Singleton.root.position;
            root.rotation = VRRigReferences.Singleton.root.rotation;

            head.position = VRRigReferences.Singleton.head.position;
            head.rotation = VRRigReferences.Singleton.head.rotation;

            //body.position = VRRigReferences.Singleton.body.position;
            //body.rotation = VRRigReferences.Singleton.body.rotation;

            leftHand.position = VRRigReferences.Singleton.leftHand.position;
            leftHand.rotation = VRRigReferences.Singleton.leftHand.rotation;

            rightHand.position = VRRigReferences.Singleton.rightHand.position;
            rightHand.rotation = VRRigReferences.Singleton.rightHand.rotation;
        }
    }
}
