using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    [Tooltip("Start the line with -lc for left character or -rc for right character.\nUse -ss to make the dialogue load a new scene and close the dialogue (you can use -nd in the next line if you need to before the dialogue closes).\nUse -sa to load a new scene in aditive mode and close the dialogue (you can use -nd in the next line if you need to before the dialogue closes).\nUse -nd to end this dialogue and set the next one in this gameObject active.\n" +
        "Use -rl to display a random line of dialogue from this dialogue and set the dialogue to be closed.\nUse [index]-eo to enable a GameObject from interactedGameObjects.\nUse [index]-do to disable a GameObject from interactedGameObjects.\nUse [index]-nd to set active next dialogue in a gameObject from interactedGameObjects.")]
    public string[] Lines;

    public CharacterDisplayHelper LCharacter = null;
    public CharacterDisplayHelper RCharacter = null;

    public string LName = null;
    public string RName = null;

    public Dialogue nextDialogue;

    public GameObject[] interactedGameObjects = null;

    public bool activated=false;
}
