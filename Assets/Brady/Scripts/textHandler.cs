//Written by Braden Turner
//handles the UI prompts to give player information

using UnityEngine;

public class textHandler : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject textBox;
    public float delayTime = 1.0f; //Delay for text

    private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            //on collide activate
            objectToActivate.SetActive(true);

            //activate text box
            Invoke("ActivateTextBox", delayTime);

            //mark activated (doesnt spawn multiple instances)
            hasActivated = true;
        }
    }

    private void ActivateTextBox()
    {
        //show text box
        textBox.SetActive(true);
    }
}
