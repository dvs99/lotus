using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class FueraPalabra : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject palabra;

    private PlayerInput pInput;

    // Start is called before the first frame update
    void Start()
    {
        pInput = Input.Instance.GetComponent<PlayerInput>();


        palabra.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string actionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("Player");

        pInput.currentActionMap.FindAction("Interact").performed += ctx => Cancelar();

        pInput.SwitchCurrentActionMap(actionMap);
    }

    private void Cancelar()
    {
        palabra.SetActive(false);
    }
}
