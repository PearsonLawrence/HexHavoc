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

    public void setPlayerElement(UnnetworkedSpellManager spellManager)
    {

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
