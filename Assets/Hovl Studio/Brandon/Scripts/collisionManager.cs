using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class collisionManager : NetworkBehaviour
{
    [SerializeField] private SpellComponent spell;
    [SerializeField] private bool isSpell = false;

    private bool collisionHandled = false; // New boolean flag

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
        GameObject matchManagerObject = GameObject.FindWithTag("MatchManager");

        if (matchManagerObject != null)
        {
            // Get the MatchManager component from the GameObject
            matchManager = matchManagerObject.GetComponent<MatchManager>();

            if (matchManager == null)
            {
                Debug.LogError("MatchManager component not found on the MatchManager GameObject.");
            }
        }
        else
        {
            Debug.LogError("MatchManager GameObject not found in the hierarchy. Make sure it's active and tagged appropriately.");
        }

        // Reset the collisionHandled flag to false when the script starts
        collisionHandled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!collisionHandled && isSpell)
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

                // Set the collisionHandled flag to true after handling the collision
                collisionHandled = true;
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
                        other.GetComponent<HealthManager>().Health.Value -= 20;
                        matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, other.GetComponent<HealthManager>().Health.Value);
                        fireball fireballSelf = (fireball)spell;
                        fireballSelf.DoImpact();
                        break;
                }

                // Set the collisionHandled flag to true after handling the collision
                collisionHandled = true;
            }

            // Reset the collisionHandled flag at the end of the collision handling
            collisionHandled = false;
        }
    }

    // Reset the collisionHandled flag when the spell is reset or reused
    public void ResetCollisionHandled()
    {
        collisionHandled = false;
    }
}
