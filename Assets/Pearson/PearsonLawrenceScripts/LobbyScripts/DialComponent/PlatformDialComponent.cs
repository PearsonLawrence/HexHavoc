using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDialComponent : MonoBehaviour
{
    [SerializeField] private GameObject platformRotateObject;
    [SerializeField] private DialFingerPointComponent dialFingerPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Rot = new Vector3(0, transform.eulerAngles.z, 0);
        platformRotateObject.gameObject.transform.eulerAngles = Vector3.Slerp(platformRotateObject.gameObject.transform.eulerAngles, Rot, Time.deltaTime * 1);
    }
}
