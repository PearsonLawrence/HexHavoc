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
            GameObject tempSpellOwner = tempSpell.getOwner();
            GameObject spellOwner = spell.getOwner();

            if (spell.spellType == SpellType.projectile)
            {
                NetworkedProjectileComponent temp = (NetworkedProjectileComponent)spell;
                if(temp != null)
                {
                    switch (temp.elementtype)
                    {
                        //This is the projectile that is thrown not what is ebing collided with
                        case elementType.FIRE:

                            if (tempSpellOwner != null && spellOwner != null && tempSpellOwner != spellOwner)
                            {
                                switch (tempSpell.spellType)
                                {
                                    case SpellType.wall:
                                        Debug.Log("hit wall collision");
                                        NetworkedWallComponent tempWall = (NetworkedWallComponent)tempSpell;
                                        tempWall.DoSpellImpact();
                                        break;
                                    case SpellType.projectile:
                                        NetworkedProjectileComponent projectile = (NetworkedProjectileComponent)tempSpell;
                                        projectile.DoImpact();
                                        Debug.Log("Wrong fireball poooooooooooooooof");
                                        break;
                                }

                                switch (spell.spellType)
                                {
                                    case SpellType.wall:
                                        break;
                                    case SpellType.projectile:
                                        Debug.Log("Fireball poof");
                                        NetworkedProjectileComponent projectileSelf = (NetworkedProjectileComponent)spell;
                                        projectileSelf.DoImpact();
                                        break;
                                }
                            }
                            else if (tempSpellOwner != null && spellOwner != null && tempSpellOwner == spellOwner)
                            {
                                if (tempSpell.spellType == SpellType.wall)
                                {
                                    NetworkedWallComponent tempWall = (NetworkedWallComponent)(tempSpell);
                                    if (tempWall.elementtype == elementType.FIRE)
                                    {
                                        NetworkedProjectileComponent fireball = (NetworkedProjectileComponent)spell;
                                        fireball.SetWentThroughWall(true, temp.elementtype);
                                    }
                                }
                            }

                            break;

                        case elementType.WATER:
                            if (tempSpellOwner != null && spellOwner != null && tempSpellOwner != spellOwner)
                            {
                                switch (tempSpell.spellType)
                                {
                                    case SpellType.wall:
                                        Debug.Log("hit wall collision");
                                        NetworkedWallComponent tempWall = (NetworkedWallComponent)tempSpell;
                                        tempWall.DoSpellImpact();
                                        break;
                                    case SpellType.projectile:
                                        NetworkedProjectileComponent projectile = (NetworkedProjectileComponent)tempSpell;
                                        projectile.DoImpact();
                                        Debug.Log("Wrong fireball poooooooooooooooof");
                                        break;
                                }

                                switch (spell.spellType)
                                {
                                    case SpellType.wall:
                                        break;
                                    case SpellType.projectile:
                                        Debug.Log("Fireball poof");
                                        NetworkedProjectileComponent projectileSelf = (NetworkedProjectileComponent)spell;
                                        projectileSelf.DoImpact();
                                        break;
                                }
                            }
                            else if (tempSpellOwner != null && spellOwner != null && tempSpellOwner == spellOwner)
                            {
                                if (tempSpell.spellType == SpellType.wall)
                                {
                                    NetworkedWallComponent tempWall = (NetworkedWallComponent)(tempSpell);
                                    if (tempWall.elementtype == elementType.FIRE)
                                    {
                                        NetworkedProjectileComponent fireball = (NetworkedProjectileComponent)spell;
                                        fireball.SetWentThroughWall(true, temp.elementtype);
                                    }
                                }
                            }
                            break;

                        case elementType.EARTH:
                            break;
                        case elementType.WIND:
                            break;
                    }
                }
            }
            //(Origonal location)SpellComponent tempSpell = other.transform.gameObject.GetComponent<SpellComponent>();
            if (tempSpell)
            {
                //Debug.Log("Have Spell");
                //GameObject tempSpellOwner = tempSpell.getOwner();
                //GameObject spellOwner = spell.getOwner();

                if (tempSpellOwner != null && spellOwner != null && tempSpellOwner != spellOwner)
                {
                    switch (tempSpell.spellType)
                    {
                        case SpellType.wall:
                            Debug.Log("hit wall collision");
                            NetworkedWallComponent tempWall = (NetworkedWallComponent)tempSpell;
                            tempWall.DoSpellImpact();
                            break;
                        case SpellType.projectile:
                            NetworkedProjectileComponent fireball = (NetworkedProjectileComponent)tempSpell;
                            fireball.DoImpact();
                            Debug.Log("Wrong fireball poooooooooooooooof");
                            break;
                    }

                    switch (spell.spellType)
                    {
                        case SpellType.wall:
                            break;
                        case SpellType.projectile:
                            Debug.Log("Fireball poof");
                            NetworkedProjectileComponent fireballSelf = (NetworkedProjectileComponent)spell;
                            fireballSelf.DoImpact();
                            break;
                    }
                }
                else if (tempSpellOwner != null && spellOwner != null && tempSpellOwner == spellOwner)
                {
                    if(tempSpell.spellType == SpellType.wall)
                    {
                        NetworkedProjectileComponent fireball = (NetworkedProjectileComponent)spell;
                        //fireball.SetWentThroughWall(true);
                    }
                }

            }

            //if the spell hits a player
            else if (other.CompareTag("Player"))
            {
                switch (spell.spellType)
                {
                    case SpellType.wall:
                        break;
                    case SpellType.projectile:
                        NetworkedProjectileComponent temp = (NetworkedProjectileComponent)spell;

                        NetworkObject networkObject = other.GetComponent<NetworkObject>();

                        //change damage based on spell type
                        switch (temp.elementtype)
                        {
                            case elementType.FIRE:
                                matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 20);
                                break;

                            case elementType.WATER:
                                matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 20);
                                break;

                            case elementType.WIND:
                                matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 20);
                                break;

                            case elementType.EARTH:
                                matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 20);
                                break;
                        }

                        Debug.Log("Hit Player" + networkObject.OwnerClientId);

                        //delete the projectile
                        NetworkedProjectileComponent projectileSelf = (NetworkedProjectileComponent)spell;
                        projectileSelf.DoImpact();

                        break;
                }
            }
        }
    }
}
