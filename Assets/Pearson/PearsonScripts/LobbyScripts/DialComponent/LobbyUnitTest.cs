using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUnitTest : MonoBehaviour
{
    public GameLobbyComponent gameLobbyComponent;
   
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TestGenerateLobbyNameReturnsCorrectFormat();
        }
    }

    public void TestGenerateLobbyNameReturnsCorrectFormat()
    {
        
        string playerName = gameLobbyComponent.playerName;
        string expected = gameLobbyComponent.playerName + "'s Arena";

        string result = gameLobbyComponent.getJoinedLobby().Name;

        if (result == expected)
        {
            Debug.Log("TestGenerateLobbyNameReturnsCorrectFormat: Passed");
        }
        else
        {
            Debug.LogError($"TestGenerateLobbyNameReturnsCorrectFormat: Failed, expected '{expected}' but got '{result}'");
        }
    }
}
