//Author: Pearson Lawrence
//purpose: Connect to unity relay, allowing players to connect between PC's. Used in tandum with menu UI to establish connections after joining lobbies
//Utilized Code monkeys tutorial to gain understanding on Relay funcitonality, and then created UI system that utilized and modified it.
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class gameRelayComponent : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
       /* await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();*/
    }


    //Open a relay to connect clients between remote pc's
    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2); //Create relay allocation on unity services
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); //Gets join code for relay
            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls"); 

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); //using a singleton update server data across network manager
            NetworkManager.Singleton.StartHost(); //Starts host on singleton for all instances on pc
            return joinCode;
        }
        catch( RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }
    //handles joining an existing relay by relay code
    public async Task<bool> JoinRelay(string joinCode, UnNetworkPlayer p)
    {
        try
        {
            Debug.Log("Joining Relay with code: " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode); //wait for client to establish connection to relay of code


            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls"); //dtls is the predefined code type

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData); //connect over transport layer using unity services on network manager
            NetworkManager.Singleton.StartClient(); //initiate client on relay to transmit data between pcs
            
            return true;
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
            return false;
        }
    }
}
