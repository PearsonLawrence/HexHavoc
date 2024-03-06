using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialFingerPointComponent : MonoBehaviour
{
    private bool isTouching = false;
    [SerializeField] private PlatformDialComponent dial;

    [SerializeField] private GameObject parent;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isMoving, isRight, isLeft, isUp, isDown;


    public bool getIsTouching()
    {
        return isTouching;
    }

    public void MouseTester()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane + .25f;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
       
        RaycastHit hitPoint;
        Physics.Raycast(ray, out hitPoint, 1000);
        //Debug.Log(transform.localPosition);
        
        if(hitPoint.collider != null && isTouching == false)
        {
            if (hitPoint.collider == this.GetComponent<Collider>() && Input.GetMouseButton(0))
            {
                isTouching = true;
            }
        }

        if (isTouching == true)
        {
            transform.position = worldPos;
        }
        if (isTouching == true && !Input.GetMouseButton(0)) isTouching = false;

        if (isTouching == false) transform.localPosition = Vector3.zero;

        if(transform.localPosition != Vector3.zero)
        {
            //Debug.Log(transform.localPosition.x);
            if (transform.localPosition.x > .025f)
            {
                dial.setIsRight(true);
                dial.setIsLeft(false);
            }
            else if(transform.localPosition.x < -.025f)
            {
                dial.setIsRight(false);
                dial.setIsLeft(true);
            }
            else
            {
                dial.setIsRight(false);
                dial.setIsLeft(false);
            }


           // Debug.Log(transform.localPosition.z);
            if (transform.localPosition.z > .05f)
            {
                dial.setIsUp(false);
                dial.setIsDown(true);
            }
            else if (transform.localPosition.z < -.05f)
            {
                dial.setIsUp(true);
                dial.setIsDown(false);
            }
            else
            {
                dial.setIsUp(false);
                dial.setIsDown(false);
            }

           
        }
        else
        {
            
            dial.setIsRight(false);
            dial.setIsLeft(false);
            dial.setIsUp(false);
            dial.setIsDown(false);
            
        }
        //Debug.Log(dial.getIsDown() + " : " + dial.getIsUp() + " : " + dial.getIsRight() + " : " + dial.getIsLeft());
        
    }

    public void Update()
    {
        MouseTester();
    }

}
