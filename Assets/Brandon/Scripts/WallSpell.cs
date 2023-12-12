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

            GetComponent<NetworkObject>().Despawn();
            Destroy(temp, 3);
            Destroy(gameObject);
        }
    }

    public void DoSpellImpact()
    {
        spellsTanked++;

        if (spellsTanked > 2)
        {
            GameObject temp = Instantiate(explodePrefab, transform.position, Quaternion.identity);
            
            GetComponent<NetworkObject>().Despawn();
            Destroy(temp, 3);
            Destroy(gameObject);

        }
    }
}
