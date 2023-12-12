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
    public List<MonoBehaviour> componentsToDisable;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            foreach (var item in meshToDisable)
            {
                item.enabled = false;
            }
            GameObject temp = Camera.main.gameObject;
            temp.transform.parent = headObj.transform;
            temp.transform.position = Vector3.zero;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
        {
        }
    }
}
