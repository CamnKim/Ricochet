using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 velocity;
    public int numRicochets;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (numRicochets == 0)
        {
            return;
        }
        float speed = velocity.magnitude;
        Vector3 direction = Vector3.Reflect(velocity.normalized, collision.contacts[0].normal);

        rb.velocity = direction * speed;
        numRicochets -= 1;
    }
}
