using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class WallSpell : SpellComponent
{
    public SpellManager parent;
    public float lifeTime = 20f;
    public int spellsTanked;

    void Start()
    {
        spellsTanked = 0;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DestroyAfterDelay());

        if(spellsTanked >= 2)
        {
            parent.DestroyServerRpc();
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(lifeTime);

        if (parent != null && parent.NetworkObject != null && parent.NetworkObject.IsSpawned)
        {
            parent.DestroyServerRpc();
        }
    }

    public void DoSpellImpact()
    {
        spellsTanked++; 
    }
}
