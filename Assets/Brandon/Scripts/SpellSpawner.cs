using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour
{
    public Transform LeftHandPos, RightHandPos;
    public Transform windVFX;
    public Transform fireVFX;
    public Transform waterVFX;
    public Transform earthVFX;

    private HandInteractableComponent hand;
    private GestureEventProcessor gesture;

    public SpellManager spellManager;

    private int castedWith;

    public bool handIn = false;


    Transform spawnedSpell;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Q))
        {
            spawnWindElement();
        }

        /*if (hand.isHolding && handIn)
        {
            if(castedWith == 1)
            {
                windVFX.position = LeftHandPos.position;
            }
            if(castedWith == 2)
            {
                windVFX.position = RightHandPos.position;
            }
        }*/
    }

    public void spawnWindElement()
    {
        if (spawnedSpell == null)
        {
            float spawnDistance = 1f;

            Vector3 endPosition = new Vector3(0,0,0);
            Vector3 startPosition = endPosition - new Vector3(0, 5, 0);
            switch(spellManager.elementSpeicalization)
            {
                case elementType.FIRE:
                    spawnedSpell = Instantiate(fireVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WATER:
                    spawnedSpell = Instantiate(waterVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.EARTH:
                    spawnedSpell = Instantiate(earthVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WIND:
                    spawnedSpell = Instantiate(windVFX, startPosition, Quaternion.identity);
                    break;
            }

            StartCoroutine(move(spawnedSpell, startPosition, endPosition));
        }
    }

    public void SpawnElementLeft()
    {
        if(spawnedSpell == null)
        {
            float spawnDistance = 1f;

            Vector3 endPosition = LeftHandPos.position + LeftHandPos.forward * spawnDistance;
            Vector3 startPosition = endPosition - new Vector3(0, 5, 0);

            switch (spellManager.elementSpeicalization)
            {
                case elementType.FIRE:
                    spawnedSpell = Instantiate(fireVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WATER:
                    spawnedSpell = Instantiate(waterVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.EARTH:
                    spawnedSpell = Instantiate(earthVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WIND:
                    spawnedSpell = Instantiate(windVFX, startPosition, Quaternion.identity);
                    break;
            }
            StartCoroutine(move(spawnedSpell, startPosition, endPosition));

            castedWith = 1;
        }
        
    }

    public void SpawnElementRight ()
    {
        if (spawnedSpell == null)
        {
            float spawnDistance = 1f;

            Vector3 endPosition = RightHandPos.position + RightHandPos.forward * spawnDistance;
            Vector3 startPosition = endPosition - new Vector3(0, 5, 0);

            switch (spellManager.elementSpeicalization)
            {
                case elementType.FIRE:
                    spawnedSpell = Instantiate(fireVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WATER:
                    spawnedSpell = Instantiate(waterVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.EARTH:
                    spawnedSpell = Instantiate(earthVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WIND:
                    spawnedSpell = Instantiate(windVFX, startPosition, Quaternion.identity);
                    break;
            }
            StartCoroutine(move(spawnedSpell, startPosition, endPosition));

            castedWith = 2;
        }
    }

    private IEnumerator move(Transform movePrefab, Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 1)
        {
            float t = elapsedTime / 1;
            movePrefab.position = Vector3.Lerp(start, end, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movePrefab.position = end;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("playerHand"))
        {
            handIn = true;

            hand = other.GetComponent<HandInteractableComponent>();
            gesture = hand.parentObj.GetComponent<GestureEventProcessor>();

            gesture.isTouchingElement = true;
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (other.CompareTag("PlayerHand"))
        {
            handIn = false;
        }*/
    }

    public void disablePrefab()
    {
        spawnedSpell = null;
    }
}
