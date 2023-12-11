using Unity.Netcode;
using UnityEngine;

public class SpellManager : NetworkBehaviour
{
    public NetworkVariable<bool> spellCasted = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    [SerializeField] private GameObject fireballPrefab;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner && spellCasted.Value == true)
        {
            spellCasting();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && IsOwner)
        {
            InstantiateFireball();
        }
    }

    private void spellCasting()
    {
        Debug.Log("callback");
        InstantiateFireball();
        spellCasted.Value = false;
    }

    private void InstantiateFireball()
    {
        GameObject fireballObject = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        if (fireballObject.TryGetComponent(out testSpell fireball))
        {
            fireball.networkedOwner = transform;
            spellCasted.Value = true;
        }
    }
}
