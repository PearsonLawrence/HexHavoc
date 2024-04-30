using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public enum tronDirection
{
    TOSTART,
    TOEND
}
public class TronMove : NetworkBehaviour
{
    // Start is called before the first frame update

    private bool isMoving = false;
    public bool moveTron = false;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    private float moveDuration = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(moveTron)
        {
            moveTron = false;
        }
    }

    [ClientRpc]
    public void MoveJumboTronClientRpc(tronDirection direction)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine(direction));
        }
    }

    private IEnumerator MoveCoroutine(tronDirection direction)
    {
        isMoving = true;
        float elapsedTime = 0f;

        Vector3 startPos = startPosition.position;
        Vector3 endPos = endPosition.position;


        if (direction == tronDirection.TOSTART)
        {
            startPos = endPosition.position;
            endPos = startPosition.position;
        }
        else
        {
            startPos = startPosition.position;
            endPos = endPosition.position;
        }

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }
}
