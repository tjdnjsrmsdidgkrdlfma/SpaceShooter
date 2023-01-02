using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelControl : MonoBehaviour
{
    public GameObject exp_effect;
    public Texture[] textures;
    public float radius = 10;

    Transform tr;
    Rigidbody rb;
    MeshRenderer render;

    int hit_count = 0;

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        render = GetComponentInChildren<MeshRenderer>();

        int idx = Random.Range(0, textures.Length);

        render.material.mainTexture = textures[idx];
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Bullet") == true)
        {
            if (++hit_count == 3)
                ExpBarrel();
        }
    }

    void ExpBarrel()
    {
        GameObject exp = Instantiate(exp_effect, tr.position, Quaternion.identity);

        Destroy(exp, 0.5f);

        //rb.mass = 1.0f;

        //rb.AddForce(Vector3.up * 1500);

        IndirectDamage(tr.position);

        Destroy(gameObject, 3);
    }

    void IndirectDamage(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        foreach(var coll in colls)
        {
            rb = coll.GetComponent<Rigidbody>();

            rb.mass = 1;

            rb.constraints = RigidbodyConstraints.None;

            rb.AddExplosionForce(1500, pos, radius, 1200);
        }
    }
}
