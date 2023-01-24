using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireControl : MonoBehaviour
{
    public GameObject bullet;
    public Transform fire_pos;
    public AudioClip fire_sfx;

    new AudioSource audio;
    MeshRenderer muzzle_flash;

    RaycastHit hit;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        muzzle_flash = fire_pos.GetComponentInChildren<MeshRenderer>();
        muzzle_flash.enabled = false;
    }

    void Update()
    {
        Debug.DrawRay(fire_pos.position, fire_pos.forward * 10, Color.green);

        if(Input.GetMouseButtonDown(0))
        {
            Fire();

            if (Physics.Raycast(fire_pos.position,
                                fire_pos.forward,
                                out hit,
                                10,
                                1 << 6))
            {
                Debug.Log($"Hit={hit.transform.name}");

                hit.transform.GetComponent<MonsterControl>()?.OnDamage(hit.point, hit.normal);
            }
        }
    }

    void Fire()
    {
        Instantiate(bullet, fire_pos.position, fire_pos.rotation);

        audio.PlayOneShot(fire_sfx, 1);

        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;

        muzzle_flash.material.mainTextureOffset = offset;

        float angle = Random.Range(0, 360);
        muzzle_flash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        float scale = Random.Range(1f, 2f);
        muzzle_flash.transform.localScale = Vector3.one * scale;

        muzzle_flash.enabled = true;

        yield return new WaitForSeconds(0.2f);

        muzzle_flash.enabled = false;
    }
}
