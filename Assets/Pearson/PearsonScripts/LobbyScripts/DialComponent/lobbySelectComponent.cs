using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbySelectComponent : MonoBehaviour
{
    public LobbyUIManager lobby;
    private Rigidbody rb;
    public float bounds = 40;
    public void OnTriggerEnter(Collider other)
    {
        rb = GetComponent<Rigidbody>();
        string tag = other.tag;
        if(other.gameObject.CompareTag("CreateLobby") )
        {
            lobby.doCreate();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.x > bounds || transform.localPosition.x < -bounds ||
            transform.localPosition.y > bounds || transform.localPosition.y < -bounds ||
            transform.localPosition.z > bounds || transform.localPosition.z < -bounds )
        {
            transform.localPosition = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }
}
