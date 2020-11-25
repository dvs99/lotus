using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField]
    private DialogueBox box;

    private Dialogue activeDialogue=null;
    private int dialogueParseIndex = -1;
    private PlayerInput pInput;
    private string prevActionMap;
    private bool dialogueEnded;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
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
                        box.DisplayNewText(activeDialogue.Lines[dialogueParseIndex].Substring(3),false);
                        Debug.Log("MODE LC" + dialogueParseIndex +" - " + activeDialogue.Lines[dialogueParseIndex]);
                        break;

                    case "-rc": //right talks
                        box.DisplayNewText(activeDialogue.Lines[dialogueParseIndex].Substring(3), true);
                        Debug.Log("MODE RC" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        break;

                    case "-ss": //load a scene single mode & close
                        Debug.Log("MODE SS" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        sceneLoad(LoadSceneMode.Single);
                        endDialogue();
                        break;

                    case "-sa": //load a scene additive mode & close
                        Debug.Log("MODE SA" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        sceneLoad(LoadSceneMode.Additive);
                        endDialogue();
                        break;

                    case "-nd": //set next dialogue & close
                        Debug.Log("MODE ND" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        setNextDialogue();
                        endDialogue();
                        break;

                    case "-rl": //run random line and set to be closed
                        Debug.Log("MODE RL" + dialogueParseIndex + " - " + activeDialogue.Lines[dialogueParseIndex]);
                        dialogueParseIndex = Random.Range(dialogueParseIndex, activeDialogue.Lines.Length - 2);
                        OnSubmit();
                        dialogueEnded = true;
                        break;

                    default:
                        Debug.LogError("Dialogue formatting is incorrect");
                        return;
                }
            }
        }
    }


    public void startDialogue(Transform dialogue)
    {
        if (activeDialogue == null)
        {
            activeDialogue = dialogue.GetComponent<Dialogue>();
            if (activeDialogue == null)
            {
                foreach (Transform child in dialogue)
                {
                    activeDialogue = activeDialogue.GetComponent<Dialogue>();
                    if (activeDialogue.enabled == false)
                        activeDialogue = null;
                }
                
                if (activeDialogue == null)
                {
                    Debug.LogError("There is no enabled dialogue in the provided gameObject or its children");
                    return;
                }
            }

            box.Enable(activeDialogue.LCharacter, activeDialogue.RCharacter);
            OnSubmit();

            prevActionMap = pInput.currentActionMap.name;
            pInput.SwitchCurrentActionMap("UI");
        }
        else
            Debug.LogWarning("There is already an open dialogue");

    }

    private void endDialogue()
    {
        dialogueParseIndex = -1;
        activeDialogue = null;
        pInput.SwitchCurrentActionMap(prevActionMap);

        box.Disable();
    }

    private void sceneLoad(LoadSceneMode mode)
    {
        string scene = activeDialogue.Lines[dialogueParseIndex].Substring(3);

        dialogueParseIndex++;
        if (activeDialogue.Lines[dialogueParseIndex] != null && activeDialogue.Lines[dialogueParseIndex] == "-nd")
            setNextDialogue();

        SceneManager.LoadScene(scene, mode);
    }

    private void setNextDialogue()
    {
        activeDialogue.nextDialogue.enabled = true;
        activeDialogue.enabled = false;
    }
}
