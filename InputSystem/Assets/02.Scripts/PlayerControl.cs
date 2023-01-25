using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    Animator anim;
    new Transform transform;
    Vector3 move_dir;

    PlayerInput player_input;
    InputActionMap main_action_map;
    InputAction move_action;
    InputAction attack_action;

    void Start()
    {
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        player_input = GetComponent<PlayerInput>();

        main_action_map = player_input.actions.FindActionMap("PlayerActions");

        move_action = main_action_map.FindAction("Move");
        attack_action = main_action_map.FindAction("Attack");

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

        attack_action.performed += ctx =>
        {
            Debug.Log("Attack by C# event");

            anim.SetTrigger("Attack");
        };
    }

    void Update()
    {
        if(move_dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(move_dir);
            transform.Translate(Vector3.forward * Time.deltaTime * 4);
        }
    }

    #region SendMessage
    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();

        move_dir = new Vector3(dir.x, 0, dir.y);

        anim.SetFloat("Movement", dir.magnitude);
        Debug.Log($"Move = ({dir.x}, {dir.y})");
    }

    void OnAttack()
    {
        Debug.Log("Attack");
        anim.SetTrigger("Attack");
    }
    #endregion

    #region UnityEvents
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();

        move_dir = new Vector3(dir.x, 0, dir.y);

        anim.SetFloat("Movement", dir.magnitude);
        Debug.Log($"Move = ({dir.x}, {dir.y})");
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx.phase={ctx.phase}");

        if(ctx.performed == true)
        {
            Debug.Log("Attack");
            anim.SetTrigger("Attack");
        }
    }
    #endregion
}
