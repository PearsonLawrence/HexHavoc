//Author: Pearson Lawrence
//Purpose: This component handles the player teleportation and a variety of other things that control the game state.
//This controls assigning grip values for the hands as well as ensuring that rendering the hand portal works.

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnNetworkPlayer : MonoBehaviour
{
    // Start is called before the first frame update\
    public InputActionProperty leftGripProperty;
    public InputActionProperty rightGripProperty;
    public HandInteractableComponent interactleft, interactRight;
    public PillarLogic currentPillar;
    public PillarLogic matchPillar;
    public List<GestureEventProcessor> processors;
    public SpellManager spellmanager;
    public bool isGame;
    public bool isArena;
    public bool isTeleported;
    public bool isTutorial;
    public bool isDojo;
    public bool isConnected;
    public bool isJoining;
    public bool isStarting;
    public bool hasPlayedAudio;
    public GameObject offset;
    public MatchManager manager;
    public GestureEventProcessor gestureEP;
    public TPPortalRenderManager portalRenderManager;
    public GameObject TPRealm;
    public UnNetworkedSpellManager unSpellManager;
    public AudioSource introAudio;
    void Start()
    {
        unSpellManager = GetComponent<UnNetworkedSpellManager>();
        manager = MatchManager.Instance; 
        
    }
    
    public void setSpellManagerProcessors()
    {
        foreach(GestureEventProcessor processor in processors)
        {
           processor.spellmanager = spellmanager;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = leftGripProperty.action.ReadValue<float>();
        float triggerValue2 = rightGripProperty.action.ReadValue<float>();

        //Teleportation
        if(currentPillar)
        {
            if(!isArena)
            {
                //If it is not the arena area teleport normally. By taking the players current pillar that is set through other scripts. If the bool is set to true then player will teleport to specified pillar tp point
                if (isTeleported)
                {
                    transform.position = currentPillar.playerPoint.transform.position;
                    Vector3 temp = new Vector3(currentPillar.playerPoint.transform.position.x, currentPillar.playerPoint.transform.position.y, currentPillar.playerPoint.transform.position.z);
                    offset.transform.position = currentPillar.playerPoint.transform.position;

                    transform.forward = currentPillar.playerPoint.transform.position;
                    offset.transform.forward = transform.forward;

                    isTeleported = false;

                }

                
            }
            else
            {
                //If the player is not currently joining over relay
                if (!isJoining)
                {
                    //And the player is connected over relay
                    if(isConnected)
                    {
                        //Play spirit guide entrence audio once
                        if(!hasPlayedAudio)
                        {
                            introAudio.Play();
                            hasPlayedAudio = true;
                        }
                        //If the round is being reset then teleport to start pillar
                        if (manager.resetRound.Value)
                        {
                            transform.position = matchPillar.playerPoint.transform.position;
                        }
                        //If the game is starting attach to current pillar so the player moves with it
                        else if (manager.isGameStarting.Value)
                        {
                            transform.position = matchPillar.playerPoint.transform.position;
                            transform.rotation = matchPillar.playerPoint.transform.rotation;
                        }
                        //If the player teleports and the other two conditions are not met then teleport the player to the new pillar set through teleportation manager
                        else if (isTeleported)
                        {
                            transform.position = currentPillar.playerPoint.transform.position;
                            offset.transform.position = currentPillar.playerPoint.transform.position;

                            isTeleported = false;
                        }
                        /*else if (manager.isGameStarting.Value)
                        {
                            transform.position = currentPillar.playerPoint.transform.position;
                            transform.rotation = currentPillar.playerPoint.transform.rotation;
                            //offset.transform.position = currentPillar.playerPoint.transform.position;
                        }*/
                    }
                    //If you are not connected and you are not joining then teleport nornally
                    else
                    {
                        if (isTeleported)
                        {
                            transform.position = currentPillar.playerPoint.transform.position;
                            Vector3 temp = new Vector3(currentPillar.playerPoint.transform.position.x, currentPillar.playerPoint.transform.position.y, currentPillar.playerPoint.transform.position.z);
                            offset.transform.position = currentPillar.playerPoint.transform.position;

                            transform.forward = currentPillar.playerPoint.transform.position;
                            offset.transform.forward = transform.forward;

                            isTeleported = false;

                        }
                    }
                }
                
            }
        }

        //Update trigger values
        if (triggerValue > 0.1f)
        {
            interactleft.isSelecting = true;
        }
        else if (triggerValue <= 0.1f )
        {
            interactleft.isSelecting = false;
            interactleft.isHolding = false;
        }
        if (triggerValue2 > 0.1f)
        {
            interactRight.isSelecting = true;
        }
        else if (triggerValue2 <= 0.1f)
        {
            interactRight.isSelecting = false;
            interactRight.isHolding = false;
        }

        //If your hands detect that they are interacting with eachother and gripping
        if(interactRight.isTPTrigger && interactleft.isTPTrigger )
        {
            //Render the portal
            if(!portalRenderManager.isUpdating)
            {
                portalRenderManager.StartThis();
            }
            else
            {
                portalRenderManager.updatePortal();
            }

        }
        else
        {
            //Stop rendering the portal
            if (portalRenderManager.isUpdating)
            {
                portalRenderManager.resetThis();
            }
        }
    }
}
