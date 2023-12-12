using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class fireball : SpellComponent
{
    public SpellManager parent;
    public float speed;
    public float amplitude = 2f;
    public float lifeTime = 3f;
    private Vector3 moveDirection;

    void Start()
    {
        speed = 2f;
        StartCoroutine(DestroyAfterDelay());
    }

    void Update()
    {
        Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;
        transform.position = newPosition;

        /*if(getOwner() != null)
        {
            Debug.Log("Has Owner");
        }*/
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        //if (!IsOwner) return;
        Debug.Log("collision");
        if (other.CompareTag("Player"))
        {
            NetworkObject networkObject = other.GetComponent<NetworkObject>();

            Debug.Log("Hit Player" + networkObject.OwnerClientId);
            parent.DestroyServerRpc();
            StopCoroutine(DestroyAfterDelay());
        }

        if (other.CompareTag("Wall")) 
        {
            parent.DestroyServerRpc();
            WallSpell wall = other.GetComponent<WallSpell>();
            Debug.Log("Hit Wall");

            wall.spellsTanked = wall.spellsTanked + 1;
        }
        
    }*/

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(lifeTime);

        // Check if the parent object is still networked before calling DestroyServerRpc
        if (parent != null && parent.NetworkObject != null && parent.NetworkObject.IsSpawned)
        {
            parent.DestroyServerRpc();
        }
    }

    public void DoImpact()
    {
        parent.DestroyServerRpc();
        Debug.Log("Fireball Deleted");
    }
}
