using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleportComponent : MonoBehaviour
{
    [SerializeField] private GameObject tpToPoint;
    [SerializeField] private PillarLogic tpToPillar;
    [SerializeField] private GameObject tpPoint;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private bool isTutorialActionComplete;
    [SerializeField] private GameObject toArea;
    [SerializeField] private GameObject fromArea;
    public bool isTutorialGate;
    public bool isArenaGate;
    public GameObject spiritGuide;
    public GameObject getTpPoint()
    {
        return tpPoint;
    }
    public PillarLogic getTpToPillar()
    {
        return tpToPillar;
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerHand"))
        {
            if (spiritGuide) spiritGuide.SetActive(false);
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
