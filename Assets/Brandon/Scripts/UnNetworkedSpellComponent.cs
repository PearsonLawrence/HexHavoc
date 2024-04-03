using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnNetworkedSpellComponent : MonoBehaviour
{
    public SpellType spellType;
    private GameObject owner;

    public GameObject getOwner()
    {
        return owner;
    }

    public void setOwner(GameObject temp)
    {
        owner = temp;
        //Debug.Log("Owner set successfully.");
    }

    public SpellType GetSpellType()
    {
        return spellType;
    }
}
