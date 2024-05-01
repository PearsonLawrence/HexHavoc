//Author: Pearson Lawrence
//Purpose: Call script to spawn spells over network using RPCs utilizing the spellmanager script developed by Brandon
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
//using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

public class SpellLauncher : NetworkBehaviour //Network behavior to allow transmition of data between clients and server
{
    // Start is called before the first frame update\
    public InputActionProperty gripProperty;
    public bool isCharging;
    [SerializeField] private SpellManager spellManager;
    public SpellType spell;

    public void setSpellManager(SpellManager spell)
    {
        spellManager = spell;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        /*
        float triggerValue = gripProperty.action.ReadValue<float>(); //Get the value of the motion controllers grip to launch spells
            //TODO: Switch to gesture based casting
        

        //Test to display grab and release
        if(triggerValue > 0.1f)
        {
            //Debug.Log("Grab");
        }
        else
        {
            // Debug.Log("Release");

        }

        triggerValue = Mathf.Clamp(triggerValue, 0f, 1f);

        if (triggerValue > 0.1f && !isCharging) 
        {
            Debug.Log("Grab");
            isCharging = true;
        }
        else if(triggerValue <= 0.1f && isCharging)
        {
            Debug.Log("Release");
            isCharging = false;
            //SpawnFireball
            switch(spell) 
            {
                case SpellType.projectile:
                    //Debug.Log("Shoot Fireball");
                    //spellManager.fireBall(transform); //Calls a fireball rpc across the network at transform location
                    break;
                case SpellType.wall:
                    //Debug.Log("Shoot Wall");
                    //spellManager.fireWall(transform); //Calls a firewall rpc across the network at transform location
                    break;
            }
        }*/


    }
}
