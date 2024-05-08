//Author: Pearson Lawrence
//Purpose: Destroy an object if a bool is set to true.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyManager : MonoBehaviour
{
    public bool destroy = false;
    public void destroyThis()
    {
        destroy = true;
    }
    private void Update()
    {
        if (destroy) Destroy(gameObject);
    }
}
