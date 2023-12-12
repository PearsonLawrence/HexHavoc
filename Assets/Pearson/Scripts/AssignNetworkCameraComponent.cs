using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem.XR;

public class AssignNetworkCameraComponent : NetworkBehaviour
{
    public GameObject headObj;
    public List<MonoBehaviour> componentsToDisable;
    public TrackedPoseDriver headDriver;
    public CharacterController controller;
    private bool isDone = false;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner && headObj)
        {
            GameObject temp = Camera.main.gameObject;
            temp.transform.parent = headObj.transform;
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localRotation = Quaternion.identity;
        }
        else
        {
            for (int i = 0; i < componentsToDisable.Count; i++)
            {
               componentsToDisable[i].enabled = false;
            }
            if (controller)
            {
               // controller.enabled = false;
            }
        }
    }

    public void Start()
    {
        
    }
    public void Update()
    {
        //if (!IsOwner)
        //{
        //    if (!isDone)
        //    {
        //        for (int i = 0; i < componentsToDisable.Count; i++)
        //        {
        //            componentsToDisable[i].enabled = false;

        //        }
        //        isDone = true;
        //    }

        //}
    }
}
