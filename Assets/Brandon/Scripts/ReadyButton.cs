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
            if (!matchStarted)
            {
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

            if (matchStarted)
            {
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
           
        }
    }
}
