using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallComponent : UnNetworkedSpellComponent
{
    public float lifeTime = 5f;
    public GameObject explodePrefab;
    public int wallHealth = 2;
    public elementType elementtype;

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Instantiate(explodePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void DoDamage()
    {
        wallHealth = wallHealth - 1;
        if(wallHealth < 0)
        {
            Destroy(gameObject);
        }
    }

    // Logic for handling spell impacts remains unchanged
}

