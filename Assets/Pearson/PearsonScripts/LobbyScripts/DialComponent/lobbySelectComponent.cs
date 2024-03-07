using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbySelectComponent : MonoBehaviour
{
    public LobbyUIManager lobby;
    public void OnTriggerEnter(Collider other)
    {
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
        
    }
}
