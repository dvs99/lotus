using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowPlayer : Interactable
{
    public float speed;
    private Transform target;
    private bool follow;
    private SimpleController_UsingPlayerInput jugador;
    private Animator anim;
    Vector2 quieto;





    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleController_UsingPlayerInput>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        quieto = new Vector2 (0, 0);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (playerInRange)
        {
            follow = true;
        }
        if (follow)
        {
            if (jugador.devolverVelocidad() == quieto)
            {
                anim.Play("Idle");
            }
            else if (jugador.devolverVelocidad() != quieto)
            {
                anim.Play("Walk");
            }
            if (Vector2.Distance(transform.position, target.position) > 0.8)
            {
                Vector2 direccion = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                
                transform.position = direccion;
               
                anim.SetFloat("MoveX", target.position.x - transform.position.x);
                anim.SetFloat("MoveY", target.position.y - transform.position.y);
            }
        }
        else
        {
            anim.Play("Idle");
        }
    }


}
