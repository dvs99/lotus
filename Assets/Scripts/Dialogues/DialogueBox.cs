using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;


public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Vector2 leftLocation=Vector2.zero;
    [SerializeField] private Vector2 rightLocation=Vector2.zero;


    [SerializeField]private CharacterDisplayHelper lCharacter;
    [SerializeField] private CharacterDisplayHelper rCharacter;

    private Image boxImage;
    private TextMeshProUGUI textBox;


    void Start()
    {

        boxImage = GetComponent<Image>();
        textBox = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void rightTalks()
    {
        if (rCharacter != null)
        {
            boxImage.sprite = rCharacter.TextBoxSprite;
            rCharacter.IsTalking = true;
        }
        if (lCharacter != null)
            lCharacter.IsTalking = false;
    }

    private void leftTalks()
    {
        if (lCharacter != null)
        {
            boxImage.sprite = lCharacter.TextBoxSprite;
            lCharacter.IsTalking = true;
        }
        if (rCharacter != null)
            rCharacter.IsTalking = false;
    }

    public void Enable(CharacterDisplayHelper leftCharacter, CharacterDisplayHelper rightCharacter, bool startWithRightActive=true)
    {
        textBox.enabled = true;
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
        if (lCharacter != null)
            lCharacter.Hide();
        if (rCharacter != null)
            rCharacter.Hide();
        lCharacter = null;
        rCharacter = null;
        boxImage.enabled = false;
        textBox.enabled = false;
    }

    public void DisplayNewText(string textToDisplay, bool rightActive= true)
    {
        textBox.pageToDisplay = 1;
        //set the proper isTalking values for each of the characters
        if (rightActive)
            rightTalks();
        else
            leftTalks();
        textBox.SetText(textToDisplay);
        textBox.ForceMeshUpdate();
    }

    //returns whether it has displayed any new text or there's nothing left to display
    public bool DisplayNext()
    {
        if (textBox.pageToDisplay<textBox.textInfo.pageCount)
        {
            textBox.pageToDisplay++;
            return true;
        }
        else
            return false;
    }
}
