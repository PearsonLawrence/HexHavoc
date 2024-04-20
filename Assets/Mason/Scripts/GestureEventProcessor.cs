//Created by Mason Smith using MiVRy Gesture Recognition Tutorial. Reads player input gestures, and completes specific tasks according to the value of the gesture.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureEventProcessor : MonoBehaviour
{
    public TeleportationManager teleportationManager;
    private bool isTeleportGestureRecognized = false;
    private bool isLeftWeaponSpawned = false;
    private bool isRightWeaponSpawned = false;
    public bool isTouchingElement = false;
    public bool isRightElementSpawned = false;
    public bool isLeftElementSpawned = false;
    public SpellManager spellmanager;
    public UnNetworkedSpellManager unNetworkSpellmanager;
    public UnNetworkPlayer unNetworkPlayer;
    public int WaterHitCount;
    public int AirBulletAmmo = 4, AirBulletAmmoMax = 4;
    public int ArrowAmmo = 4, ArrowAmmoMax = 4;

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
            if (gestureCompletionData.gestureName == "Right Bow Spawn" && isTouchingElement)
            {
                Debug.Log("Bow Successfully Spawned");
                isRightWeaponSpawned = true;
                isLeftWeaponSpawned = false;
                ArrowAmmo = ArrowAmmoMax;
            }

            if (gestureCompletionData.gestureName == "Left Bow Spawn" && isTouchingElement)
            {
                Debug.Log("Bow Successfully Spawned");
                isLeftWeaponSpawned = true;
                isRightWeaponSpawned = false;
                ArrowAmmo = ArrowAmmoMax;
            }

            //Casts Arrow Draw
            if (isLeftWeaponSpawned == true)
            {
                if (gestureCompletionData.gestureName == "Left Arrow Draw")
                {
                    Debug.Log("Arrow Successfully Fired");
                    ArrowAmmo--;
                    if(ArrowAmmo <= 0)
                    {
                        isLeftWeaponSpawned = false;
                        isRightWeaponSpawned = false;
                    }
                }
            }
            if(isRightWeaponSpawned == true)
            {
                if (gestureCompletionData.gestureName == "Right Arrow Draw")
                {
                    Debug.Log("Arrow Successfully Fired");
                }
                ArrowAmmo--;
                if (ArrowAmmo <= 0)
                {
                    isLeftWeaponSpawned = false;
                    isRightWeaponSpawned = false;
                }
            }
            /* else if(isLeftWeaponSpawned == false || isRightWeaponSpawned == false)
             {
                 if (gestureCompletionData.gestureName == "Left Arrow Draw" || gestureCompletionData.gestureName == "Right Arrow Draw")
                 {
                     Debug.Log("Cannot Fire Arrow, Bow Has Not Been Spawned");
                 }
             }*/

            //Casts Fire Wall
            if (gestureCompletionData.gestureName == "Left Fire Wall" && isTouchingElement)
            {
                Debug.Log("Fire Wall Successfully Casted");
            }
            if (gestureCompletionData.gestureName == "Right Fire Wall" && isTouchingElement)
            {
                Debug.Log("Fire Wall Successfully Casted");
            }

            //Casts Element Spawn (for Earth, Water, and Air)
            if (gestureCompletionData.gestureName == "Left Element Spawn" && !isRightElementSpawned)
            {
                Debug.Log("Element Successfully Spawned");
                isLeftElementSpawned = true;
                isLeftElementSpawned = false;
            }
            if (gestureCompletionData.gestureName == "Right Element Spawn" && !isLeftElementSpawned)
            {
                Debug.Log("Element Successfully Spawned");
                isRightElementSpawned = true;
                isRightElementSpawned = false;
            }

            //Casts Hit (for Water and Earth)
            if (gestureCompletionData.gestureName == "Left Hit" && isTouchingElement)
            {
                WaterHitCount++;
                if(WaterHitCount >= 2)
                {
                    //launch spell
                }
                else if(WaterHitCount == 1)
                {
                    //attach Hand;
                }
                Debug.Log("Hit Successfully Casted");
            }
            if (gestureCompletionData.gestureName == "Right Hit" && isTouchingElement)
            {
                WaterHitCount++;
                if (WaterHitCount >= 2)
                {
                    //launch spell
                }
                else if (WaterHitCount == 1)
                {
                    //attach Hand;
                }
                Debug.Log("Hit Successfully Casted");
            }

            //Casts Rock Wall
            if (gestureCompletionData.gestureName == "Left Rock Wall" && isTouchingElement)
            {
                Debug.Log("Rock Wall Successfully Spawned");
            }
            if (gestureCompletionData.gestureName == "Right Rock Wall" && isTouchingElement)
            {
                Debug.Log("Rock Wall Successfully Spawned");
            }

            //Casts Reload
            if (gestureCompletionData.gestureName == "Left Reload" && isTouchingElement)
            {
                Debug.Log("Reload Successfully Casted");
            }
            if (gestureCompletionData.gestureName == "Right Reload" && isTouchingElement)
            {
                Debug.Log("Reload Successfully Casted");
            }

            //Casts Air Shield
            if (gestureCompletionData.gestureName == "Air Shield" && isTouchingElement)
            {
                Debug.Log("Air Shield Successfully Spawned");
            }

            //Casts Water Shield
            if (gestureCompletionData.gestureName == "Left Water Shield" && isTouchingElement)
            {
                Debug.Log("Water Shield Successfully Spawned");
            }
            if (gestureCompletionData.gestureName == "Right Water Shield" && isTouchingElement)
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
    /*public bool IsWeaponSpawned()
    {
        return isWeaponSpawned;
    }*/

    //Checks if Player is touching element
    public bool IsTouchingElement()
    {
        return isTouchingElement;
    }
}