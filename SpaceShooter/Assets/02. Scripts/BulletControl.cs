using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float damage = 20f;
    public float force = 1500f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * force);
    }

    void Update()
    {
        
    }
}
