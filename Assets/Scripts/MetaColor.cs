using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaColor : MonoBehaviour
{

    public int vidasIb = 3;

    private GameObject vidaIb1, vidaIb2, vidaIb3;

    Vector2 pos0;
    Vector2 pos1;
    Vector2 pos2;

    //private GameObject meta;

    // Start is called before the first frame update
    void Start()
    {
        vidaIb1 = GameObject.Find("VidaIb1");
        vidaIb2 = GameObject.Find("VidaIb2");
        vidaIb3 = GameObject.Find("VidaIb3");

        vidaIb1.SetActive(true);
        vidaIb2.SetActive(true);
        vidaIb3.SetActive(true);

        this.gameObject.SetActive(true);

        pos0 = new Vector2 (-7.04f, 11.7f);

        transform.position = pos0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cambiarVidaIb()
    {
        vidasIb--;

        switch (vidasIb)
        {
            case 2:
                vidaIb3.SetActive(false);
                transform.position = new Vector2(12.8f, -2.98f);
                break;

            case 1:
                vidaIb2.SetActive(false);
                transform.position = new Vector2(-7.04f, -7.94f);
                break;

            case 0:
                vidaIb1.SetActive(false);
                this.gameObject.SetActive(false);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            
            cambiarVidaIb();

        }
    }
}
