using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
//using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

public class SpellLauncher : NetworkBehaviour
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

        float triggerValue = gripProperty.action.ReadValue<float>();
        
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
                case SpellType.fireball:
                    Debug.Log("Shoot Fireball");
                    spellManager.fireBall(transform);
                    break;
                case SpellType.wall:
                    Debug.Log("Shoot Wall");
                    spellManager.fireWall(transform);
                    break;
            }
        }


    }
}
