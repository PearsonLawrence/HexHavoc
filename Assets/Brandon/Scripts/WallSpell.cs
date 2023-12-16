//Author: Brandon(Ri) Yu
//Purpose: This script is attached to the wall gameobject and gives it its logic. It creates a timer til it is
//destroyed along with keeling track of how many spells it has blocked.

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
    public GameObject explodePrefab;

    void Start()
    {
        spellsTanked = 0;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if(lifeTime < 0)
        {
            GameObject temp = Instantiate(explodePrefab, transform.position, Quaternion.identity);

            Destroy(temp, 3);
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }

    public void DoSpellImpact()
    {
        spellsTanked++;

        if (spellsTanked > 2)
        {
            GameObject temp = Instantiate(explodePrefab, transform.position, Quaternion.identity);

            Destroy(temp, 3);
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);

        }
    }
}
