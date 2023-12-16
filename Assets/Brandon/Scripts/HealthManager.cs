//Author: Brandon(Ri) Yu
//Purpose: Simple script that creates a networked Health varibale which means that its kept between clients
//and insstaniates all players health to 100

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthManager : NetworkBehaviour
{
    public NetworkVariable<int> Health = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Health.Value = 100;
    }
}
