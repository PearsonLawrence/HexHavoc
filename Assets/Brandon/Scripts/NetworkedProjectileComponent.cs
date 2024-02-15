//Auhtor: Brandon(Ri) Yu
//purpose: This is the the script that is attached onto the fireball gameobject. It gives the firball its logic
//like its movement and despawning

using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public enum elementType
{
    WIND,
    FIRE,
    WATER,
    EARTH
}

public class NetworkedProjectileComponent : SpellComponent
{
    public elementType elementtype;
    public SpellManager parent;
    public float speed;
    public float maxspeed;
    public float amplitude = 2f;
    public float lifeTime = 3f;
    public float maxlifeTime;
    private Vector3 moveDirection;
    public GameObject destroyPrefab;

    private bool wentThroughWall;

    

    public void SetWentThroughWall(bool newVal)
    {
        wentThroughWall = newVal;
    }

    public bool GetWentThroughWall()
    {
        return wentThroughWall;
    }

    void Start()
    {
        speed = maxspeed;
        lifeTime = maxlifeTime;

        switch (elementtype)
        {
            case elementType.WIND:
                break;
            case elementType.FIRE:
                break;
            case elementType .WATER:
                break;
            case elementType .EARTH:
                break;
        }
    }

    //update the fireballs position in launch direction
    void Update()
    {
        Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;
        transform.position = newPosition;

        lifeTime -= Time.deltaTime;
        //delete spell after certain amount of time
        if(lifeTime < 0)
        {
            DoImpact();
        }
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    //instantiate particle effect and despawn from network
    public void DoImpact()
    {
        GameObject temp =  Instantiate(destroyPrefab, transform.position, Quaternion.identity);
        Destroy(temp, 3);

        GetComponent<NetworkObject>().Despawn();

        Destroy(gameObject);
    }
}
