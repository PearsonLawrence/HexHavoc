using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class fireball : SpellComponent
{
    public SpellManager parent;
    public float speed;
    public float maxspeed;
    public float amplitude = 2f;
    public float lifeTime = 3f;
    public float maxlifeTime;
    private Vector3 moveDirection;
    public GameObject destroyPrefab;
    void Start()
    {
        speed = maxspeed;
        lifeTime = maxlifeTime;
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
