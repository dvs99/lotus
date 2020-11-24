using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerControllerBaile : MonoBehaviour
{

    public float speed = 0;

    public Image vida1, vida2, vida3;

    private Rigidbody2D rb;
    public int vidas = 3;
    

    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        vida1 = GameObject.Find("vida1").GetComponent<Image>();
        vida2 = GameObject.Find("vida2").GetComponent<Image>();
        vida3 = GameObject.Find("vida3").GetComponent<Image>();

        vida1.gameObject.SetActive(true);
        vida2.gameObject.SetActive(true);
        vida3.gameObject.SetActive(true);

        /*vidaIb1 = GameObject.Find("vidaIb1").GetComponent<Image>();
        vidaIb2 = GameObject.Find("vidaIb2").GetComponent<Image>();
        vidaIb3 = GameObject.Find("vidaIb3").GetComponent<Image>();

        vidaIb1.gameObject.SetActive(true);
        vidaIb2.gameObject.SetActive(true);
        vidaIb3.gameObject.SetActive(true);*/
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(movementX, movementY);

        rb.AddForce(movement * speed);


    }

    public void cambiarVida()
    {
        vidas--;

        switch (vidas)
        {
            case 2:
                vida3.gameObject.SetActive(false);
                break;

            case 1:
                vida2.gameObject.SetActive(false);
                break;

            case 0:
                vida1.gameObject.SetActive(false);
                break;
        }
    }

  

    private void OnCollisionEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemigo"))
        {
            Debug.Log("Trigger");
            cambiarVida();
        }

        /*else if(collision.CompareTag("Meta"))
        {
            cambiarVidaIb();
        }*/
       
    }
}
