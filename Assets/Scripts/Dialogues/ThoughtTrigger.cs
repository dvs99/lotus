using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThoughtTrigger : MonoBehaviour
{
    [SerializeField] private Transform dialogue = null;
    [SerializeField] private bool runOnlyOnce = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.StartDialogue(dialogue,onDialogueEnded);
        }
    }


    private void onDialogueEnded()
    {
        if (runOnlyOnce)
            Destroy(gameObject);
    }
}
