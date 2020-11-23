using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class SimpleController_UsingPlayerInput : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    private Vector2 m_Move;

    private Rigidbody2D rb;
    private PlayerInput pInput;
    private Animator animator;

    //---CALLBACKS---
    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    //---UNITY METHODS---
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        pInput = Input.Instance.GetComponent<PlayerInput>();

        //Subscribe the input callback functions to the corresponding input events
        string actionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("Player");

        pInput.currentActionMap.FindAction("Move").performed += ctx => OnMove(ctx);
        pInput.currentActionMap.FindAction("Move").canceled += ctx => OnMove(ctx);

        pInput.SwitchCurrentActionMap(actionMap);

    }

    public void FixedUpdate()
    {
        Move(m_Move);
    }

    //---METHODS---
    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, direction.y,0 );

        rb.MovePosition(transform.position + move * scaledMoveSpeed);
    }
}
