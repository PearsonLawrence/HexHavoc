using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NetworkHand"))
        {
            Debug.Log("Touched sensually");
            NetworkHandInteractable temp = other.GetComponent<NetworkHandInteractable>();
            NetworkPlayer networkPlayer = temp.parentObj;
            if (networkPlayer)
            {
                Debug.Log("touched2");
            }
            if(temp)
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
