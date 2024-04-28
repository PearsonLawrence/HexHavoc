//Created by Mason Smith and Pearson Lawrence using MiVRy Gesture Recognition Tutorial. Reads player input gestures, and completes specific tasks according to the value of the gesture.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using System.IO;

public class GestureEventProcessor : MonoBehaviour
{
    public ParticleSystem UpBowParticleLeft, UpBowParticleRight;
    public ParticleSystem DownBowParticleLeft, DownBowParticleRight;
    public InputActionProperty leftTriggerProperty;
    public InputActionProperty rightTriggerProperty;
    public bool isHandsTouching = false;
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
    public int reloadCount;
    public int reloadCountMax = 3;

    public TeleportationManager teleportationManager;
    public SpellManager spellmanager;
    public SpellSpawner spellSpawner;
    public UnNetworkedSpellManager unNetworkSpellmanager;
    public UnNetworkPlayer unNetworkPlayer;
    private GestureCombinations gc;

    public DestroyManager CurrentElement;
    public GameObject AirGunLeft, AirGunRight;

    
    // Start is called before the first frame update
    void Start()
    {
        gc = new GestureCombinations(2);
        #if UNITY_EDITOR // this will happen when using the Unity Editor:
        int error = gc.loadFromFile("StreamingAssets/Gestures/1and2HandGestures.dat");
        #else // this will happen in stand-alone build:
        int error = gc.loadFromFile(Application.streamingAssetsPath + "/Gestures/1and2HandGestures.dat");
        #endif
        /*if (error != null)
        {
            throw new Exception(GestureRecognition.getErrorMessage(error) + " at: " + Application.streamingAssetsPath + "/1and2HandGestures.dat");
        }*/
        //Sets current ammo to max ammo when bow/gun is spawned
        currentBowAmmo = maxBowAmmo;
        currentGunAmmo = maxAmmo;
        reloadCount = 0;
        UpBowParticleRight.Stop();
        DownBowParticleRight.Stop();
        UpBowParticleLeft.Stop();
        DownBowParticleLeft.Stop();
    }

