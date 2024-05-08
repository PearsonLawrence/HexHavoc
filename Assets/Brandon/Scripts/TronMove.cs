//Author: Brandon Yu
//Purpose: This script is used to lower the jumbotron down when the match starts

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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
            MoveJumboTronClientRpc();
        }
    }

    [ClientRpc]
    public void MoveJumboTronClientRpc()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine());
        }
    }

    private IEnumerator MoveCoroutine()
    {
        isMoving = true;
        float elapsedTime = 0f;

        Vector3 startPos = startPosition.position;
        Vector3 endPos = endPosition.position;


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
