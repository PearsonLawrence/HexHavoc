using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSpell : MonoBehaviour
{
    public Transform networkedOwner;
    public float speed;
    public float amplitude = 2f; 

    void Start()
    {
        speed = .2f;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        float x = Time.time * speed;
        float y = Mathf.Sin(x) * amplitude;

        // Set the new position
        Vector3 newPosition = new Vector3(x, y, 0f);
        transform.Translate(newPosition * Time.deltaTime);
    }
}
