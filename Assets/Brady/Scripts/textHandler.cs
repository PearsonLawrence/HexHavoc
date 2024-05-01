using UnityEngine;

public class textHandler : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject textBox;
    public float delayTime = 1.0f; //delay

    private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            //activate game object
            objectToActivate.SetActive(true);

            //display text
            Invoke("ActivateTextBox", delayTime);

            //mark as activated (no Multiple instances)
            hasActivated = true;
        }
    }

    private void ActivateTextBox()
    {
        //activate call
        textBox.SetActive(true);
    }
}
