using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCameraComponent : MonoBehaviour
{
    public GameObject CameraHeadPos;
    public GameObject tempCam;
    // Start is called before the first frame update
    void Start()
    {
        tempCam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        tempCam.transform.position = CameraHeadPos.transform.position;
        tempCam.transform.rotation = CameraHeadPos.transform.rotation;
    }
}
