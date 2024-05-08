//Author: Pearson Lawrence
//Purpose: This component handles what happens when the player grips their hands. This component is responsible for all object-hand interactions in the game, from grabbing elements to UI objects.
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
    public bool isLeft;
    private MatchManager matchManager;

    //If your hand is staying inside of an object
    public void OnTriggerStay(Collider other)
    {
        string tag = other.gameObject.tag;

        switch(tag)
        {
            //If this is the dial finger point
            case "RotatePoint":
                DialFingerPointComponent point = other.gameObject.GetComponent<DialFingerPointComponent>();
                if (!isHolding)
                {
                    if(isSelecting)
                    {
                        //If is holding update currentInteractable item
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
                //If the Lobby selector is being held
            case "LobbySelect":
                if (!isHolding)
                {
                    //If is holding update currentInteractable item
                    if (isSelecting)
                    {
                        currentInteractableItem = other.gameObject;
                        isHolding = true;

                    }
                }
                break;
                //If touching element do same but update gesture event processor
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
                //If you grip another hand then have the portal render component start working
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

    //This handles on collision event that are supposed to happen instantly
    public void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        switch(tag)
        {
            case "PortalLobby":
                //If touching a lobby portal then begin join
                lobbyManager.doJoin();
                break;
                //If this is a teleport portal then assign correct variables
            case "TpPortal":
                PortalTeleportComponent temp = other.gameObject.GetComponent<PortalTeleportComponent>();
                parentUnNetworkObj.currentPillar = temp.getTpToPillar();
                if (temp.isTutorialGate) parentUnNetworkObj.isTutorial = (parentUnNetworkObj.isTutorial) ? false : true;
                if (temp.isArenaGate) parentUnNetworkObj.isArena = (parentUnNetworkObj.isArena) ? false : true;
                break;
           
            case "Element":
                //If touching an element and you are not gripping your hand
                if (!isSelecting)
                {
                    SpellClassifier spell = other.gameObject.GetComponent<SpellClassifier>();

                    //Launch earth spell and assign spell directions if the element is of type earth.
                    if (spell.element == SpellClassifier.ElementType.EARTH)
                    {
                        Vector3 directionBetween = -(this.gameObject.transform.position - other.gameObject.transform.position).normalized;

                        //If the player has a spell manager spawn over network
                        if (parentUnNetworkObj.spellmanager)
                        {
                            parentUnNetworkObj.spellmanager.spellDirection.Value = directionBetween;

                            if (isLeft)
                                parentUnNetworkObj.spellmanager.fireLeftProjectile();
                            else
                                parentUnNetworkObj.spellmanager.fireRightProjectile();
                            Debug.Log("HAND SLAP");
                        }
                        //If the player does not have a spell manager spawn locally
                        else
                        {
                            parentUnNetworkObj.unSpellManager.spellDirection = directionBetween;
                            Debug.Log("dir : " + directionBetween + " : managerDir : " + parentUnNetworkObj.unSpellManager.spellDirection);
                            parentUnNetworkObj.unSpellManager.SpawnHitProjectile();
                            Debug.Log("HAND SLAP UNNETWORK");
                        }

                        parentUnNetworkObj.gestureEP.isElementSpawned = false;
                        Destroy(other.gameObject, .01f);
                    }
                }
                break;
            case "PlayerHand":
                //If you touch tour hand and you are holding an air pistol then reload it.
                if(gestureEP.isTouchingElement)
                {
                    if(gestureEP.isElementSpawned)
                    {
                        if ((gestureEP.isTouchingElement && !isLeft && gestureEP.IsGunSpawnedLeft()) || (gestureEP.isTouchingElement && isLeft && gestureEP.IsGunSpawnedRight()))
                        {
                            gestureEP.currentGunAmmo = gestureEP.maxAmmo;
                            gestureEP.hasAmmo = true;
                            gestureEP.reloadCount++;
                            Debug.Log("Reload Successfully Casted. Current Ammo Left: " + gestureEP.currentGunAmmo);
                            gestureEP.isElementSpawned = false;
                            gestureEP.CurrentElement.destroyThis();
                            
                        }
                    }
                    
                }
                break;
        }
        
    }
    //On release of an object
    public void release()
    {
        string tag = currentInteractableItem.tag;
        switch (tag)
        {
            case "RotatePoint":
                //If its the rotate point reset its position
                currentInteractableItem.transform.localPosition = Vector3.zero;
                break;
            case "LobbySelect":
                //If it is the lobby selector then launch it in the forward hand direction
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
                //If it is an element then make sure it stops moving when you release it
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
                    point.doReset(); //Resets the point values on release
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
        matchManager = MatchManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHolding) //If holding then update the objects position to hand position
        {
            if(!isTPTrigger) currentInteractableItem.transform.position = transform.position;
        }
        else
        {
           if(currentInteractableItem != null) //If there is no item in hand then release
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
