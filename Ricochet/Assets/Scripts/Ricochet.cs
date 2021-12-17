using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 velocity;
    public int numRicochets;
    public int damage;
    public float damageRange;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy"))
        {
            DealDamage();
            
        }

        if (numRicochets == 0)
        {
            Destroy(gameObject);
        }
        float speed = velocity.magnitude;
        Vector3 direction = Vector3.Reflect(velocity.normalized, collision.contacts[0].normal);

        rb.velocity = direction * speed;
        numRicochets -= 1;
    }

    private void DealDamage()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, damageRange);

        foreach(Collider enemy in enemies)
        {
            if (enemy.CompareTag("Player"))
            {
                Debug.Log("Damaged");
                enemy.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }
}
