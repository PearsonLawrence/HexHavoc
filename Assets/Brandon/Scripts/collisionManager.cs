//Author:Brandon(Ri) Yu
//Purpose: Detect collsions within the game and determining what hit what. i.e a fireball hit the player or
//a fireball hitting the wall

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class collisionManager : NetworkBehaviour
{
    [SerializeField] private SpellComponent spell;
    [SerializeField] private bool isSpell = false;

    private MatchManager matchManager;

    public SpellComponent getSpell()
    {
        return spell;
    }

    public void setSpell(SpellComponent temp)
    {
        spell = temp;
    }

    void Start()
    {
        // Find the MatchManager GameObject in the hierarchy
        MatchManager matchManagerObject = MatchManager.Instance;

        if (matchManagerObject != null)
        {
            // Get the MatchManager component from the GameObject
            matchManager = matchManagerObject;

            if (matchManager == null)
            {
                Debug.LogError("MatchManager component not found on the MatchManager GameObject.");
            }
        }
        else
        {
            Debug.LogError("MatchManager GameObject not found in the hierarchy. Make sure it's active and tagged appropriately.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSpell)
        {
            SpellComponent tempSpell = other.transform.gameObject.GetComponent<SpellComponent>();
            if (tempSpell)
            {
                //Debug.Log("Have Spell");
                GameObject tempSpellOwner = tempSpell.getOwner();
                GameObject spellOwner = spell.getOwner();

                if (tempSpellOwner != null && spellOwner != null && tempSpellOwner != spellOwner)
                {
                    switch (tempSpell.spellType)
                    {
                        case SpellType.wall:
                            Debug.Log("hit wall collision");
                            WallSpell tempWall = (WallSpell)tempSpell;
                            tempWall.DoSpellImpact();
                            break;
                        case SpellType.fireball:
                            fireball fireball = (fireball)tempSpell;
                            fireball.DoImpact();
                            Debug.Log("Wrong fireball poooooooooooooooof");
                            break;
                    }

                    switch (spell.spellType)
                    {
                        case SpellType.wall:
                            break;
                        case SpellType.fireball:
                            Debug.Log("Fireball poof");
                            fireball fireballSelf = (fireball)spell;
                            fireballSelf.DoImpact();
                            break;
                    }
                }

            }
            else if (other.CompareTag("Player"))
            {
                switch (spell.spellType)
                {
                    case SpellType.wall:
                        break;
                    case SpellType.fireball:
                        NetworkObject networkObject = other.GetComponent<NetworkObject>();

                        Debug.Log("Hit Player" + networkObject.OwnerClientId);
                        HealthManager tmepManager = other.GetComponent<HealthManager>();

                        tmepManager.Health.Value -= 20;
                        matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, tmepManager.Health.Value);
                        fireball fireballSelf = (fireball)spell;
                        fireballSelf.DoImpact();
                        break;
                }
            }
        }
    }
}
