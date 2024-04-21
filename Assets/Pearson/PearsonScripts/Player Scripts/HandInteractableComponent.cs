using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class HandInteractableComponent : NetworkBehaviour
{
    public GameObject currentInteractableItem;
    public LobbyUIManager lobbyManager;
    public bool isSelecting, isHolding;
    public NetworkPlayer parentObj;
    public UnNetworkPlayer parentUnNetworkObj;
    public UnNetworkedSpellManager spellManager;
    public GestureEventProcessor gestureEP;
    public bool isTPTrigger;
    public bool isInteractableItem;
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
            case "Element":
                if (!isHolding)
                {
                    if (isSelecting)
                    {
                        currentInteractableItem = other.gameObject;
                        isHolding = true;
                        gestureEP.isTouchingElement = true;
                    }
                }
                break;
            case "PlayerHand":
                if (!isHolding)
                {
                    if (isSelecting)
                    {
                        isTPTrigger = true;
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

        if(other.CompareTag("TpPortal"))
        {
           PortalTeleportComponent temp = other.gameObject.GetComponent<PortalTeleportComponent>();
            parentUnNetworkObj.currentPillar = temp.getTpToPillar();
            if (temp.isTutorialGate) parentUnNetworkObj.isTutorial = (parentUnNetworkObj.isTutorial) ? false : true;
            if (temp.isArenaGate) parentUnNetworkObj.isArena = (parentUnNetworkObj.isArena) ? false : true;
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
                currentInteractableItem = null;
                if (tempRB)
                {
                    tempRB.AddForce(transform.forward * 50);
                }
                break;
            case "PortalLobby":

                break;
            case "Element":
                Rigidbody tempRB2 = currentInteractableItem.GetComponent<Rigidbody>();
                currentInteractableItem = null;
                if (tempRB2)
                {
                    tempRB2.velocity = Vector3.zero;
                }
                gestureEP.isTouchingElement = false;
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
            if(!isTPTrigger) currentInteractableItem.transform.position = transform.position;
        }
        else
        {
           if(currentInteractableItem != null)
            {
                release();
            }
           else
            {
                if(isTPTrigger)
                {
                    isTPTrigger = false;
                }
            }
        }
    }
}
