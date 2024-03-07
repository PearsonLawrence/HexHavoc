//Created by Mason Smith. Used to activate teleportation during two-handed Teleport gesture.
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public GameObject teleportationRayPrefab;
    public XROrigin XR;

    private GameObject teleportationRay;
    private Transform playerHead;
    private GestureEventProcessor gestureEventProcessor;

    // Start is called before the first frame update
    void Start()
    {
        //XR = GetComponent<XROrigin>();
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

        //Uses direction vector from where player is currently facing
        Vector3 rayDirection = playerHead.forward;

        //Creates ray from left hand position
        Ray ray = new Ray(playerHead.position, rayDirection);

        //Calculates ray's starting position at player's head during the gesture
        Vector3 rayStartPosition = playerHead.position;

        Debug.DrawRay(rayStartPosition, rayDirection*1000, Color.red, 30);

        if (Physics.Raycast(rayStartPosition, rayDirection, out hit, 1000))
        {
            Debug.Log("Raycast has been Casted");
            //Checks if hit object has Pillar tag
            if (hit.collider.gameObject.CompareTag("Pillar"))
            {
                //Get pillar logic script
                PillarLogic pillarLogic = hit.collider.gameObject.GetComponent<PillarLogic>();
                Debug.Log(pillarLogic);

                //Check if script was found
                if (pillarLogic != null)
                {
                    XR.Origin.transform.position = pillarLogic.playerPoint.transform.position;
                    Debug.Log("Pillar Logic is NOT Null");
                    Debug.Log("Player teleported to " + pillarLogic.playerPoint.transform.position + " : Current position: " + this.gameObject.transform.position);
                }
            }
        }
        else
        {
            Debug.Log("No teleport destination found.");
        }        
    }
}