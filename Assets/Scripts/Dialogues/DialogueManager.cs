using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField]
    private DialogueBox box=null;

    private Dialogue activeDialogue=null;
    private int dialogueParseIndex = -1;
    private PlayerInput pInput;
    private string prevActionMap;
    private bool dialogueEnded = false;
    private Action callback=null;
    private Regex regex;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        regex = new Regex(@"\[\d+\]");

        pInput = Input.Instance.GetComponent<PlayerInput>();

        //Subscribe the input callback functions to the corresponding input events
        string actionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("UI");

        pInput.currentActionMap.FindAction("Submit").performed += ctx => OnSubmit(ctx);

        pInput.SwitchCurrentActionMap(actionMap);
    }


    private void OnSubmit(InputAction.CallbackContext context)
    {
        OnSubmit();
    }

    private void OnSubmit()
    {
        if (activeDialogue != null)
        {

            if (box.DisplayNext())
            {
                return;
            }
            else if (dialogueParseIndex >= activeDialogue.Lines.Length - 1 || dialogueEnded)
            {
                endDialogue();
                return;
            }
            else
            {
                dialogueParseIndex++;
                switch (activeDialogue.Lines[dialogueParseIndex].Substring(0, 3))
                {
                    case "-lc": //left talks
                        if (activeDialogue.Lines[dialogueParseIndex][3] == '<' && activeDialogue.Lines[dialogueParseIndex][5] == '>')
                        {
                            box.DisplayNewText(activeDialogue.Lines[dialogueParseIndex].Substring(6), false, getEmotion(activeDialogue.Lines[dialogueParseIndex][4]), activeDialogue.LName);
                        }
                        else
                        {
                            //set emotion
                            box.DisplayNewText(activeDialogue.Lines[dialogueParseIndex].Substring(3), false, Emotion.Neutral, activeDialogue.LName);
                        }
                        //Debug.Log("MODE LC" + dialogueParseIndex +" - " + activeDialogue.Lines[dialogueParseIndex]);
                        break;

                    case "-rc": //right talks
                        if (activeDialogue.Lines[dialogueParseIndex][3] == '<' && activeDialogue.Lines[dialogueParseIndex][5] == '>')
                        {
                            //set emotion
                            box.DisplayNewText(activeDialogue.Lines[dialogueParseIndex].Substring(6), true, getEmotion(activeDialogue.Lines[dialogueParseIndex][4]), activeDialogue.RName);
                        }
                        else
                        {
                            //set emotion
                            box.DisplayNewText(activeDialogue.Lines[dialogueParseIndex].Substring(3), true, Emotion.Neutral, activeDialogue.RName);
                        }
                        //Debug.Log("MODE RC" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        break;

                    case "-ss": //load a scene single mode & close
                        //Debug.Log("MODE SS" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        sceneLoad(LoadSceneMode.Single);
                        break;

                    case "-sa": //load a scene additive mode & close
                        //Debug.Log("MODE SA" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        sceneLoad(LoadSceneMode.Additive);
                        break;

                    case "-nd": //set next dialogue & close
                        //Debug.Log("MODE ND" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        setNextDialogue();
                        endDialogue();
                        break;

                    case "-rl": //run random line and set to be closed
                        //Debug.Log("MODE RL" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        dialogueParseIndex = UnityEngine.Random.Range(dialogueParseIndex, activeDialogue.Lines.Length - 2);
                        OnSubmit();
                        dialogueEnded = true;
                        break;
                    default: //check for interactions with other gameobjects
                        Match match = regex.Match(activeDialogue.Lines[dialogueParseIndex]);
                        if (match.Success)
                        {
                            int index = Convert.ToInt32(match.Value.Substring(1, match.Value.Length - 2));
                            if (index >= activeDialogue.interactedGameObjects.Length)
                            {
                                Debug.LogWarning("Game object doesn't exist at index " + index + " of interactedGameObjects");
                            }
                            else
                                switch (activeDialogue.Lines[dialogueParseIndex].Substring(match.Value.Length,3))
                                {
                                    //EXPLICAR EN TOOLTIP

                                    case "-do": //disable gameobject
                                        activeDialogue.interactedGameObjects[index].SetActive(false);
                                        break;

                                    case "-eo": //enable gameobect
                                        activeDialogue.interactedGameObjects[index].SetActive(true);
                                        break;
                                    case "-nd": //change the dialogue in the game object to the next dialogue
                                        Dialogue otherDialogue = activeDialogue.interactedGameObjects[index].GetComponent<Dialogue>();
                                        if (otherDialogue == null)
                                        {
                                            foreach (Transform child in activeDialogue.interactedGameObjects[index].transform)
                                            {
                                                otherDialogue = child.GetComponent<Dialogue>();
                                                if (otherDialogue != null)
                                                    if (!otherDialogue.activated)
                                                        otherDialogue = null;
                                                    else { //dialogue found{
                                                        setNextDialogue(otherDialogue);
                                                        break; }
                                            }

                                            if (otherDialogue == null)
                                            {
                                                Debug.LogWarning("There is no activated dialogue at index " + index + " of interactedGameObjects or its children");
                                                return;
                                            }
                                        }
                                        break;
                                }
                            OnSubmit();
                        }
                        else
                            Debug.LogError("Dialogue formatting is incorrect");
                        break;
                }
            }
        }
        else
            endDialogue();
    }

    private Emotion getEmotion(char emotionChar)
    {
        if (emotionChar == 'h')
            return Emotion.Happy;
        if (emotionChar == 's')
            return Emotion.Sad;
        if (emotionChar == 'a')
            return Emotion.Angry;
        if (emotionChar == 'n')
            return Emotion.Neutral;
        Debug.LogError("Attempted to use unexisting emotion. Defaulted to happy.");
        return Emotion.Happy;
    }

    public void StartDialogue(Transform dialogue, Action cb)
    {
        callback = cb;
        StartDialogue(dialogue);
    }

    public void StartDialogue(Transform dialogue)
    {
        if (activeDialogue == null)
        {
            activeDialogue = dialogue.GetComponent<Dialogue>();
            if (activeDialogue == null)
            {
                foreach (Transform child in dialogue)
                {
                    activeDialogue = child.GetComponent<Dialogue>();
                    if (activeDialogue != null)
                        if (!activeDialogue.activated)
                            activeDialogue = null;
                        else //dialogue found
                            break;
                }
                
                if (activeDialogue == null)
                {
                    Debug.Log("There is no activated dialogue in the provided gameObject or its children, no dialogue started");
                    return;
                }
            }

            dialogueEnded = false;
            prevActionMap = pInput.currentActionMap.name;
            pInput.SwitchCurrentActionMap("UI");

            box.Enable(activeDialogue.LCharacter, activeDialogue.RCharacter);
            OnSubmit();


        }
        else
            Debug.LogWarning("There is already an open dialogue");

    }

    private void endDialogue()
    {
        dialogueParseIndex = -1;
        activeDialogue = null;

        box.Disable();
        callback?.Invoke();
        callback = null;
        pInput.SwitchCurrentActionMap(prevActionMap);
    }

    private void sceneLoad(LoadSceneMode mode)
    {
        string scene = activeDialogue.Lines[dialogueParseIndex].Substring(3);

        dialogueParseIndex++;
        if (activeDialogue.Lines.Length < dialogueParseIndex && activeDialogue.Lines[dialogueParseIndex] == "-nd")
            setNextDialogue();

        endDialogue();
        SceneManager.LoadScene(scene, mode);
    }

    private void setNextDialogue()
    {
        setNextDialogue(activeDialogue);
    }
    private void setNextDialogue(Dialogue d)
    {
        if (d.nextDialogue != null)
            d.nextDialogue.activated = true;
        d.activated = false;
    }

}
