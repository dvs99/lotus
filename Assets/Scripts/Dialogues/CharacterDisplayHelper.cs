using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplayHelper : MonoBehaviour
{
    [SerializeField] private Sprite[] emotionSprites = null;

    [SerializeField] private Sprite textBoxSprite;
    public Sprite TextBoxSprite { get { return textBoxSprite; } private set { textBoxSprite = value; } }

    public Color GreyOutColor;

    private Image characterImage;

    private Emotion activeEmotion;
    private bool isTalking;

    public Emotion ActiveEmotion
    {
        get { return activeEmotion; }
        set //when the active emotion is set also change the sprite accordingly
        {
            activeEmotion = value;
            if ((int)activeEmotion >= emotionSprites.Length)
                activeEmotion = Emotion.Happy;
            characterImage.sprite = emotionSprites[(int)activeEmotion];
        }
    }

    public bool IsTalking
    {
        get { return isTalking; }
        set //when the character is not talking make it loog greyed out
        {
            if (value)
            {
                isTalking = true;
                characterImage.color = Color.white;
            }
            else
            {
                isTalking = false;
                characterImage.color = GreyOutColor;
            }
        }
    }

    void Start()
    {
        if (textBoxSprite == null)
            throw new System.Exception("No textbox assigned to CharacterDisplayHelper on " + gameObject.name);

        characterImage = GetComponent<Image>();
        ActiveEmotion = Emotion.Happy;
    }

    public void Show()
    {
        characterImage.enabled = true;
    }
    public void Hide()
    {
        characterImage.enabled = false;
    }
}