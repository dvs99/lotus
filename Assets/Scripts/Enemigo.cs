using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{

    //private float speed = 15.0f;
    private bool vertical;
    public float changeTime = 3.0f;
    public List<Transform> tail;

    private new Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lastPos = transform.position;
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
            vertical = Random.Range(0, 2) == 0;
        }

        Vector2 position = rigidbody2D.position;
        //Vector2 temp = rigidbody2D.position;

        

        if (vertical)
        {
            //lastPos.y = position.y + Time.deltaTime * speed * direction;
            position.y = position.y + Time.deltaTime * direction * 4.85f;
        }

        else
        {
            //lastPos.x = position.x + Time.deltaTime * speed * direction;
            position.x = position.x + Time.deltaTime * direction * 4.85f;
        }

        /*for (int i = 0; i < tail.Count; i++)
        {
            temp = tail[i].position;
            tail[i].position = lastPos;
            lastPos = temp;
        }*/

        //rigidbody2D.MovePosition(lastPos);
        rigidbody2D.MovePosition(position);
        


        

    }

}
