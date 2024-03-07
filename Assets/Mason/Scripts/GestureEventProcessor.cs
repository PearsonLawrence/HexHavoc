//Created by Mason Smith using MiVRy Gesture Recognition Tutorial. Reads player input gestures, and completes specific tasks according to the value of the gesture.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEventProcessor : MonoBehaviour
{
    public TeleportationManager teleportationManager;
    private bool isTeleportGestureRecognized = false;
    public SpellManager spellmanager;
    // Start is called before the first frame update
    void Start()
    {
        //gr.loadFromFile("StreamingAssets/1and2HandGestures.dat");
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
                if (spellmanager) spellmanager.fireLeftWall();
            }
            //Casts Right Hand Wall Spell
            if (gestureCompletionData.gestureName == "Right Wall")
            {
                Debug.Log("Right Wall Gesture Successfully Casted");
                if (spellmanager) spellmanager.fireRightWall();
            }
            //Casts Left Hand Cast Spell
            if (gestureCompletionData.gestureName == "Left Cast")
            {
                Debug.Log("Left Cast Gesture Successfully Casted");
               if (spellmanager) spellmanager.fireLeftProjectile();
            }
            //Casts Right Hand Cast Spell
            if (gestureCompletionData.gestureName == "Right Cast")
            {
                Debug.Log("Right Cast Gesture Successfully Casted");
                if (spellmanager) spellmanager.fireRightProjectile();
            }
            //Casts Teleport
            if (gestureCompletionData.gestureName == "Teleport")
            {
                Debug.Log("Teleport Gesture Successfully Casted");
                teleportationManager.Teleport();
                isTeleportGestureRecognized = true;
            }
        }
        else {
            Debug.Log("Gesture Failed to Cast");
        }
    }

    //Resets Teleport flag if needed
    public void ResetTeleportGesture()
    {
        isTeleportGestureRecognized = false;
    }

    //Checks if Teleport gesture is recognized
    public bool IsTeleportGestureRecognized()
    { 
        return isTeleportGestureRecognized;
    }
}