//Created by Mason Smith using MiVRy Gesture Recognition Tutorial. Reads player input gestures, and completes specific tasks according to the value of the gesture.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEventProcessor : MonoBehaviour
{
    public TeleportationManager teleportationManager;
    private bool isTeleportGestureRecognized = false;
    private bool isWeaponSpawned = false;
    private bool isTouchingElement = false;
    public SpellManager spellmanager;
    public UnNetworkedSpellManager unNetworkSpellmanager;
    public UnNetworkPlayer unNetworkPlayer;
    //public GestureRecognition gr = newGestureRecognition();
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
            Debug.Log("Gesture");
            /*if (gestureCompletionData.gestureName == "Left Wall") {
                
                Debug.Log("Left Wall Gesture Successfully Casted");
                if (unNetworkSpellmanager && unNetworkPlayer.isTutorial) unNetworkSpellmanager.SpawnWall(true);
                if (spellmanager && !unNetworkPlayer.isTutorial) spellmanager.fireLeftWall();
            }
            //Casts Right Hand Wall Spell
            if (gestureCompletionData.gestureName == "Right Wall")
            {
                if (unNetworkSpellmanager && unNetworkPlayer.isTutorial) unNetworkSpellmanager.SpawnWall(false);
                Debug.Log("Right Wall Gesture Successfully Casted");
                if (spellmanager && !unNetworkPlayer.isTutorial) spellmanager.fireRightWall();
            }
            //Casts Left Hand Cast Spell
            if (gestureCompletionData.gestureName == "Left Cast")
            {

                if (unNetworkSpellmanager && unNetworkPlayer.isTutorial) unNetworkSpellmanager.SpawnProjectile(true);
                Debug.Log("Left Cast Gesture Successfully Casted");
                if (spellmanager && !unNetworkPlayer.isTutorial) spellmanager.fireLeftProjectile();
            }
            //Casts Right Hand Cast Spell
            if (gestureCompletionData.gestureName == "Right Cast")
            {
                if (unNetworkSpellmanager && unNetworkPlayer.isTutorial) unNetworkSpellmanager.SpawnProjectile(false);
                Debug.Log("Right Cast Gesture Successfully Casted");
                if (spellmanager && !unNetworkPlayer.isTutorial) spellmanager.fireRightProjectile();
            }*/
            //Casts Bow Spawn
            if (gestureCompletionData.gestureName == "Left Bow Spawn" || gestureCompletionData.gestureName == "Right Bow Spawn")
            {
                Debug.Log("Bow Successfully Spawned");
                isWeaponSpawned = true;
            }

            //Casts Arrow Draw
            if(isWeaponSpawned = true)
            {
                if (gestureCompletionData.gestureName == "Left Arrow Draw" || gestureCompletionData.gestureName == "Right Arrow Draw")
                {
                    Debug.Log("Arrow Successfully Fired");
                }
            }
            else if(isWeaponSpawned = false)
            {
                if (gestureCompletionData.gestureName == "Left Arrow Draw" || gestureCompletionData.gestureName == "Right Arrow Draw")
                {
                    Debug.Log("Cannot Fire Arrow, Bow Has Not Been Spawned");
                }
            }

            //Casts Fire Wall
            if (gestureCompletionData.gestureName == "Left Fire Wall" || gestureCompletionData.gestureName == "Right Fire Wall")
            {
                Debug.Log("Fire Wall Successfully Casted");
            }

            //Casts Element Spawn (for Earth, Water, and Air)
            if (gestureCompletionData.gestureName == "Left Element Spawn" || gestureCompletionData.gestureName == "Right Element Spawn")
            {
                Debug.Log("Element Successfully Spawned");
            }

            //Casts Hit (for Water and Earth)
            if (gestureCompletionData.gestureName == "Left Hit" || gestureCompletionData.gestureName == "Right Hit")
            {
                Debug.Log("Hit Successfully Casted");
            }

            //Casts Rock Wall
            if (gestureCompletionData.gestureName == "Left Rock Wall" || gestureCompletionData.gestureName == "Right Rock Wall")
            {
                Debug.Log("Rock Wall Successfully Spawned");
            }

            //Casts Reload
            if (gestureCompletionData.gestureName == "Left Reload" || gestureCompletionData.gestureName == "Right Reload")
            {
                Debug.Log("Reload Successfully Casted");
            }

            //Casts Air Shield
            if (gestureCompletionData.gestureName == "Air Shield")
            {
                Debug.Log("Air Shield Successfully Spawned");
            }

            //Casts Water Shield
            if (gestureCompletionData.gestureName == "Left Water Shield" || gestureCompletionData.gestureName == "Right Water Shield")
            {
                Debug.Log("Water Shield Successfully Spawned");
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

    //Checks if Bow has been spawned
    public bool IsWeaponSpawned()
    {
        return isWeaponSpawned;
    }

    //Checks if Player is touching element
    public bool IsTouchingElement()
    {
        return isTouchingElement;
    }
}