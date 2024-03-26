
//Author: Pearson Lawrence
//Purpose: Was meant to assign camera to network player. But obsolete
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
        //Sets the players head to correct camera
        base.OnNetworkSpawn();
        if (IsOwner && headObj)
        {
            GameObject temp = Camera.main.gameObject;
        }
       
    }

}
