using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public bool isTutorial;
    public TutorialManager tutorialManager;
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
                transform.position = currentPillar.playerPoint.transform.position;

               // transform.forward = currentPillar.playerPoint.transform.forward;
                isArena = true;

            }
            transform.position = currentPillar.playerPoint.transform.position;

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
    }
}
