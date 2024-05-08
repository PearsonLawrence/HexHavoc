//Author: Pearson Lawrence
//Purpose: sync the models rig to the players head position and hand position to show over the network.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigSyncComponent : MonoBehaviour
{
    public GameObject RigLHand;
    public GameObject RigRHand;
    public GameObject RigHead;
    public GameObject NetLHand;
    public GameObject NetRHand;
    public GameObject NetHead;
    

    // Update is called once per frame
    void Update()
    {
        RigLHand.gameObject.transform.position = NetLHand.transform.position;
        RigRHand.gameObject.transform.position = NetRHand.transform.position;
        RigHead.gameObject.transform.position = NetHead.transform.position;
        RigLHand.gameObject.transform.rotation = NetLHand.transform.rotation;
        RigRHand.gameObject.transform.rotation = NetRHand.transform.rotation;
        RigHead.gameObject.transform.rotation = NetHead.transform.rotation;
    }
}
