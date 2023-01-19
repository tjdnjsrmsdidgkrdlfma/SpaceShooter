using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float move_speed = 10f;
    public float turn_speed = 80f;

    readonly float init_hp = 100;
    public float curr_hp;

    Transform tr;
    Animation anim;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        curr_hp = init_hp;

        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        anim.Play("Idle");

        turn_speed = 0;
        yield return new WaitForSeconds(0.3f);
        turn_speed = 80;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 move_dir = Vector3.forward * v + Vector3.right * h;

        tr.Translate(move_dir.normalized * move_speed * Time.deltaTime);

        tr.Rotate(Vector3.up * turn_speed * Time.deltaTime * r);

        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        if (v >= 0.1f)
            anim.CrossFade("RunF", 0.25f);
        else if (v <= -0.1f)
            anim.CrossFade("RunB", 0.25f);
        else if (h >= 0.1f)
            anim.CrossFade("RunR", 0.25f);
        else if (h <= -0.1f)
            anim.CrossFade("RunL", 0.25f);
        else
            anim.CrossFade("Idle", 0.25f);
    }

    void OnTriggerEnter(Collider other)
    {
        if(curr_hp >= 0 && other.CompareTag("Punch"))
        {
            curr_hp -= 10;
            Debug.Log($"Player hp = {curr_hp / init_hp}");

            if(curr_hp <= 0)
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player Die!");

        OnPlayerDie();
    }
}