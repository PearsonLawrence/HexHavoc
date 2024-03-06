using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSelect : SpellComponent
{
    // Start is called before the first frame update

    public elementType elementtype;

    SpellManager playerSpellManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("isplayer");
            playerSpellManager = other.GetComponent<SpellManager>();

            playerSpellManager.SetElementType(elementtype);
        }
    }
}
