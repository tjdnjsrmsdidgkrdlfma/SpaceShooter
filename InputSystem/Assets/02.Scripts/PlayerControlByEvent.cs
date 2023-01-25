using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlByEvent : MonoBehaviour
{
    InputAction move_action;
    InputAction attack_action;

    Animator anim;
    Vector3 move_dir;

    void Start()
    {
        anim = GetComponent<Animator>();

        move_action = new InputAction("Move", InputActionType.Value);

        move_action.AddCompositeBinding("2DVector")
        .With("Up", "<Keyboard>/w")
        .With("Down", "<Keyboard>/s")
        .With("Left", "<Keyboard>/a")
        .With("Right", "<Keyboard>/d");

        move_action.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            move_dir = new Vector3(dir.x, 0, dir.y);

            anim.SetFloat("Movement", dir.magnitude);
        };

        move_action.canceled += ctx =>
        {
            move_dir = Vector3.zero;

            anim.SetFloat("Movement", 0);
        };

        move_action.Enable();

        attack_action = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");

        attack_action.performed += ctx =>
        {
            anim.SetTrigger("Attack");
        };

        attack_action.Enable();
    }

    void Update()
    {
        if(move_dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(move_dir);
            transform.Translate(Vector3.forward * Time.deltaTime * 4);
        }
    }
}
