using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MetaColor : MonoBehaviour
{

    public int vidasIb = 3;

    private GameObject vidaIb1, vidaIb2, vidaIb3;

    Vector2 pos0;
    Vector2 pos1;
    Vector2 pos2;

    private string sceneName;

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

        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (sceneName == "BaileIb") pos0 = new Vector2 (-7.05f, 11.77f);

        else if (sceneName == "BaileYakuza") pos0 = new Vector2(2.89f, -2.99f);

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
                if (sceneName == "BaileIb") transform.position = new Vector2(7.92f, -7.98f);
                else if (sceneName == "BaileYakuza") transform.position = new Vector2(-12.06f, 6.89f);
                break;

            case 1:
                vidaIb2.SetActive(false);
                if (sceneName == "BaileIb") transform.position = new Vector2(-7.05f, 1.92f);
                else if (sceneName == "BaileYakuza") transform.position = new Vector2(7.79f, -3.03f);
                break;

            case 0:
                vidaIb1.SetActive(false);
                this.gameObject.SetActive(false);
                SceneManager.LoadScene("Almacen");
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
