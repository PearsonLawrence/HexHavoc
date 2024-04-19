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
    void Start()
    {
        
    }

    public void setSpellManagerProcessors()
    {
        Debug.Log("Err2: " + spellmanager);
        foreach(GestureEventProcessor processor in processors)
        {
           processor.spellmanager = spellmanager;
            Debug.Log("Err*: " + processor.spellmanager);
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
                if (!isConnected && !isJoining)
                {

                    if (isTeleported)
                    {
                        transform.position = currentPillar.playerPoint.transform.position;
                        offset.transform.position = currentPillar.playerPoint.transform.position;


                        isTeleported = false;
                    }
                }
                if (isConnected && !isJoining)
                {

                    if (isTeleported)
                    {
                        transform.position = currentPillar.playerPoint.transform.position;
                        offset.transform.position = currentPillar.playerPoint.transform.position;


                        isTeleported = false;
                    }
                }
                if (isConnected && !isJoining && isStarting)
                {

                    
                        transform.position = currentPillar.playerPoint.transform.position;
                        offset.transform.position = currentPillar.playerPoint.transform.position;

                }
                if (isConnected && isJoining)
                {

                    isJoining = false;
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
            interactleft .isHolding = false;
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
    }
}
