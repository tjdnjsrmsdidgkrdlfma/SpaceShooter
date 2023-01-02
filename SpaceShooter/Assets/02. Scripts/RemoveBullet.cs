using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject spark_effect;

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Bullet") == true)
        {
            ContactPoint cp = other.GetContact(0);

            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            GameObject spark = Instantiate(spark_effect, cp.point, rot);

            Destroy(spark, 0.5f);

            Destroy(other.gameObject);
        }
    }
}
