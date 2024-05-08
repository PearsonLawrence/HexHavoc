//Author: Brandon yu
//Purpose: This script serves two putposes. The first is as a ready button. When the match has not started if the button is pressed the script will read it as the players readying up
//However if the match has already started and the button is pressed it will be read as the player wanting to rematch.

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    // Start is called before the first frame update
    public bool matchStarted = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMatchStarted(bool temp)
    {
        matchStarted = temp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NetworkHand"))
        {
            if (matchStarted == false)
            {
                Camera.main.gameObject.GetComponent<BackgroundMusic>().PlayArenaMusic();
                Debug.Log("Touched sensually");
                NetworkHandInteractable temp = other.GetComponent<NetworkHandInteractable>();
                NetworkPlayer networkPlayer = temp.parentObj;
                if (networkPlayer)
                {
                    Debug.Log("touched2");
                }
                if (temp)
                {
                    Debug.Log("touchable");
                }
                if (networkPlayer)
                {
                    MatchManager.Instance.DeclareReadyServerRpc(networkPlayer.GetComponent<NetworkObject>().OwnerClientId);
                }
            }

            if (matchStarted == true)
            {
                Debug.Log("Touched sensually");
                NetworkHandInteractable temp = other.GetComponent<NetworkHandInteractable>();
                NetworkPlayer networkPlayer = temp.parentObj;
                Camera.main.gameObject.GetComponent<BackgroundMusic>().ReturnToMainMenu();
                if (networkPlayer)
                {
                    Debug.Log("touched2");
                }
                if (temp)
                {
                    Debug.Log("touchable");
                }
                if (networkPlayer)
                {
                    MatchManager.Instance.DeclareRematchServerRpc(networkPlayer.GetComponent<NetworkObject>().OwnerClientId);
                }
            }
           
        }
    }
}
