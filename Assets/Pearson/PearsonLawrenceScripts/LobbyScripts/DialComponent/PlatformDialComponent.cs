using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDialComponent : MonoBehaviour
{
    [SerializeField] private GameObject platformRotateObject;
    [SerializeField] private DialFingerPointComponent dialFingerPoint;
    

    // Update is called once per frame
    void Update()
    {
        Vector3 Rot = new Vector3(0, transform.eulerAngles.z, 0);
      

        platformRotateObject.gameObject.transform.eulerAngles = Vector3.Slerp(platformRotateObject.gameObject.transform.eulerAngles, Rot, Time.deltaTime * 1);
        platformRotateObject.gameObject.transform.eulerAngles = new Vector3(0, platformRotateObject.gameObject.transform.eulerAngles.y, 0);
    }
}
