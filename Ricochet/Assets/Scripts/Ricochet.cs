using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 velocity;
    public int numRicochets;
    public int maxRicochet;
    public int damage;
    public float damageRange;
    public float destructTime = 10;
    private float timer = 0;
    // Start is called before the first frame update
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= destructTime) Destroy(gameObject);
        velocity = rb.velocity;
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
                //Debug.Log("Damaged");
                enemy.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
            if (enemy.CompareTag("Enemy"))
            {
                enemy.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
                enemy.gameObject.GetComponent<EnemyAI>().scoreHit(numRicochets, maxRicochet);
                
            }
        }
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }
}
