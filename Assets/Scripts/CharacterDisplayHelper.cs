using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplayHelper : MonoBehaviour
{
    [SerializeField] private Sprite textBoxSprite;

    [SerializeField] private Color greyOutColor;

    [SerializeField] private Sprite[] emotionSprites;
    public Sprite TextBoxSprite { get; private set; }
    public Color GreyOutColor { get; set; }


    private Image characterImage;
    private Emotion activeEmotion;
    private bool isTalking;

    public Emotion ActiveEmotion
    {
        get { return activeEmotion; }
        set //when the active emotion is set also change the sprite accordingly
        {
            activeEmotion = value;
            Debug.Log (emotionSprites.Length);
            characterImage.sprite = emotionSprites[(int)activeEmotion];
        }
    }

    public bool IsTalking
    {
        get { return isTalking; }
        set //when the character is not talking make it loog greyed out
        {
            if (value)
               characterImage.color = Color.white;
            else
                characterImage.color = greyOutColor;

        }
    }

    void Start()
    {
        characterImage = GetComponent<Image>();
        ActiveEmotion = Emotion.Neutral;
    }
}