//Author: Brandon
//Purpose: This is just an enum that can be attached to spells so we know what element the spell is

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellClassifier : MonoBehaviour
{
    // Start is called before the first frame update

    public enum ElementType
    {
        EARTH,
        WIND,
        WATER,
        FIRE
    }

    public ElementType element;
}
