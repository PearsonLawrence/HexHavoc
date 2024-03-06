//Created by Mason Smith. Used to activate teleportation during two-handed Teleport gesture.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public GameObject teleportationRayPrefab;

    private GameObject teleportationRay;
    private Transform leftHand;
    private Transform rightHand;
    private GestureEventProcessor gestureEventProcessor;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.Find("Left Controller").transform;
        rightHand = GameObject.Find("Right Controller").transform;
        gestureEventProcessor = GetComponent<GestureEventProcessor>();
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

        //Uses direction vector from left hand to right hand as the ray direction
        Vector3 rayDirection = (rightHand.up + leftHand.up).normalized;

        //Calculates ray's starting position at center of the hands during the gesture
        Vector3 rayStartPosition = (rightHand.position + leftHand.position) / 2f;

        //Creates ray from left hand position
        Ray ray = new Ray(leftHand.position, rayDirection);

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