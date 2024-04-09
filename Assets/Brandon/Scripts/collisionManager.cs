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


    GameObject tempSpellOwner;

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
                //Debug.LogError("MatchManager component not found on the MatchManager GameObject.");
            }
        }
        else
        {
            //Debug.LogError("MatchManager GameObject not found in the hierarchy. Make sure it's active and tagged appropriately.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (isSpell)
        {
            SpellManager spellManager = spell.getOwner().GetComponent<SpellManager>();
            SpellComponent tempSpell = other.transform.gameObject.GetComponent<SpellComponent>();

            if (tempSpell == null && !other.CompareTag("PlayerBody")) return;

            //Debug.Log(tempSpell);
            //Debug.Log(other.gameObject.tag + "spell");

            if(tempSpell != null)
            {
                tempSpellOwner = tempSpell.getOwner();
            }

            GameObject spellOwner = spell.getOwner();

            if (other.CompareTag("PlayerBody"))
            {
                //Debug.Log("hit player");
                switch (spell.spellType)
                {
                    case SpellType.wall:
                        break;
                    case SpellType.projectile:
                        NetworkedProjectileComponent temp = (NetworkedProjectileComponent)spell;

                        NetworkObject networkObject = other.GetComponent<NetworkObject>();

                        //Debug.Log("At swtich");
                        //change damage based on spell type
                        switch (temp.elementtype)
                        {
                            case elementType.FIRE:
                                Debug.Log("Player hit with Fireball");
                                matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 20);
                                break;

                            case elementType.WATER:
                                Debug.Log("Player hit with Water Shot");
                                matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 20);
                                break;

                            case elementType.WIND:
                                Debug.Log("Player hit with Wind Blast");
                                matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 20);
                                break;

                            case elementType.EARTH:
                                Debug.Log("Player hit with Earth Spear");
                                switch (spellManager.earthShotCount)
                                {
                                    case 0:
                                        matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 15);
                                        break;
                                    case 1:
                                        matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 20);
                                        break;
                                    case 2:
                                        matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 25);
                                        break;
                                    case 3:
                                        matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 30);
                                        break;
                                    case 4:
                                        matchManager.UpdatePlayerHealthServerRpc(networkObject.OwnerClientId, 35);
                                        break;
                                }
                                break;
                        }

                        //Debug.Log("Hit Player" + networkObject.OwnerClientId);

                        //delete the projectile
                        NetworkedProjectileComponent projectileSelf = (NetworkedProjectileComponent)spell;
                        projectileSelf.DoImpact();

                        break;
                }
            }

            else if(spell.spellType == SpellType.projectile)
            {
                //Debug.Log("wrong place");
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
                                        //Debug.Log("hit wall collision");
                                        NetworkedWallComponent tempWall = (NetworkedWallComponent)tempSpell;
                                        tempWall.DoSpellImpact();
                                        break;
                                    case SpellType.projectile:
                                        NetworkedProjectileComponent projectile = (NetworkedProjectileComponent)tempSpell;
                                        projectile.DoImpact();
                                        //Debug.Log("Wrong fireball poooooooooooooooof");
                                        break;
                                }

                                switch (spell.spellType)
                                {
                                    case SpellType.wall:
                                        break;
                                    case SpellType.projectile:
                                        //Debug.Log("Fireball poof");
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
                                        //Debug.Log("hit wall collision");
                                        NetworkedWallComponent tempWall = (NetworkedWallComponent)tempSpell;
                                        tempWall.DoSpellImpact();
                                        break;
                                    case SpellType.projectile:
                                        NetworkedProjectileComponent projectile = (NetworkedProjectileComponent)tempSpell;
                                        projectile.DoImpact();
                                        //Debug.Log("Wrong fireball poooooooooooooooof");
                                        break;
                                }

                                switch (spell.spellType)
                                {
                                    case SpellType.wall:
                                        break;
                                    case SpellType.projectile:
                                        //Debug.Log("Fireball poof");
                                        NetworkedProjectileComponent projectileSelf = (NetworkedProjectileComponent)spell;
                                        projectileSelf.DoImpact();
                                        break;
                                }
                            }
                            else if (tempSpellOwner != null && spellOwner != null && tempSpellOwner == spellOwner)
                            {
                                //Debug.Log("Outside if");
                                //Debug.Log(tempSpell);
                                if (spell.spellType != SpellType.wall)
                                {
                                    if (tempSpell.spellType == SpellType.wall)
                                    {
                                        //Debug.Log("Inside if");

                                        NetworkedWallComponent tempWall = (NetworkedWallComponent)(tempSpell);
                                        if (tempWall.elementtype == elementType.WATER)
                                        {
                                            NetworkedProjectileComponent fireball = (NetworkedProjectileComponent)spell;
                                            fireball.SetWentThroughWall(true, temp.elementtype);
                                        }
                                    }
                                }
                                
                            }
                            break;

                        case elementType.EARTH:
                            if (tempSpellOwner != null && spellOwner != null && tempSpellOwner != spellOwner)
                            {
                                switch (tempSpell.spellType)
                                {
                                    case SpellType.wall:
                                        //Debug.Log("hit wall collision");
                                        NetworkedWallComponent tempWall = (NetworkedWallComponent)tempSpell;
                                        if (spellManager.earthShotCount == 4)
                                        {
                                            tempWall.DestroyWall();
                                        }
                                        else
                                        {
                                            tempWall.DoSpellImpact();
                                        }
                                        break;

                                    case SpellType.projectile:
                                        NetworkedProjectileComponent projectile = (NetworkedProjectileComponent)tempSpell;
                                        projectile.DoImpact();
                                        //Debug.Log("Wrong fireball poooooooooooooooof");
                                        break;
                                }

                                switch (spell.spellType)
                                {
                                    case SpellType.wall:
                                        break;
                                    case SpellType.projectile:
                                        //Debug.Log("Fireball poof");
                                        NetworkedProjectileComponent projectileSelf = (NetworkedProjectileComponent)spell;
                                        if (spellManager.earthShotCount == 4) { }
                                        else
                                        {
                                            projectileSelf.DoImpact();
                                        }
                                        break;
                                }
                            }
                            break;
                        case elementType.WIND:
                            if (tempSpellOwner != null && spellOwner != null && tempSpellOwner != spellOwner)
                            {
                                switch (tempSpell.spellType)
                                {
                                    case SpellType.wall:
                                        //Debug.Log("hit wall collision");
                                        NetworkedWallComponent tempWall = (NetworkedWallComponent)tempSpell;
                                        tempWall.DoSpellImpact();
                                        break;
                                    case SpellType.projectile:
                                        NetworkedProjectileComponent projectile = (NetworkedProjectileComponent)tempSpell;
                                        projectile.DoImpact();
                                        //Debug.Log("Wrong fireball poooooooooooooooof");
                                        break;
                                }

                                switch (spell.spellType)
                                {
                                    case SpellType.wall:
                                        break;
                                    case SpellType.projectile:
                                        //Debug.Log("Fireball poof");
                                        NetworkedProjectileComponent projectileSelf = (NetworkedProjectileComponent)spell;
                                        projectileSelf.DoImpact();
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
            else if (other.CompareTag("Pillar"))
            {
                NetworkedProjectileComponent temp = (NetworkedProjectileComponent)spell;

                if(temp.elementtype == elementType.EARTH)
                {
                    if(spellManager.earthShotCount == 4)
                    {
                        PillarLogic pillarLogic = other.transform.gameObject.GetComponent<PillarLogic>();
                        pillarLogic.DestroyPillarClientRpc();
                    }
                }
            }
            //(Origonal location)SpellComponent tempSpell = other.transform.gameObject.GetComponent<SpellComponent>();
            /*if (tempSpell)
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

            }*/

            //if the spell hits a player

        }
    }
}
