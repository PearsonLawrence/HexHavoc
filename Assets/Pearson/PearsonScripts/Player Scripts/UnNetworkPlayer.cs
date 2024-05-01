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
    public bool isConnected;
    public bool isJoining;
    public bool isStarting;
    public TutorialManager tutorialManager;
    public GameObject offset;
    public MatchManager manager;
    public GestureEventProcessor gestureEP;
    public TPPortalRenderManager portalRenderManager;
    public GameObject TPRealm;
    public UnNetworkedSpellManager unSpellManager;
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

        if(currentPillar)
        {
            if(!isArena)
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
            else
            {
                if (!isJoining)
                {
                    
                    if(isConnected)
                    {
                        
                        if (manager.resetRound.Value || manager.isGameStarting.Value)
                        {
                            transform.position = matchPillar.playerPoint.transform.position;
                            transform.rotation = matchPillar.playerPoint.transform.rotation;
                        }
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

        if(interactRight.isTPTrigger && interactleft.isTPTrigger )
        {
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
            if (portalRenderManager.isUpdating)
            {
                portalRenderManager.resetThis();
            }
        }
    }
}
