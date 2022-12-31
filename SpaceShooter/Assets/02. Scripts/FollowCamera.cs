using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform targetTr;

    [Range(2f, 20f)]
    public float distance = 10f;

    [Range(0f, 10f)]
    public float height = 2f;

    public float damping = 10f;
    public float target_offset = 2f;

    Transform camTr;
    Vector3 velocity = Vector3.zero;

    void Start()
    {
        camTr = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        Vector3 pos = targetTr.position + -targetTr.forward * distance + Vector3.up * height;

        //camTr.position = Vector3.Slerp(camTr.position,
        //                               pos,
        //                               Time.deltaTime * damping);

        camTr.position = Vector3.SmoothDamp(camTr.position,
                                            pos,
                                            ref velocity,
                                            damping);

        camTr.LookAt(targetTr.position + (targetTr.up * target_offset));
    }
}
