using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandInteractableComponent : MonoBehaviour
{
    public GameObject currentInteractableItem;
    public LobbyUIManager lobbyManager;
    public bool isSelecting, isHolding;

    public void OnTriggerStay(Collider other)
    {
        string tag = other.gameObject.tag;

        switch(tag)
        {
            case "RotatePoint":
                DialFingerPointComponent point = other.gameObject.GetComponent<DialFingerPointComponent>();
                if (!isHolding)
                {
                    if(isSelecting)
                    {
                        currentInteractableItem = other.gameObject;
                        isHolding = true;
                        
                    }
                }
                else
                {
                    //other.gameObject.transform.position = transform.position;
                    if (point)
                    {
                        point.doPointHold();
                    }
                }
                break;
            case "LobbySelect":
                if (!isHolding)
                {
                    if (isSelecting)
                    {
                        currentInteractableItem = other.gameObject;
                        isHolding = true;

                    }
                }
                break;
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PortalLobby"))
        {
            lobbyManager.doJoin();
        }
    }
    public void release()
    {
        string tag = currentInteractableItem.tag;
        switch (tag)
        {
            case "RotatePoint":
                currentInteractableItem.transform.localPosition = Vector3.zero;
                break;
            case "LobbySelect":
                Rigidbody tempRB = currentInteractableItem.GetComponent<Rigidbody>();
                if(tempRB)
                {
                    tempRB.AddForce(transform.forward * 5);
                }
                break;
            case "PortalLobby":
                
                break;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        string tag = other.gameObject.tag;

        switch (tag)
        {
            case "RotatePoint":
                DialFingerPointComponent point = other.gameObject.GetComponent<DialFingerPointComponent>();
                if(point)
                {
                    point.doReset();
                }
                currentInteractableItem = null;
                break;
            case "LobbySelect":
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isHolding)
        {
            currentInteractableItem.transform.position = transform.position;
        }
        else
        {
           if(currentInteractableItem != null)
            {
                release();
            }
        }
    }
}
