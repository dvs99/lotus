using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class DetectarPalabra : MonoBehaviour
{

    public GameObject palabra;
    public GameObject informacion;
    public GameObject inputField;
    public GameObject pensamiento;
    private string pal;
    private AudioSource audioPlayer;

    public AudioClip error;
    public AudioClip acierto;
  
    private PlayerInput pInput;


    // Start is called before the first frame update
    void Start()
    {
        pInput = Input.Instance.GetComponent<PlayerInput>();

       
        palabra.SetActive(false);
        informacion.SetActive(false);
        pensamiento.SetActive(true);
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Read()
    {
        palabra.SetActive(true);
    
    }

    public void Atras()
    {
        palabra.SetActive(false);
        informacion.SetActive(false);

    }

    public void Aceptar()
    {
        pal = inputField.GetComponent<Text>().text;
        Debug.Log(pal);
        if (pal == "feto" || pal == "Feto")
        {
            informacion.SetActive(true);
            palabra.SetActive(false);
            pensamiento.SetActive(false);
            audioPlayer.clip = acierto;
            audioPlayer.Play();
        }
        else
        {
            audioPlayer.clip = error;
            audioPlayer.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string actionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("Player");

        pInput.currentActionMap.FindAction("Interact").performed += ctx => Read();

        pInput.SwitchCurrentActionMap(actionMap);
    }


   
  
}
