//Created by Mason Smith using MiVRy Gesture Recognition Tutorial. Reads player input gestures, and completes specific tasks according to the value of the gesture.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEventProcessor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {
        //Negative values are errors, so this throws an error if the gesture was not read properly
        if (gestureCompletionData.gestureID < 0) {
            string errorMessage = GestureRecognition.getErrorMessage(gestureCompletionData.gestureID);
            // ...
            return;
        }
        //Specifies how similar gestures made in game must be to pre-recorded gesture samples
        if (gestureCompletionData.similarity >= 0.3) {
            //Casts Wall Spell
            if (gestureCompletionData.gestureName == "Wall") {
                Debug.Log("Wall Gesture Successfully Casted");
            }
        }
        else {
            Debug.Log("Gesture Failed to Cast");
        }
    }
}
