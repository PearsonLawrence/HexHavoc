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
        if(other.CompareTag("Player"))
        {
            if(!isTutorialActionComplete)
            {
                isTutorialActionComplete = true;
                tutorialManager.setTutorialActionNum(tutorialManager.getTutorialActionNum() + 1);
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
