using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PlayerControllerBaile : MonoBehaviour
{

    private float speed = 10;
    private Vector2 m_Move;

    private PlayerInput pInput;

    private GameObject vida1, vida2, vida3;

    private Rigidbody2D rb;

    public int vidas = 3;

    //private float movementX;
    //private float movementY;

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    private void Awake()
    {
        vida1 = GameObject.Find("Vida1");
        vida2 = GameObject.Find("Vida2");
        vida3 = GameObject.Find("Vida3");

        pInput = GetComponent<PlayerInput>();

        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        vida1.SetActive(true);
        vida2.SetActive(true);
        vida3.SetActive(true);

        //Subscribe the input callback functions to the corresponding input events
        string actionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("Puzle");

        pInput.currentActionMap.FindAction("Move").performed += ctx => OnMove(ctx);
        pInput.currentActionMap.FindAction("Move").canceled += ctx => OnMove(ctx);

        pInput.SwitchCurrentActionMap(actionMap);
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(m_Move);
        //Vector2 movement = new Vector2(movementX, movementY);
        //rb.AddForce(movement * speed);


    }

    /*private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }*/

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = speed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, direction.y, 0);

        rb.MovePosition(transform.position + move * scaledMoveSpeed);

    }

    public void cambiarVida()
    {
        vidas--;

        switch (vidas)
        {
            case 2:
                vida3.SetActive(false);
                break;

            case 1:
                vida2.SetActive(false);
                break;

            case 0:
                vida1.SetActive(false);
                SceneManager.LoadScene("Almacen", LoadSceneMode.Single);
                break;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            //Debug.Log("Trigger");
            cambiarVida();
        }
       
    }
}
