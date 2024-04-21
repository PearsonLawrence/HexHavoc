using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DojoControllerComponent : MonoBehaviour
{
    [SerializeField] private GameObject orbholder;
    [SerializeField] private GameObject displayHolder;
    [SerializeField] private List<DojoOrbSelector> orbs;
    [SerializeField] private List<GameObject> elementDisplays;
    public bool toggle = true;

    // Start is called before the first frame update
    public void ToggleDisplay()
    {
        
        orbholder.SetActive(true);
        displayHolder.SetActive(false);
           
    }
    public void setElementDisplay(int idx)
    {
        orbholder.SetActive(false);
        displayHolder.SetActive(true);
        for (int i = 0; i < elementDisplays.Count; i++)
        {
            if (i == idx) elementDisplays[idx].SetActive(true);
            else elementDisplays[i].SetActive(false);
        }
        

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
