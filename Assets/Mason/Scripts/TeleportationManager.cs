using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public GameObject teleportationRayPrefab;

    private GameObject teleportationRay;
    private Transform leftHand;
    private Transform rightHand;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.Find("Left Controller").transform;
        rightHand = GameObject.Find("Right Controller").transform;
    }

    //Called when Teleport gesture is recognized
    public void SpawnTeleportationRay()
    { 
        teleportationRay = Instantiate(teleportationRayPrefab, (leftHand.position + rightHand.position) / 2f, Quaternion.identity);
    }

    //Called when Teleport gesture is completed
    public void Teleport()
    { 
        Vector3 destination = teleportationRay.transform.position;
        Destroy(teleportationRay);
    }
}
