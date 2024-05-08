//Written by braden Turner
//a script to allow the player to touch a collide box and to display and cycle through text
using UnityEngine;

public class textHandler : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject textBox;
    public float delayTime = 1.0f; //delay
    [SerializeField] private Animation anim;
    [SerializeField] private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            //activate game object
            objectToActivate.SetActive(true);
            if (!anim.isPlaying) anim.Play();
            //display text
            Invoke("ActivateTextBox", delayTime);

            //mark as activated (no Multiple instances)
            hasActivated = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (hasActivated && other.CompareTag("Player"))
        {
            //activate game object
            objectToActivate.SetActive(false);
            if (!anim.isPlaying) anim.Stop();
            //display text
            if (textBox) textBox.SetActive(false);

            //mark as activated (no Multiple instances)
            hasActivated = false;
        }
    }

    private void ActivateTextBox()
    {
        //activate call
        if (textBox) textBox.SetActive(true);
    }
}
