//Created by Mason Smith. Used to activate teleportation during two-handed Teleport gesture.
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public GameObject teleportationRayPrefab;
    public Material outlineMaterial;

    private GameObject teleportationRay;
    private Transform playerHead;
    private GestureEventProcessor gestureEventProcessor;
    private GameObject currentPillarWithOutline;
    public XROrigin xr;
    // Start is called before the first frame update
    void Start()
    {
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
            if (hit.collider.gameObject.CompareTag("Pillar"))
            {
                PillarLogic pillarLogic = hit.collider.gameObject.GetComponent<PillarLogic>();
                if (pillarLogic)
                {
                    EnableOutline(hit.collider.gameObject);
                    xr.GetComponent<UnNetworkPlayer>().currentPillar = pillarLogic;
                }
            }
        }
        else
        {
            Debug.Log("No teleport destination found.");
        }
        
    }

    //Enables outline on specified Pillar
    void EnableOutline(GameObject obj)
    {
        //Disables outline on previous pillar
        DisableOutline();

        //Assigns outline material to pillar's renderer
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            outlineMaterial[] materials =  renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = outlineMaterial;
            }
            renderer.materials = materials;

            //Stores current pillar w/ outline
            currentPillarWithOutline = obj;
        }
    }

    //Disables outline effect on previous pillar
    void DisableOutline()
    {
        if (currentPillarWithOutline != null)
        {
            Renderer renderer = currentPillarWithOutline.GetComponent<Renderer>();
            if (renderer != null)
            {
                outlineMaterial[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    //Resets material to original material
                    materials[i] = currentPillarWithOutline.GetComponent<PillarLogic>().originalMaterial;
                }
                renderer.material = materials;
            }
            currentPillarWithOutline = null;
        }
    }
}