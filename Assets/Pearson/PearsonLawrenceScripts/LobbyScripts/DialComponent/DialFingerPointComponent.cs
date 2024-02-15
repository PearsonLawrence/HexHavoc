using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialFingerPointComponent : MonoBehaviour
{
    private bool isTouching = false;
    [SerializeField] private PlatformDialComponent dial;
    public bool getIsTouching()
    {
        return isTouching;
    }

    public void MouseTester()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;
        Physics.Raycast(ray, out hitPoint, 1000);

        if(hitPoint.collider != null)
        {
            if (hitPoint.collider == this.GetComponent<Collider>())
            {
                Vector3 dir = (hitPoint.point - dial.transform.position).normalized;
               
                dial.gameObject.transform.up = dir;

                dial.gameObject.transform.eulerAngles = new Vector3(0, 0, dial.gameObject.transform.eulerAngles.z);
            }
        }
    }

    public void Update()
    {
        MouseTester();
    }

}
