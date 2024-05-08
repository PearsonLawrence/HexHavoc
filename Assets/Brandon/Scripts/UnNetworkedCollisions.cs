//Author: Brandon Yu
//Purpose: This script is used to handle the collisions of unnetworked spells. Behaves the same as the regular collision manager example on local clients

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnNetworkedCollisions : MonoBehaviour
{
    [SerializeField] private UnNetworkedSpellComponent spell;
    [SerializeField] private bool isSpell = false;

    GameObject tempSpellOwner;

    public UnNetworkedSpellComponent getSpell()
    {
        return spell;
    }

    public void setSpell(UnNetworkedSpellComponent temp)
    {
        spell = temp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSpell)
        {
            UnNetworkedSpellComponent tempSpell = other.transform.gameObject.GetComponent<UnNetworkedSpellComponent>();

            if (tempSpell == null && !other.CompareTag("Player")) return;

            if (tempSpell != null)
            {
                tempSpellOwner = tempSpell.getOwner();
            }

            GameObject spellOwner = spell.getOwner();

            if (other.CompareTag("Player"))
            {

            }

            else if (spell.spellType == SpellType.projectile)
            {
                //Debug.Log("wrong place");
                ProjectileComponent temp = (ProjectileComponent)spell;
                if (temp != null)
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
                                        WallComponent tempWall = (WallComponent)tempSpell;
                                        tempWall.DoDamage();
                                        break;
                                    case SpellType.projectile:
                                        ProjectileComponent projectile = (ProjectileComponent)tempSpell;
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
                                        ProjectileComponent projectileSelf = (ProjectileComponent)spell;
                                        projectileSelf.DoImpact();
                                        break;
                                }
                            }
                            else if (tempSpellOwner != null && spellOwner != null && tempSpellOwner == spellOwner)
                            {
                                if (tempSpell.spellType == SpellType.wall)
                                {
                                    WallComponent tempWall = (WallComponent)(tempSpell);
                                    if (tempWall.elementtype == elementType.FIRE)
                                    {
                                        ProjectileComponent fireball = (ProjectileComponent)spell;
                                        //fireball.SetWentThroughWall(true, temp.elementtype);
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
                                        WallComponent tempWall = (WallComponent)tempSpell;
                                        tempWall.DoDamage();
                                        break;
                                    case SpellType.projectile:
                                        ProjectileComponent projectile = (ProjectileComponent)tempSpell;
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
                                        ProjectileComponent projectileSelf = (ProjectileComponent)spell;
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

                                        WallComponent tempWall = (WallComponent)(tempSpell);
                                        if (tempWall.elementtype == elementType.WATER)
                                        {
                                            ProjectileComponent fireball = (ProjectileComponent)spell;
                                            //fireball.SetWentThroughWall(true, temp.elementtype);
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
                                        WallComponent tempWall = (WallComponent)tempSpell;
                                        tempWall.DoDamage();
                                        break;
                                    case SpellType.projectile:
                                        ProjectileComponent projectile = (ProjectileComponent)tempSpell;
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
                                        ProjectileComponent projectileSelf = (ProjectileComponent)spell;
                                        projectileSelf.DoImpact();
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
                                        WallComponent tempWall = (WallComponent)tempSpell;
                                        tempWall.DoDamage();
                                        break;
                                    case SpellType.projectile:
                                        ProjectileComponent projectile = (ProjectileComponent)tempSpell;
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
                                        ProjectileComponent projectileSelf = (ProjectileComponent)spell;
                                        projectileSelf.DoImpact();
                                        break;
                                }
                            }
                            break;

                    }
                }
            }
        }
    }
}
