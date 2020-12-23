using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public GameObject end;

    private void Start()
    {
        end.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            end.SetActive(true);
            other.gameObject.SetActive(false);
            Camera.current.GetComponent<AudioListener>().enabled = false;
        }
    }
}