    //Update is called once per frame
    void Update()
    {
        bool triggerValue = leftTriggerProperty.action.IsPressed();
        bool triggerValue2 = rightTriggerProperty.action.IsPressed();
        //Prevents gun from firing multiple times on trigger hold


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
            if (gestureCompletionData.gestureName == "Right Element Spawn" && !isElementSpawned && !isGunSpawnedRight && !isBowSpawnedRight)
            {
                isElementSpawned = true;
                spellSpawner.SpawnElementRight();
                Debug.Log("Element Successfully Spawned");
            }
            if (gestureCompletionData.gestureName == "Left Element Spawn" && !isElementSpawned && !isGunSpawnedLeft && !isBowSpawnedLeft)
            {
                isElementSpawned = true;
                Debug.Log("Element Successfully Spawned");
                spellSpawner.SpawnElementLeft();
            }
            if (spellmanager)
            {
                if (spellmanager.elementSpeicalization.Value != unNetworkSpellmanager.elementSpeicalization) unNetworkSpellmanager.elementSpeicalization = spellmanager.elementSpeicalization.Value;
                
                switch (spellmanager.elementSpeicalization.Value)
                {
                    case elementType.FIRE:
                        //Casts Left Bow Spawn
                        if (gestureCompletionData.gestureName == "Left Bow Spawn" && isTouchingElement && !isBowSpawnedRight && isElementSpawned)
                        {
                            Debug.Log("Left Bow Successfully Spawned");
                            isBowSpawnedLeft = true;
                            currentBowAmmo = maxBowAmmo;
                            hasAmmo = true;
                            UpBowParticleLeft.Play();
                            DownBowParticleLeft.Play();
                            isElementSpawned = false;
                            CurrentElement.destroyThis();

                        }

                        //Casts Right Bow Spawn
                        if (gestureCompletionData.gestureName == "Right Bow Spawn" && isTouchingElement && !isBowSpawnedLeft && isElementSpawned)
                        {
                            Debug.Log("Right Bow Successfully Spawned");
                            isBowSpawnedRight = true;
                            currentBowAmmo = maxBowAmmo;
                            hasAmmo = true;
                            UpBowParticleRight.Play();
                            DownBowParticleRight.Play();
                            isElementSpawned = false;
                            CurrentElement.destroyThis();

                        }

                        //Casts Left Arrow Draw
                        //Checks if Bow is spawned. If not, will not fire arrow.
                        if (isBowSpawnedRight)
                        {
                            //Checks if Bow has ammo. If not, will not fire arrow.
                            if (hasAmmo && currentBowAmmo > 0)
                            {
                                if (gestureCompletionData.gestureName == "Left Arrow Draw")
                                {
                                    currentBowAmmo--;
                                    if (currentBowAmmo <= 0)
                                    {
                                        hasAmmo = false;
                                    }
                                    spellmanager.fireRightProjectile();
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
                                UpBowParticleRight.Stop();
                                DownBowParticleRight.Stop();
                                UpBowParticleLeft.Stop();
                                DownBowParticleLeft.Stop();
                                //Destroy(BowGameObject);
                            }
                        }
                        else if (!isBowSpawnedRight)
                        {
                            if (gestureCompletionData.gestureName == "Left Arrow Draw")
                            {
                                Debug.Log("Cannot Fire Arrow, Left Bow Has Not Been Spawned");
                            }
                        }

                        //Casts Right Arrow Draw
                        //Checks if Bow is spawned. If not, will not fire arrow.
                        if (isBowSpawnedLeft)
                        {
                            //Checks if Bow has ammo. If not, will not fire arrow.
                            if (hasAmmo && currentBowAmmo > 0)
                            {
                                if (gestureCompletionData.gestureName == "Right Arrow Draw")
                                {
                                    currentBowAmmo--;
                                    if (currentBowAmmo <= 0)
                                    {
                                        hasAmmo = false;
                                    }
                                    spellmanager.fireLeftProjectile();
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
                                UpBowParticleRight.Stop();
                                DownBowParticleRight.Stop();
                                UpBowParticleLeft.Stop();
                                DownBowParticleLeft.Stop();
                                //Destroy(BowGameObject);
                            }
                        }
                        else if (!isBowSpawnedLeft)
                        {
                            if (gestureCompletionData.gestureName == "Right Arrow Draw")
                            {
                                Debug.Log("Cannot Fire Arrow, Right Bow Has Not Been Spawned");
                            }
                        }

                        //Casts Fire Wall
                        if (gestureCompletionData.gestureName == "Right Fire Wall")
                        {
                            Debug.Log("Fire Wall Successfully Casted");
                            spellmanager.fireRightWall();
                            isElementSpawned = false;
                            CurrentElement.destroyThis();


                        }
                        if (gestureCompletionData.gestureName == "Left Fire Wall")
                        {
                            Debug.Log("Fire Wall Successfully Casted");
                            spellmanager.fireLeftWall();

                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                        }
                        break;
                    case elementType.EARTH:
                        //Casts Rock Wall
                        if (gestureCompletionData.gestureName == "Right Rock Wall" && isTouchingElement)
                        {
                            Debug.Log("Rock Wall Successfully Spawned");
                            spellmanager.fireRightWall();

                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                        }
                        //Casts Rock Wall
                        if (gestureCompletionData.gestureName == "Left Rock Wall" && isTouchingElement)
                        {
                            Debug.Log("Rock Wall Successfully Spawned");
                            spellmanager.fireLeftWall();

                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                        }
                        break;
                    case elementType.WIND:
                        //Casts Reload
                        if (gestureCompletionData.gestureName == "Right Reload" && isTouchingElement && isGunSpawnedLeft && !hasAmmo)
                        {
                            //Refills ammo back to maxAmmo value
                            currentGunAmmo = maxAmmo;
                            hasAmmo = true;
                            reloadCount++;
                            Debug.Log("Reload Successfully Casted. Current Ammo Left: " + currentGunAmmo);
                            isElementSpawned = false;
                            CurrentElement.destroyThis();

                        }

                        if (gestureCompletionData.gestureName == "Left Reload" && isTouchingElement && isGunSpawnedRight && !hasAmmo)
                        {
                            //Refills ammo back to maxAmmo value
                            currentGunAmmo = maxAmmo;
                            hasAmmo = true;
                            reloadCount++;
                            Debug.Log("Reload Successfully Casted. Current Ammo Left: " + currentGunAmmo);
                            isElementSpawned = false;
                            CurrentElement.destroyThis();

                        }

                        //Casts Left Gun Spawn (Same gesture name as Left Bow Spawn)
                        if (gestureCompletionData.gestureName == "Left Bow Spawn" && isTouchingElement && !isGunSpawnedRight && !isGunSpawnedLeft && isElementSpawned)
                        {
                            Debug.Log("Left Gun Successfully Spawned");
                            isGunSpawnedLeft = true;
                            hasAmmo = true;
                            currentGunAmmo = maxAmmo;
                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                            AirGunLeft.SetActive(true);
                            reloadCount = 0;

                        }

                        //Casts Right Gun Spawn (Same gesture name as Right Bow Spawn)
                        if (gestureCompletionData.gestureName == "Right Bow Spawn" && isTouchingElement && !isGunSpawnedLeft && !isGunSpawnedRight && isElementSpawned)
                        {
                            Debug.Log("Right Gun Successfully Spawned");
                            isGunSpawnedRight = true;
                            hasAmmo = true;
                            currentGunAmmo = maxAmmo;
                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                            AirGunRight.SetActive(true);
                            reloadCount = 0;

                        }

                        //Casts Air Shield
                        if (gestureCompletionData.gestureName == "Air Shield" && isTouchingElement)
                        {
                            Debug.Log("Air Shield Successfully Spawned");
                            isElementSpawned = false;
                            spellmanager.fireLeftWall();
                            CurrentElement.destroyThis();
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
                            CurrentElement.destroyThis();

                        }
                        if (gestureCompletionData.gestureName == "Right Water Shield" && isTouchingElement)
                        {
                            Debug.Log("Water Shield Successfully Spawned");
                            isElementSpawned = false;
                            spellmanager.fireRightWall();
                            CurrentElement.destroyThis();

                        }
                        break;
                }
            }
            else if (unNetworkSpellmanager)
            {
                switch (unNetworkSpellmanager.elementSpeicalization)
                {
                    case elementType.FIRE:
                        //Casts Left Bow Spawn
                        if (gestureCompletionData.gestureName == "Left Bow Spawn" && isTouchingElement && !isBowSpawnedRight && isElementSpawned)
                        {
                            Debug.Log("Left Bow Successfully Spawned");
                            isBowSpawnedLeft = true;
                            currentBowAmmo = maxBowAmmo;
                            hasAmmo = true;
                            UpBowParticleLeft.Play();
                            DownBowParticleLeft.Play();
                            isElementSpawned = false;
                            CurrentElement.destroyThis();

                        }

                        //Casts Right Bow Spawn
                        if (gestureCompletionData.gestureName == "Right Bow Spawn" && isTouchingElement && !isBowSpawnedLeft && isElementSpawned)
                        {
                            Debug.Log("Right Bow Successfully Spawned");
                            isBowSpawnedRight = true;
                            currentBowAmmo = maxBowAmmo;
                            hasAmmo = true;
                            UpBowParticleRight.Play();
                            DownBowParticleRight.Play();
                            isElementSpawned = false;
                            CurrentElement.destroyThis();

                        }

                        //Casts Left Arrow Draw
                        //Checks if Bow is spawned. If not, will not fire arrow.
                        if (isBowSpawnedRight)
                        {
                            //Checks if Bow has ammo. If not, will not fire arrow.
                            if (hasAmmo && currentBowAmmo > 0)
                            {
                                if (gestureCompletionData.gestureName == "Left Arrow Draw")
                                {
                                    currentBowAmmo--;
                                    if (currentBowAmmo <= 0)
                                    {
                                        hasAmmo = false;
                                    }
                                    unNetworkSpellmanager.SpawnRightProjectile();
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
                                UpBowParticleRight.Stop();
                                DownBowParticleRight.Stop();
                                UpBowParticleLeft.Stop();
                                DownBowParticleLeft.Stop();
                                //Destroy(BowGameObject);
                            }
                        }
                        else if (!isBowSpawnedRight)
                        {
                            if (gestureCompletionData.gestureName == "Left Arrow Draw")
                            {
                                Debug.Log("Cannot Fire Arrow, Left Bow Has Not Been Spawned");
                            }
                        }

                        //Casts Right Arrow Draw
                        //Checks if Bow is spawned. If not, will not fire arrow.
                        if (isBowSpawnedLeft)
                        {
                            //Checks if Bow has ammo. If not, will not fire arrow.
                            if (hasAmmo && currentBowAmmo > 0)
                            {
                                if (gestureCompletionData.gestureName == "Right Arrow Draw")
                                {
                                    currentBowAmmo--;
                                    if (currentBowAmmo <= 0)
                                    {
                                        hasAmmo = false;
                                    }
                                    unNetworkSpellmanager.SpawnLeftProjectile();
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
                                UpBowParticleRight.Stop();
                                DownBowParticleRight.Stop();
                                UpBowParticleLeft.Stop();
                                DownBowParticleLeft.Stop();
                                //Destroy(BowGameObject);
                            }
                        }
                        else if (!isBowSpawnedLeft)
                        {
                            if (gestureCompletionData.gestureName == "Right Arrow Draw")
                            {
                                Debug.Log("Cannot Fire Arrow, Right Bow Has Not Been Spawned");
                            }
                        }

                        //Casts Fire Wall
                        if (gestureCompletionData.gestureName == "Right Fire Wall")
                        {
                            Debug.Log("Fire Wall Successfully Casted");
                            unNetworkSpellmanager.spawnRightWall();
                            isElementSpawned = false;
                            CurrentElement.destroyThis();


                        }
                        if (gestureCompletionData.gestureName == "Left Fire Wall")
                        {
                            Debug.Log("Fire Wall Successfully Casted");
                            unNetworkSpellmanager.spawnLeftWall();

                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                        }
                        break;
                    case elementType.EARTH:
                        //Casts Rock Wall
                        if (gestureCompletionData.gestureName == "Right Rock Wall" && isTouchingElement)
                        {
                            Debug.Log("Rock Wall Successfully Spawned");
                            unNetworkSpellmanager.spawnRightWall();

                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                        }
                        //Casts Rock Wall
                        if (gestureCompletionData.gestureName == "Left Rock Wall" && isTouchingElement)
                        {
                            Debug.Log("Rock Wall Successfully Spawned");
                            unNetworkSpellmanager.spawnLeftWall();

                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                        }
                        break;
                    case elementType.WIND:
                        //Casts Reload
                        if (gestureCompletionData.gestureName == "Right Reload" && isTouchingElement && isGunSpawnedLeft && !hasAmmo)
                        {
                            //Refills ammo back to maxAmmo value
                            currentGunAmmo = maxAmmo;
                            hasAmmo = true;
                            reloadCount++;
                            Debug.Log("Reload Successfully Casted. Current Ammo Left: " + currentGunAmmo);
                            isElementSpawned = false;
                            CurrentElement.destroyThis();

                        }

                        if (gestureCompletionData.gestureName == "Left Reload" && isTouchingElement && isGunSpawnedRight && !hasAmmo)
                        {
                            //Refills ammo back to maxAmmo value
                            currentGunAmmo = maxAmmo;
                            hasAmmo = true;
                            reloadCount++;
                            Debug.Log("Reload Successfully Casted. Current Ammo Left: " + currentGunAmmo);
                            isElementSpawned = false;
                            CurrentElement.destroyThis();

                        }

                        //Casts Left Gun Spawn (Same gesture name as Left Bow Spawn)
                        if (gestureCompletionData.gestureName == "Left Bow Spawn" && isTouchingElement && !isGunSpawnedRight && !isGunSpawnedLeft && isElementSpawned)
                        {
                            Debug.Log("Left Gun Successfully Spawned");
                            isGunSpawnedLeft = true;
                            hasAmmo = true;
                            currentGunAmmo = maxAmmo;
                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                            AirGunLeft.SetActive(true);
                            reloadCount = 0;

                        }

                        //Casts Right Gun Spawn (Same gesture name as Right Bow Spawn)
                        if (gestureCompletionData.gestureName == "Right Bow Spawn" && isTouchingElement && !isGunSpawnedLeft && !isGunSpawnedRight && isElementSpawned)
                        {
                            Debug.Log("Right Gun Successfully Spawned");
                            isGunSpawnedRight = true;
                            hasAmmo = true;
                            currentGunAmmo = maxAmmo;
                            isElementSpawned = false;
                            CurrentElement.destroyThis();
                            AirGunRight.SetActive(true);
                            reloadCount = 0;

                        }

                        //Casts Air Shield
                        if (gestureCompletionData.gestureName == "Air Shield" && isTouchingElement)
                        {
                            Debug.Log("Air Shield Successfully Spawned");
                            isElementSpawned = false;
                            unNetworkSpellmanager.spawnLeftWall();
                            CurrentElement.destroyThis();
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
                            unNetworkSpellmanager.spawnLeftWall();
                            CurrentElement.destroyThis();

                        }
                        if (gestureCompletionData.gestureName == "Right Water Shield" && isTouchingElement)
                        {
                            Debug.Log("Water Shield Successfully Spawned");
                            isElementSpawned = false;
                            unNetworkSpellmanager.spawnRightWall();
                            CurrentElement.destroyThis();

                        }
                        break;
                }
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
                    Debug.Log("Left Gun Fired. Current Ammo Left: " + currentGunAmmo);
                    if (spellmanager)
                    {
                        spellmanager.fireLeftProjectile();
                    }
                    else
                    {
                        unNetworkSpellmanager.SpawnLeftProjectile();
                    }
                    if(currentGunAmmo <= 0)
                    {
                        hasAmmo = false;
                        if(reloadCount >= reloadCountMax)
                        {
                            isGunSpawnedLeft = false;
                            AirGunLeft.SetActive(false);
                            reloadCount = 0;
                            //TODO: Disable;
                        }
                    }
                }
                else if (GetTriggerButtonState(UnityEngine.XR.CommonUsages.triggerButton, XRNode.RightHand))
                {
                    Debug.Log("Cannot Fire Left Gun. Left gun spawned but right button tried to fire.");
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
                    Debug.Log("Right Gun Fired. Current Ammo Left: " + currentGunAmmo);
                    if (spellmanager)
                    {
                        spellmanager.fireRightProjectile();
                    }
                    else
                    {
                        unNetworkSpellmanager.SpawnRightProjectile();
                    }
                    if (currentGunAmmo <= 0)
                    {
                        hasAmmo = false;
                        if (reloadCount >= reloadCountMax)
                        {
                            isGunSpawnedRight = false;
                            AirGunRight.SetActive(false);
                            reloadCount = 0;
                            //TODO: Disable;
                        }
                    }
                }
                else if (GetTriggerButtonState(UnityEngine.XR.CommonUsages.triggerButton, XRNode.LeftHand))
                {
                    Debug.Log("Could Not Fire Right Gun. Right gun spawned but left button tried to fire.");
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