using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaColor : MonoBehaviour
{

    public int vidasIb = 3;

    public Image vidaIb1, vidaIb2, vidaIb3;

    // Start is called before the first frame update
    void Start()
    {
        vidaIb1 = GameObject.Find("vidaIb1").GetComponent<Image>();
        vidaIb2 = GameObject.Find("vidaIb2").GetComponent<Image>();
        vidaIb3 = GameObject.Find("vidaIb3").GetComponent<Image>();

        vidaIb1.gameObject.SetActive(true);
        vidaIb2.gameObject.SetActive(true);
        vidaIb3.gameObject.SetActive(true);
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
                vidaIb3.gameObject.SetActive(false);
                break;

            case 1:
                vidaIb2.gameObject.SetActive(false);
                break;

            case 0:
                vidaIb1.gameObject.SetActive(false);
                break;
        }
    }

    private void OnCollisionEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Trhugutfugtur");
            cambiarVidaIb();

        }
    }
}
