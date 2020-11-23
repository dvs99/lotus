using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;


public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Vector2 leftLocation;
    [SerializeField] private Vector2 rightLocation;


    [SerializeField]private CharacterDisplayHelper lCharacter;
    [SerializeField] private CharacterDisplayHelper rCharacter;

    private Image boxImage;
    private TextMeshProUGUI textBox;
    private string prevActionMap;
    private PlayerInput pInput;


    void Start()
    {

        boxImage = GetComponent<Image>();
        textBox = GetComponentInChildren<TextMeshProUGUI>();

        pInput = Input.Instance.GetComponent<PlayerInput>();

        //Subscribe the input callback functions to the corresponding input events
        string actionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("UI");

        pInput.currentActionMap.FindAction("Submit").performed += ctx => thinghy(ctx);

        pInput.SwitchCurrentActionMap(actionMap);

        //
        Enable(lCharacter, rCharacter);
        DisplayNewText("SOMETHING", !rCharacter.IsTalking);
        //
    }

    public void thinghy(InputAction.CallbackContext context)
    {
        Enable(lCharacter, rCharacter);
        DisplayNewText("The standard Lorem Ipsum passage, used since the 1500s Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Section 1.10.32 of de Finibus Bonorum et Malorum, written by Cicero in 45 BC " +
    "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?" +
"But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?" +
"", Random.value > 0.5);
    }

    private void rightTalks()
    {
        boxImage.sprite = rCharacter.TextBoxSprite;
        rCharacter.IsTalking = true;
        lCharacter.IsTalking = false;
    }

    private void leftTalks()
    {
        boxImage.sprite = lCharacter.TextBoxSprite;
        rCharacter.IsTalking = false;
        lCharacter.IsTalking = true;
    }

    public void Enable(CharacterDisplayHelper leftCharacter, CharacterDisplayHelper rightCharacter, bool startWithRightActive=true)
    {
        lCharacter = leftCharacter;
        rCharacter = rightCharacter;

        lCharacter.GetComponent<RectTransform>().anchoredPosition = leftLocation;
        rCharacter.GetComponent<RectTransform>().anchoredPosition = rightLocation;
        lCharacter.Show();
        rCharacter.Show();

        //set the proper isTalking values for each of the characters
        if (startWithRightActive)
            rightTalks();
        else
            leftTalks();

        prevActionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("UI");
    }

    public void Disable(CharacterDisplayHelper leftCharacter, CharacterDisplayHelper rightCharacter, bool startWithRightActive = true)
    {
        lCharacter.Hide();
        rCharacter.Hide();
        lCharacter = null;
        rCharacter = null;
        boxImage.enabled = false;

        pInput.SwitchCurrentActionMap(prevActionMap);
    }

    public void DisplayNewText(string textToDisplay, bool rightActive= true)
    {
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
