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
    Vector2 lookDirection = new Vector2(1, 0);

    //---CALLBACKS---
    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();

    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        Interact();
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


        pInput.currentActionMap.FindAction("Interact").performed += ctx => OnInteract(ctx);
        

        pInput.SwitchCurrentActionMap(actionMap);

    }

    public void FixedUpdate()
    {
        Move(m_Move);
    }

    public void Update()
    {
        float horizontal = m_Move.x;
        float vertical = m_Move.y;

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("MoveX", lookDirection.x);
        animator.SetFloat("MoveY", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

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
    private void Interact()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC")); //Tercer Parametro es la longitud del Rayo
        if (hit.collider != null)
        {
            foreach (Transform child in hit.collider.transform)
                if (child.CompareTag("Interactable"))
                    DialogueManager.Instance.StartDialogue(child);
        }
        else {
        //    Debug.Log("Donde estas mirando?");
        }
    }
}
