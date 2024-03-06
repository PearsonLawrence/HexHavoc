//Created by Mason Smith using MiVRy Gesture Recognition Tutorial. Reads player input gestures, and completes specific tasks according to the value of the gesture.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEventProcessor : MonoBehaviour
{
    private TeleportationManager teleportationManager;

    // Start is called before the first frame update
    void Start()
    {
        teleportationManager = GetComponent<TeleportationManager>();
        if (teleportationManager == null)
        {
            Debug.LogError("TeleportationManager is not found on the same GameObject.");
        }
    }

    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {
        //Negative values are errors, so this throws an error if the gesture was not read properly
        if (gestureCompletionData.gestureID < 0) {
            string errorMessage = GestureRecognition.getErrorMessage(gestureCompletionData.gestureID);
            return;
        }
        //Specifies how similar gestures made in game must be to pre-recorded gesture samples
        if (gestureCompletionData.similarity >= 0.5) {
            //Casts Left Hand Wall Spell
            if (gestureCompletionData.gestureName == "Left Wall") {
                Debug.Log("Left Wall Gesture Successfully Casted");
            }
            //Casts Right Hand Wall Spell
            if (gestureCompletionData.gestureName == "Right Wall")
            {
                Debug.Log("Right Wall Gesture Successfully Casted");
            }
            //Casts Left Hand Cast Spell
            if (gestureCompletionData.gestureName == "Left Cast")
            {
                Debug.Log("Left Cast Gesture Successfully Casted");
            }
            //Casts Right Hand Cast Spell
            if (gestureCompletionData.gestureName == "Right Cast")
            {
                Debug.Log("Right Cast Gesture Successfully Casted");
            }
            //Casts Teleport
            if (gestureCompletionData.gestureName == "Teleport")
            {
                Debug.Log("Teleport Gesture Successfully Casted");
                teleportationManager.SpawnTeleportationRay();
                teleportationManager.Teleport();
            }
        }

        else {
            Debug.Log("Gesture Failed to Cast");
        }
    }
}