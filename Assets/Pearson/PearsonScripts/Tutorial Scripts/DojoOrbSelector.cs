using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DojoOrbSelector : MonoBehaviour
{
    public int OrbIdx;
    public DojoControllerComponent controller;
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var component = other.GetComponent<HandInteractableComponent>().spellManager;
            setPlayerElement(component);

            controller.setElementDisplay(OrbIdx);
        }
    }

    public void setPlayerElement(UnNetworkedSpellManager spellManager)
    {
        switch(OrbIdx)
        {
            case 0:
                spellManager.setPlayerSpecialization(elementType.FIRE);
                break;
            case 1:
                spellManager.setPlayerSpecialization(elementType.WATER);
                break;
            case 2:
                spellManager.setPlayerSpecialization(elementType.EARTH);
                break;
            case 3:
                spellManager.setPlayerSpecialization(elementType.WIND);
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
