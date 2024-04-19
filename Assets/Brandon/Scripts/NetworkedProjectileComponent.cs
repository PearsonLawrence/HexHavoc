//Auhtor: Brandon(Ri) Yu
//purpose: This is the the script that is attached onto the fireball gameobject. It gives the firball its logic
//like its movement and despawning

using System.Collections;
using System.Xml.Linq;
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
    //information about spell
    public elementType elementtype;
    public SpellManager parent;


    //varibles for all spells
    public float speed;
    public float maxspeed;
    public float amplitude = 2f;
    public float lifeTime = 3f;
    public float maxlifeTime;
    private Vector3 moveDirection;


    //for deleteing
    public GameObject destroyPrefab;


    //varibles for different spell types
    public bool fireWentThroughWall;
    public bool waterWallThroughWall;
    public bool dontMove = true;

    public void SetWentThroughWall(bool newVal, elementType element)
    {
        switch (element)
        {
            case elementType.FIRE:
                fireWentThroughWall = newVal;
                break;
            case elementType.WATER:
                waterWallThroughWall = newVal;
                break;
        }
    }

    public bool GetWentThroughWall(elementType elemnt)
    {
        switch (elemnt)
        {
            case elementType.FIRE:
                return fireWentThroughWall;

            case elementType.WATER:
                return waterWallThroughWall;

        }
        return false;
    }

    void Start()
    {
        AudioManager audioManager = AudioManager.Instance;

        speed = maxspeed;
        lifeTime = maxlifeTime;
        //Debug.Log(earthShot.Value);
    }

    //update the fireballs position in launch direction
    void Update()
    {
        if (!dontMove)
        {
            switch (elementtype)
            {
                case elementType.WIND:

                    Vector3 windNewPosition = transform.position + moveDirection * speed * Time.deltaTime;
                    transform.position = windNewPosition;

                    lifeTime -= Time.deltaTime;

                    if (lifeTime < 0)
                    {
                        DoImpact();
                    }
                    break;

                case elementType.FIRE:

                    Vector3 fireNewPosition = transform.position + moveDirection * speed * Time.deltaTime;
                    transform.position = fireNewPosition;

                    lifeTime -= Time.deltaTime;

                    if (lifeTime < 0)
                    {
                        DoImpact();
                    }
                    break;

                case elementType.WATER:
                    Vector3 waterNewPosition = transform.position + moveDirection * speed * Time.deltaTime;
                    transform.position = waterNewPosition;

                    lifeTime -= Time.deltaTime;

                    if (lifeTime < 0)
                    {
                        DoImpact();
                    }
                    break;

                case elementType.EARTH:

                    Vector3 earthNewPosition = transform.position + moveDirection * speed * Time.deltaTime;
                    transform.position = earthNewPosition;

                    lifeTime -= Time.deltaTime;

                    if (lifeTime < 0)
                    {
                        DoImpact();
                    }
                    break;
            }
        }
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
        dontMove = false;
    }

    //instantiate particle effect and despawn from network
    public void DoImpact()
    {
        GameObject temp = Instantiate(destroyPrefab, transform.position, Quaternion.identity);
        Destroy(temp, 3);
        if (IsServer) 
        {
            GetComponent<NetworkObject>().Despawn();

            Destroy(gameObject);
        }
        else
        {
            return;
        }
        
    }
}
