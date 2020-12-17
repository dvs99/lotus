using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowPlayer : Interactable
{
    public float speed;
    private Transform target;
    private bool follow;
    //private Animator anim;

    //private Vector3 directionVector;



    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
            if (Vector2.Distance(transform.position, target.position) > 0.5)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                
            }
        }
    }


}
