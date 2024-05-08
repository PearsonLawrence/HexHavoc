//Author: Pearson Lawrence
//Purpose: This script handles door teleportation, that will teleport players to new areas while deactivating the previous area they were in.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleportComponent : MonoBehaviour
{
    [SerializeField] private GameObject tpToPoint;
    [SerializeField] private PillarLogic tpToPillar;
    [SerializeField] private GameObject tpPoint;
    [SerializeField] private bool isTutorialActionComplete;
    [SerializeField] private GameObject toArea;
    [SerializeField] private GameObject fromArea;
    public bool isTutorialGate;
    public bool isArenaGate;
    public GameObject spiritGuide;
    public AudioSource audioDoor;
    public GameObject getTpPoint()
    {
        return tpPoint;
    }
    public PillarLogic getTpToPillar()
    {
        return tpToPillar;
    }
    //When the door is touched if it is a player hand then disable the area and tell the player to teleport to a new area specified by the doors to and from values.
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            if (spiritGuide) spiritGuide.SetActive(false);

            //If This door takes you to arena area then flip flop.
            if (isArenaGate)
            {
                toArea.SetActive(true);
                HandInteractableComponent temp = other.GetComponent<HandInteractableComponent>();
                temp.parentUnNetworkObj.currentPillar = tpToPillar;
                temp.parentUnNetworkObj.isTeleported = true;
                temp.parentUnNetworkObj.gameObject.transform.forward = tpToPillar.playerPoint.transform.forward;
                //temp.parentUnNetworkObj.isArena = (temp.parentUnNetworkObj.isArena) ? false : true;
                fromArea.SetActive(false);
            }
            else
            {
                toArea.SetActive(true);
                HandInteractableComponent temp = other.GetComponent<HandInteractableComponent>();
                temp.parentUnNetworkObj.currentPillar = tpToPillar;
                temp.parentUnNetworkObj.isTeleported = true;
                temp.parentUnNetworkObj.gameObject.transform.forward = tpToPillar.playerPoint.transform.forward;
                temp.parentUnNetworkObj.isArena = false;
                fromArea.SetActive(false);
            }

            //Obsolete: Used to indicate tutorial done will be used so that audio doesnt play everytime from spirit guide
            if (!isTutorialActionComplete)
            {
                isTutorialActionComplete = true;
                //tutorialManager.setTutorialActionNum(tutorialManager.getTutorialActionNum() + 1);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioDoor = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
