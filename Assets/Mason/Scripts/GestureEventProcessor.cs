//Created by Mason Smith using MiVRy Gesture Recognition Tutorial. Reads player input gestures, and completes specific tasks according to the value of the gesture.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class GestureEventProcessor : MonoBehaviour
{
    public InputActionProperty leftTriggerProperty;
    public InputActionProperty rightTriggerProperty;

    private bool isEarthClass = false;
    private bool isFireClass = false;
    private bool isWaterClass = false;
    private bool isAirClass = false;

    private bool isTeleportGestureRecognized = false;
    private bool isBowSpawnedLeft = false;
    private bool isBowSpawnedRight = false;
    private bool isGunSpawnedLeft = false;
    private bool isGunSpawnedRight = false;
    public bool isTouchingElement = false;
    public bool isElementSpawned = false;

    private bool hasAmmo = false;
    private bool hasShotLeft = false;
    private bool hasShotRight = false;
    public int maxAmmo = 5;
    public int maxBowAmmo = 5;
    private int currentBowAmmo;
    private int currentGunAmmo;
    private int reloadCount;

    public TeleportationManager teleportationManager;
    public SpellManager spellmanager;
    public SpellSpawner spellSpawner;
    public UnNetworkedSpellManager unNetworkSpellmanager;
    public UnNetworkPlayer unNetworkPlayer;
    //public GestureRecognition gr = newGestureRecognition();
    // Start is called before the first frame update
    void Start()
    {
        //gr.loadFromFile("StreamingAssets/1and2HandGestures.dat");
        //Sets current ammo to max ammo when bow/gun is spawned
        currentBowAmmo = maxBowAmmo;
        currentGunAmmo = maxAmmo;
        reloadCount = 0;
    }

    //Update is called once per frame
    void Update()
    {
        bool triggerValue = leftTriggerProperty.action.IsPressed();
        bool triggerValue2 = rightTriggerProperty.action.IsPressed();
        //Prevents gun from firing multiple times on trigger hold

        if(spellmanager != null)
        {
            if(spellSpawner.spellManager == null)
            {
                spellSpawner.spellManager = spellmanager;
            }
        }

        if (!triggerValue && hasShotLeft)
        {
            hasShotLeft = false;
        }
        else if (triggerValue && isGunSpawnedLeft && !hasShotLeft)
        {
            FireLeftGun();
            hasShotLeft = true;
        }
        if (!triggerValue2 && hasShotRight)
        {
            hasShotRight = false;
        }
        else if (triggerValue2 && isGunSpawnedRight && !hasShotRight)
        {
            FireRightGun();
            hasShotRight = true;
        }
        if (currentGunAmmo == 0)
        {
            hasAmmo = false;
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
        if (gestureCompletionData.similarity >= 0.7) {
            //Casts Element Spawn (for Earth, Water, and Air)
            if (gestureCompletionData.gestureName == "Right Element Spawn" && !isElementSpawned)
            {
                isElementSpawned = true;
                spellSpawner.SpawnElementRight();
                Debug.Log("Element Successfully Spawned");
            }
            if (gestureCompletionData.gestureName == "Left Element Spawn" && !isElementSpawned)
            {
                isElementSpawned = true;
                Debug.Log("Element Successfully Spawned");
                spellSpawner.SpawnElementLeft();
            }
            if (spellmanager)
            {
                switch (spellmanager.elementSpeicalization)
                {
                    case elementType.FIRE:
                        //Casts Left Bow Spawn
                        if (gestureCompletionData.gestureName == "Left Bow Spawn" && isTouchingElement)
                        {
                            Debug.Log("Left Bow Successfully Spawned");
                            isBowSpawnedLeft = true;
                            currentBowAmmo = maxBowAmmo;
                            hasAmmo = true;
                        }

                        //Casts Right Bow Spawn
                        if (gestureCompletionData.gestureName == "Right Bow Spawn" && isTouchingElement)
                        {
                            Debug.Log("Right Bow Successfully Spawned");
                            isBowSpawnedRight = true;
                            currentBowAmmo = maxBowAmmo;
                            hasAmmo = true;
                        }

                        //Casts Left Arrow Draw
                        //Checks if Bow is spawned. If not, will not fire arrow.
                        if (isBowSpawnedLeft)
                        {
                            //Checks if Bow has ammo. If not, will not fire arrow.
                            if (hasAmmo && currentBowAmmo > 0)
                            {
                                if (gestureCompletionData.gestureName == "Left Arrow Draw")
                                {
                                    currentBowAmmo--;
                                    spellmanager.fireLeftProjectile();
                                    Debug.Log("Arrow Successfully Fired From Left Bow. Current Ammo Left: " + currentBowAmmo);
                                }
                            }
                            else if (!hasAmmo && currentBowAmmo == 0)
                            {
                                Debug.Log("Cannot Fire Arrow, Left Bow Has No Ammo. Bow Despawned.");
                                isTouchingElement = false;
                                isBowSpawnedLeft = false;
                                isBowSpawnedRight = false;
                                isElementSpawned = false;
                                //Destroy(BowGameObject);
                            }
                        }
                        else if (!isBowSpawnedLeft)
                        {
                            if (gestureCompletionData.gestureName == "Left Arrow Draw")
                            {
                                Debug.Log("Cannot Fire Arrow, Left Bow Has Not Been Spawned");
                            }
                        }

                        //Casts Right Arrow Draw
                        //Checks if Bow is spawned. If not, will not fire arrow.
                        if (isBowSpawnedRight)
                        {
                            //Checks if Bow has ammo. If not, will not fire arrow.
                            if (hasAmmo && currentBowAmmo > 0)
                            {
                                if (gestureCompletionData.gestureName == "Right Arrow Draw")
                                {
                                    currentBowAmmo--;
                                    spellmanager.fireRightProjectile();
                                    Debug.Log("Arrow Successfully Fired From Right Bow. Current Ammo Left: " + currentBowAmmo);
                                }
                            }
                            else if (!hasAmmo)
                            {
                                Debug.Log("Cannot Fire Arrow, Right Bow Has No Ammo. Bow Despawned.");
                                isTouchingElement = false;
                                isBowSpawnedLeft = false;
                                isBowSpawnedRight = false;
                                isElementSpawned = false;
                                //Destroy(BowGameObject);
                            }
                        }
                        else if (!isBowSpawnedRight)
                        {
                            if (gestureCompletionData.gestureName == "Right Arrow Draw")
                            {
                                Debug.Log("Cannot Fire Arrow, Right Bow Has Not Been Spawned");
                            }
                        }

                        //Casts Fire Wall
                        if (gestureCompletionData.gestureName == "Right Fire Wall" && isTouchingElement)
                        {
                            Debug.Log("Fire Wall Successfully Casted");
                            spellmanager.fireLeftWall();
                            isElementSpawned = false;
                        }
                        if (gestureCompletionData.gestureName == "Left Fire Wall" && isTouchingElement)
                        {
                            Debug.Log("Fire Wall Successfully Casted");
                            spellmanager.fireRightWall();
                            isElementSpawned = false;
                        }
                        break;
                    case elementType.EARTH:
                        //Casts Rock Wall
                        if (gestureCompletionData.gestureName == "Right Rock Wall" && isTouchingElement)
                        {
                            Debug.Log("Rock Wall Successfully Spawned");
                            spellmanager.fireRightWall();
                            isElementSpawned = false;
                        }
                        //Casts Rock Wall
                        if (gestureCompletionData.gestureName == "Left Rock Wall" && isTouchingElement)
                        {
                            Debug.Log("Rock Wall Successfully Spawned");
                            spellmanager.fireLeftWall();
                            isElementSpawned = false;
                        }
                        break;
                    case elementType.WIND:
                        //Casts Reload
                        if ((gestureCompletionData.gestureName == "Left Reload" || gestureCompletionData.gestureName == "Right Reload") && isTouchingElement)
                        {
                            //Refills ammo back to maxAmmo value
                            currentGunAmmo = maxAmmo;
                            hasAmmo = true;
                            reloadCount++;
                            Debug.Log("Reload Successfully Casted. Current Ammo Left: " + currentGunAmmo);
                            isElementSpawned = false;
                        }

                        //Casts Left Gun Spawn (Same gesture name as Left Bow Spawn)
                        if (gestureCompletionData.gestureName == "Left Bow Spawn" && isTouchingElement)
                        {
                            Debug.Log("Left Gun Successfully Spawned");
                            isGunSpawnedLeft = true;
                            hasAmmo = true;
                            currentGunAmmo = maxAmmo;
                            isElementSpawned = true;
                        }

                        //Casts Right Gun Spawn (Same gesture name as Right Bow Spawn)
                        if (gestureCompletionData.gestureName == "Right Bow Spawn" && isTouchingElement)
                        {
                            Debug.Log("Right Gun Successfully Spawned");
                            isGunSpawnedRight = true;
                            hasAmmo = true;
                            currentGunAmmo = maxAmmo;
                            isElementSpawned = true;
                        }

                        //Casts Air Shield
                        if (gestureCompletionData.gestureName == "Air Shield" && isTouchingElement)
                        {
                            Debug.Log("Air Shield Successfully Spawned");
                            isElementSpawned = false;
                        }

                        break;
                    case elementType.WATER:
                        //Casts Hit (for Water and Earth)
                        if ((gestureCompletionData.gestureName == "Left Hit" || gestureCompletionData.gestureName == "Right Hit") && isTouchingElement)
                        {
                            Debug.Log("Hit Successfully Casted");
                        }
                        //Casts Water Shield
                        if (gestureCompletionData.gestureName == "Left Water Shield" && isTouchingElement)
                        {
                            Debug.Log("Water Shield Successfully Spawned");
                            isElementSpawned = false;
                            spellmanager.fireLeftWall();
                        }
                        if (gestureCompletionData.gestureName == "Right Water Shield" && isTouchingElement)
                        {
                            Debug.Log("Water Shield Successfully Spawned");
                            isElementSpawned = false;
                            spellmanager.fireRightWall();
                        }
                        break;
                }
            }
            

            //Casts Left Hand Wall Spell
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

    //Checks if Player chose Earth Class
    public bool IsEarthClass()
    {
        return isEarthClass;
    }

    //Checks if Player chose Fire Class
    public bool IsFireClass()
    {
        return isFireClass;
    }

    //Checks if Player chose Water Class
    public bool IsWaterClass()
    {
        return isWaterClass;
    }

    //Checks if Player chose Air Class
    public bool IsAirClass()
    {
        return isAirClass;
    }

    //Checks if Bow has been spawned in Left hand
    public bool IsBowSpawnedLeft()
    {
        return isBowSpawnedLeft;
    }

    //Checks if Bow has been spawned in Right hand
    public bool IsBowSpawnedRight()
    {
        return isBowSpawnedRight;
    }

    //Checks if Gun has been spawned in Left hand
    public bool IsGunSpawnedLeft()
    {
        return isGunSpawnedLeft;
    }

    //Checks if Gun has been spawned in Right hand
    public bool IsGunSpawnedRight()
    {
        return isGunSpawnedRight;
    }

    //Checks if Player is touching element
    public bool IsTouchingElement()
    {
        return isTouchingElement;
    }

    public bool HasAmmo()
    {
        return hasAmmo;
    }

    //Gets state of triggers for FireGun() function
    public bool GetTriggerButtonState(InputFeatureUsage<bool> usage, XRNode controllerNode)
    {
        UnityEngine.XR.InputDevice device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(controllerNode);
        bool value;
        if (device.TryGetFeatureValue(usage, out value))
        {
            return value;
        }
        return false;
    }

    //Fires Left Gun
    public void FireLeftGun()
    {
        //Checks if Left Gun is spawned. If not, will not fire gun.
        if (isGunSpawnedLeft)
        {
            //Checks if Left Gun has ammo. If not, will not fire gun.
            if (hasAmmo && currentGunAmmo > 0)
            {
                //Checks if correct trigger has been pulled. If not, will not fire gun.
                if (GetTriggerButtonState(UnityEngine.XR.CommonUsages.triggerButton, XRNode.LeftHand))
                {
                    currentGunAmmo--;
                    spellmanager.fireLeftProjectile();
                    Debug.Log("Left Gun Fired. Current Ammo Left: " + currentGunAmmo);
                }
                else if (GetTriggerButtonState(UnityEngine.XR.CommonUsages.triggerButton, XRNode.RightHand))
                {
                    Debug.Log("Cannot Fire Left Gun. Left gun spawned but right button tried to fire.");
                }
            }
            else if (!hasAmmo)
            {
                //Checks if gun has been reloaded once. If so, will despawn after ammo depletion.
                if (reloadCount >= 1)
                {
                    Debug.Log("Cannot Fire Gun, Left Gun Has No Ammo. Gun Despawned.");
                    //Destroy(GunGameObject);
                    reloadCount = 0;
                }
                else
                {
                    Debug.Log("Cannot Fire Gun, Left Gun Has No Ammo. Must Reload.");
                }
            }
        }
        else if (!isGunSpawnedLeft)
        {
            if (GetTriggerButtonState(UnityEngine.XR.CommonUsages.triggerButton, XRNode.LeftHand))
            {
                Debug.Log("Cannot Fire Gun, Left Gun Not Spawned.");
            }
        }
        else
        {
            Debug.Log("ERROR: Could Not Fire Left Gun");
        }
    }

    //Fires Right Gun
    public void FireRightGun()
    {
        //Checks if Right Gun is spawned. If not, will not fire gun.
        if (isGunSpawnedRight)
        {
            //Checks if Right Gun has ammo. If not, will not fire gun.
            if (hasAmmo && currentGunAmmo > 0)
            {
                //Checks if correct trigger has been pulled. If not, will not fire gun.
                if (GetTriggerButtonState(UnityEngine.XR.CommonUsages.triggerButton, XRNode.RightHand))
                {
                    currentGunAmmo--;
                    spellmanager.fireRightProjectile();
                    Debug.Log("Right Gun Fired. Current Ammo Left: " + currentGunAmmo);
                }
                else if (GetTriggerButtonState(UnityEngine.XR.CommonUsages.triggerButton, XRNode.LeftHand))
                {
                    Debug.Log("Could Not Fire Right Gun. Right gun spawned but left button tried to fire.");
                }
            }
            else if (!hasAmmo)
            {
                //Checks if gun has been reloaded once. If so, will despawn after ammo depletion.
                if (reloadCount >= 1)
                {
                    Debug.Log("Cannot Fire Gun, Right Gun Has No Ammo. Gun Despawned.");
                    //Destroy(GunGameObject);
                    reloadCount = 0;
                }
                else
                {
                    Debug.Log("Cannot Fire Gun, Right Gun Has No Ammo. Must Reload.");
                }
            }
        }
        else if (!isGunSpawnedRight)
        {
            if (GetTriggerButtonState(UnityEngine.XR.CommonUsages.triggerButton, XRNode.RightHand))
            {
                Debug.Log("Cannot Fire Gun, Right Gun Not Spawned.");
            }
        }
        else
        {
            Debug.Log("ERROR: Could Not Fire Right Gun");
        }
    }
}