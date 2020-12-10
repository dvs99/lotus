using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;


public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Vector2 leftLocation = Vector2.zero;
    [SerializeField] private Vector2 rightLocation = Vector2.zero;
    [SerializeField] private float secondsBetweenChars = 0.1f;
    [SerializeField] private float secondsBetweenCharsFast = 0.001f;


    [SerializeField] private CharacterDisplayHelper lCharacter = null;
    [SerializeField] private CharacterDisplayHelper rCharacter = null;
    [SerializeField] private Sprite defaultBoxImageSprite = null;


    private Image boxImage;
    [SerializeField] private TextMeshProUGUI textBox = null;
    [SerializeField] private TextMeshProUGUI nameBox = null;

    private Coroutine cor;
    private float corWaitTime;

    private PlayerInput pInput;
    private string auxActionMap;

    private void onSubmit(InputAction.CallbackContext context)
    {
        corWaitTime = secondsBetweenCharsFast;
    }

    private void onCancel(InputAction.CallbackContext context)
    {
        corWaitTime = secondsBetweenChars;
    }


    void Start()
    {
        boxImage = GetComponent<Image>();
        corWaitTime = secondsBetweenChars;

        pInput = Input.Instance.GetComponent<PlayerInput>();

        //Subscribe the input callback functions to the corresponding input events
        string actionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("DisplayingText");

        pInput.currentActionMap.FindAction("Submit").performed += ctx => onSubmit(ctx);
        pInput.currentActionMap.FindAction("Submit").canceled += ctx => onCancel(ctx);

        pInput.SwitchCurrentActionMap(actionMap);
    }

    private void rightTalks()
    {
        if (rCharacter != null)
        {
            boxImage.sprite = rCharacter.TextBoxSprite;
            rCharacter.IsTalking = true;
        }
        else
            boxImage.sprite = defaultBoxImageSprite;
        if (lCharacter != null)
        {
            lCharacter.IsTalking = false;
            lCharacter.ActiveEmotion = Emotion.Neutral;
        }
    }

    private void leftTalks()
    {
        if (lCharacter != null)
        {
            boxImage.sprite = lCharacter.TextBoxSprite;
            lCharacter.IsTalking = true;
        }
        else
            boxImage.sprite = defaultBoxImageSprite;
        if (rCharacter != null)
        {
            rCharacter.IsTalking = false;
            lCharacter.ActiveEmotion = Emotion.Neutral;
        }
    }

    public void Enable(CharacterDisplayHelper leftCharacter, CharacterDisplayHelper rightCharacter, bool startWithRightActive = true)
    {   
        auxActionMap = pInput.currentActionMap.name;

        textBox.enabled = true;
        nameBox.enabled = true;
        boxImage.enabled = true;

        lCharacter = leftCharacter;
        rCharacter = rightCharacter;

        if (lCharacter != null)
        {
            lCharacter.GetComponent<RectTransform>().anchoredPosition = leftLocation;
            lCharacter.Show();
        }

        if (rCharacter != null)
        {
            rCharacter.GetComponent<RectTransform>().anchoredPosition = rightLocation;
            rCharacter.Show();
        }

        //set the proper isTalking values for each of the characters
        if (startWithRightActive)
            rightTalks();
        else
            leftTalks();

    }

    public void Disable()
    {
        if (cor != null)
        {
            StopCoroutine(cor);
            pInput.SwitchCurrentActionMap(auxActionMap);
        }

        if (lCharacter != null)
            lCharacter.Hide();
        if (rCharacter != null)
            rCharacter.Hide();
        lCharacter = null;
        rCharacter = null;
        boxImage.enabled = false;
        textBox.enabled = false;
        nameBox.enabled = false;
    }

    public void DisplayNewText(string textToDisplay, bool rightActive = true, Emotion emotion = Emotion.Neutral, string name = "")
    {
        pInput.SwitchCurrentActionMap("DisplayingText");

        textBox.pageToDisplay = 1;

        //set the proper isTalking values, emotion and names for each of the characters
        if (rightActive)
        {
            rightTalks();
            if (rCharacter != null)
                rCharacter.ActiveEmotion = emotion;
        }
        else
        {
            leftTalks();
            if (lCharacter != null)
                lCharacter.ActiveEmotion = emotion;
        }
        nameBox.SetText(name);

        textBox.SetText(textToDisplay);


        if (cor != null)
        {
            StopCoroutine(cor);
        }
        corWaitTime = secondsBetweenChars;
        cor = StartCoroutine("showTextCharByChar");

        //paginas
    }

    //returns whether it has displayed any new text or there's nothing left to display
    public bool DisplayNext()
    {
        if (textBox.pageToDisplay < textBox.textInfo.pageCount)
        {
            pInput.SwitchCurrentActionMap("DisplayingText");

            textBox.pageToDisplay++;

            if (cor != null)
            {
                StopCoroutine(cor);
            }
            corWaitTime = secondsBetweenChars;
            cor = StartCoroutine("showTextCharByChar");

            return true;
        }
        else
            return false;
    }

    IEnumerator showTextCharByChar()
    {
        textBox.ForceMeshUpdate();
        {
            TMP_TextInfo textInfo = textBox.textInfo;
            int currentCharacter = 0;
            int characterCount = textInfo.characterCount;

            Color32[] newVertexColors;
            Color32 c0 = new Color32(255, 255, 255, 255);

            while (currentCharacter<characterCount)
            {

                // If No Characters then just yield and wait for some text to be added
                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }
                
                // Skip chars in other pages
                if (textInfo.characterInfo[currentCharacter].pageNumber != textBox.pageToDisplay-1)
                {
                    currentCharacter++;
                    continue;
                }


                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;


                // Only change the vertex color if the text element is visible.
                if (textInfo.characterInfo[currentCharacter].isVisible)
                {
                    newVertexColors[vertexIndex + 0] = c0;
                    newVertexColors[vertexIndex + 1] = c0;
                    newVertexColors[vertexIndex + 2] = c0;
                    newVertexColors[vertexIndex + 3] = c0;

                    // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                    textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                    // This last process could be done to only update the vertex data that has changed as opposed to all of the vertex data but it would require extra steps and knowing what type of renderer is used.
                    // These extra steps would be a performance optimization but it is unlikely that such optimization will be necessary.
                }

                currentCharacter = currentCharacter + 1;

                yield return new WaitForSeconds(corWaitTime);
            }
            pInput.SwitchCurrentActionMap(auxActionMap);
            yield break;
        }
    }
}
