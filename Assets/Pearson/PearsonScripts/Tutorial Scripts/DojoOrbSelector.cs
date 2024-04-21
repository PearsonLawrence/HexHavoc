using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DojoOrbSelector : MonoBehaviour
{
    public int OrbIdx;
    public DojoControllerComponent controller;
    public bool isSwitch;
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerHand"))
        {
            if(isSwitch)
            {
                controller.ToggleDisplay();
            }
            else
            {
                var component = other.GetComponent<HandInteractableComponent>().spellManager;
                setPlayerElement(component);

                controller.setElementDisplay(OrbIdx);
            }
        }
    }

    public void setPlayerElement(UnNetworkedSpellManager spellManager)
    {
        switch(OrbIdx)
        {
            case 0:
                spellManager.elementSpeicalization = elementType.FIRE;
                break;
            case 1:
                spellManager.elementSpeicalization = elementType.WATER;
                break;
            case 2:
                spellManager.elementSpeicalization = elementType.EARTH;
                break;
            case 3:
                spellManager.elementSpeicalization = elementType.WIND;
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
        
    }
}
