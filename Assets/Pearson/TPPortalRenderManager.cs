using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPortalRenderManager : MonoBehaviour
{
    public GameObject PortalHolder;
    //public GameObject LookPoint;
    public List<GameObject> PortalParts;
    public GameObject LeftHand;
    public GameObject RightHand;
    public float MaxDistance;
    public float MaxSize;
    public bool isUpdating = false;
    public void updatePortal()
    {
        Vector3 direction = -(LeftHand.transform.position - RightHand.transform.position).normalized;
        float distance = Vector3.Distance(LeftHand.transform.position, RightHand.transform.position);
        Vector3 Modify = direction * (distance / 2);
        Vector3 Position = LeftHand.transform.position + Modify;
        float DistancePercentage = distance / MaxDistance;
        transform.position = Position;
        
        Vector3 direction2 = -(Camera.main.transform.position - Position).normalized;
        transform.forward = direction2;

        Vector3 NewScale = new Vector3(MaxSize * DistancePercentage, MaxSize * (DistancePercentage / 2), MaxSize * DistancePercentage);
        
        for (int i = 0; i < PortalParts.Count; i++)
        {

            PortalParts[i].transform.localScale = NewScale;
        }

    }

    public void StartThis()
    {
        isUpdating = true;
        PortalHolder.SetActive(true);
       // LookPoint.SetActive(true);
    }

    public void resetThis()
    {
        isUpdating = false;
        for (int i = 0; i < PortalParts.Count; i++)
        {
            PortalParts[i].transform.localScale = Vector3.zero;
        }
        PortalHolder.SetActive(false);
       // LookPoint.SetActive(false);
    }
}
