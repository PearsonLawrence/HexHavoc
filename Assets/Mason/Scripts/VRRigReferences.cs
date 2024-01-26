//Created by Pearson Lawrence and Mason Smith. Creates objects for XROrigin to reference to help create Player models and control the Player's POV.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.Netcode;
//using Unity.Services.Authentication;
//using Unity.Services.Core;

public class VRRigReferences : MonoBehaviour
{
    public static VRRigReferences Singleton;

    //private string playerID;
    //private string playerName;

    public Transform root;
    public Transform head;
    //public Transform body;
    public Transform leftHand;
    public Transform rightHand;
    public InputActionProperty leftGripProperty;
    public InputActionProperty rightGripProperty;

    private void Awake()
    {
        Singleton = this;
    }

    /*private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            playerID = AuthenticationService.Instance.PlayerId;
            Debug.Log("Signed in " + playerID);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "TestName" + UnityEngine.Random.Range(0, 1000);
        Debug.Log(playerName);
    }*/
}
