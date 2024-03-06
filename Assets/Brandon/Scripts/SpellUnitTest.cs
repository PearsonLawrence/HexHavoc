//Author: Brandon (Ri) Yu
//Purpose: Unit test to ensure that spells spawn correctly when passed in with prefab

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellUnitTest : MonoBehaviour
{
    // Start is called before the first frame update
    public SpellManager spellManager;
    public float spellDelay;

    [SerializeField] private Transform fireballPrefab;
    [SerializeField] private Transform windBlastPrefab;
    [SerializeField] private Transform waterShotPrefab;
    [SerializeField] private Transform earthSpearPrefab;
    void Start()
    {
        Debug.Log("Spawn Started");
        StartCoroutine(TestSpawnSpells(spellDelay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TestSpawnSpells(float delay)
    {
        yield return new WaitForSeconds(delay);
        spellManager.desiredProjectile = fireballPrefab.transform;
        spellManager.SpawnProjectile();

        Debug.Log("Spawned Fireball");
        //collision text and damage will be printed from collision manager

        yield return new WaitForSeconds(delay);
        spellManager.desiredProjectile = windBlastPrefab.transform;
        spellManager.SpawnProjectile();

        Debug.Log("Spawned Wind Blast");
        //collision text and damage will be printed from collision manager

        yield return new WaitForSeconds(delay);
        spellManager.desiredProjectile = waterShotPrefab.transform;
        spellManager.SpawnProjectile();

        Debug.Log("Spawned Water Shot");
        //collision text and damage will be printed from collision manager

        yield return new WaitForSeconds(delay);
        spellManager.desiredProjectile = earthSpearPrefab.transform;
        spellManager.SpawnProjectile();

        Debug.Log("Spawned Earth Spear");
        //collision text and damage will be printed from collision manager
    }
}
