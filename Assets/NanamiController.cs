using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class NanamiController : MonoBehaviour
{
    public float speed = 3.0f;
    Rigidbody2D rigidbody2d;

    private void Move()
    {
        Debug.Log("hola");
        // if (direction.sqrMagnitude < 0.01)
        //     return;
        // var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // // direction according to world Y rotation of player.
        // var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        // transform.position += move * scaledMoveSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
