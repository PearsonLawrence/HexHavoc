//Created by Mason Smith. Used to activate teleportation during two-handed Teleport gesture.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public GameObject teleportationRayPrefab;

    private GameObject teleportationRay;
    private Transform playerHead;
    private GestureEventProcessor gestureEventProcessor;

    // Start is called before the first frame update
    void Start()
    {
        gestureEventProcessor = GetComponent<GestureEventProcessor>();
        playerHead = Camera.main.transform;

        if (playerHead == null)
        {
            Debug.Log("Player Camera Reference not set in TeleportationManager");
        }
    }

    void Update()
    {
        if (gestureEventProcessor.IsTeleportGestureRecognized())
        {
            Teleport();
            gestureEventProcessor.ResetTeleportGesture();
        }
    }

    //Called when Teleport gesture is recognized
    public void Teleport()
    {
        RaycastHit hit;
        Vector3 teleportDestination;

        //Uses direction vector from where player is currently facing
        Vector3 rayDirection = playerHead.forward;

        //Creates ray from left hand position
        Ray ray = new Ray(playerHead.position, rayDirection);

        //Calculates ray's starting position at player's head during the gesture
        Vector3 rayStartPosition = playerHead.position;

        Debug.DrawRay(rayStartPosition, rayDirection*1000, Color.red, 30);

        if (Physics.Raycast(rayStartPosition, rayDirection, out hit, 1000))
        {
            //Set teleport destination to hit point
            teleportDestination = hit.point;

            //Instantiate teleportation ray prefab between two hands
            teleportationRay = Instantiate(teleportationRayPrefab, teleportDestination, Quaternion.identity);

            Debug.Log("Player teleported to " + teleportDestination);
        }
        else
        {
            Debug.Log("No teleport destination found.");
        }
        
    }
}