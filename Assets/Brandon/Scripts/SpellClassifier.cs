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
