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
    public int spellsTanked; //Can take up to 3 spells before destroyed
    [SerializeField] private int maxSpellsToAbsorb = 3;
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

    //instantiate destroy particle effect and despawn from network
    public void DoSpellImpact()
    {
        spellsTanked++;
        //TODO: Only call this function if 1. it does not collide with a spell from the player
        if (spellsTanked > maxSpellsToAbsorb)
        {
            GameObject temp = Instantiate(explodePrefab, transform.position, Quaternion.identity);

            Destroy(temp, 3);
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);

        }
    }
}
