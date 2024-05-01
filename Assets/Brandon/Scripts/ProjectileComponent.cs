using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : UnNetworkedSpellComponent
{
    // Start is called before the first frame update
    public elementType elementtype;
    public float speed;
    public float maxspeed;
    public float lifeTime = 3f;
    public float maxlifeTime;
    private Vector3 moveDirection;
    public GameObject destroyPrefab;
    public UnNetworkedSpellManager parent;

    void Start()
    {
        speed = maxspeed;
        lifeTime = maxlifeTime;
    }

    void Update()
    {
        // Movement logic remains unchanged
        Debug.Log("In movemnet");
        transform.position += moveDirection * speed * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            DoImpact();
        }
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    public void DoImpact()
    {
        //Instantiate(destroyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
