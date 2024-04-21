using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour
{
    public Transform LeftHandPos, RightHandPos;
    public GameObject windVFX;
    public GameObject fireVFX;
    public GameObject waterVFX;
    public GameObject earthVFX;

    private HandInteractableComponent hand;
    public GestureEventProcessor gesture;

    public UnNetworkedSpellManager spellManager;

    private int castedWith;

    public bool handIn = false;

    public GameObject currentSpawnedElementLeft, currentSpawnedElementRight;
    Transform spawnedSpell;
    public Vector3 offset = new Vector3(0,5,0);
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DeSpawnElement()
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
        /*if (spawnedSpell == null)
        {
            float spawnDistance = 1f;

            Vector3 endPosition = LeftHandPos.transform.position;
            Vector3 startPosition = endPosition - offset;
            switch(spellManager.elementSpeicalization)
            {
                case elementType.FIRE:
                    currentSpawnedElement = Instantiate(fireVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WATER:
                    currentSpawnedElement = Instantiate(waterVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.EARTH:
                    currentSpawnedElement = Instantiate(earthVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WIND:
                    currentSpawnedElement = Instantiate(windVFX, startPosition, Quaternion.identity);
                    break;
            }
            StartCoroutine(move(currentSpawnedElement.transform, startPosition, endPosition));
        }*/
    }

    public void SpawnElementLeft()
    {
        if(spawnedSpell == null)
        {
            float spawnDistance = 1f;

            Vector3 endPosition = LeftHandPos.position;
            Vector3 startPosition = endPosition - offset;
            gesture.isElementSpawned = true;

            switch (spellManager.elementSpeicalization)
            {
                case elementType.FIRE:
                    currentSpawnedElementLeft = Instantiate(fireVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WATER:
                    currentSpawnedElementLeft = Instantiate(waterVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.EARTH:
                    currentSpawnedElementLeft = Instantiate(earthVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WIND:
                    currentSpawnedElementLeft = Instantiate(windVFX, startPosition, Quaternion.identity);
                    break;
            }
            gesture.CurrentElement = currentSpawnedElementLeft.GetComponent<DestroyManager>();
            StartCoroutine(move(currentSpawnedElementLeft.transform, startPosition, endPosition));

            castedWith = 1;
        }
        
    }

    public void SpawnElementRight ()
    {
        if (spawnedSpell == null)
        {
            float spawnDistance = 1f;

            Vector3 endPosition = RightHandPos.position;
            Vector3 startPosition = endPosition - offset;
            gesture.isElementSpawned = true;
            switch (spellManager.elementSpeicalization)
            {
                case elementType.FIRE:
                    currentSpawnedElementRight = Instantiate(fireVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WATER:
                    currentSpawnedElementRight = Instantiate(waterVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.EARTH:
                    currentSpawnedElementRight = Instantiate(earthVFX, startPosition, Quaternion.identity);
                    break;
                case elementType.WIND:
                    currentSpawnedElementRight = Instantiate(windVFX, startPosition, Quaternion.identity);
                    break;
            }

            gesture.CurrentElement = currentSpawnedElementRight.GetComponent<DestroyManager>();
            StartCoroutine(move(currentSpawnedElementRight.transform, startPosition, endPosition));

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
