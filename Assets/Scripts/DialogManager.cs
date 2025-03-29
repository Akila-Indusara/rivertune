using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using static ChapterData;

public class DialogManager : MonoBehaviour
{
    public GameObject namePlate;
    public GameObject textBox;
    public GameObject character_left;
    public GameObject character_right;
    public GameObject character_right_2;
    public GameObject button_Skip;
    public GameObject Background;
    public GameObject Music;
    public GameObject content;
    public GameObject BlackScreen;
    public ChapterData[] chapters;

    private Animator blackScreenAnimator;
    private List<DialogData> dialogData = new List<DialogData>();
    private bool isSkipping = false;
    private bool isWaiting = false;
    private int chapterIndex = 0;


    void Awake()
    {
        blackScreenAnimator = BlackScreen.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(startChapter());
    }

    public void ToggleSkip()
    {
        isSkipping = !isSkipping;
        button_Skip.GetComponent<Button_Skip>().toggleState();
    }
    public void SetSkipFalse()
    {
        isSkipping = false;
        button_Skip.GetComponent<Button_Skip>().setIsToggledFalse();
    }

    public void setIsWaitingFalse()
    {
        blackScreenAnimator.ResetTrigger("FadeIn_Out");
        isWaiting = false;
    }

    private IEnumerator startChapter()
    {
        
        while (chapterIndex < chapters.Length)
        {
            if (content.activeSelf)
            {
                SetSkipFalse();
            }

            ChapterData chapter = chapters[chapterIndex];
            ChapterData.ChapterDialogData [] chapterDialogDataArr = chapter.chapterDialogDataArr;

            isWaiting = true;
            
            BlackScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = chapter.chapterName;
            blackScreenAnimator.SetTrigger("FadeIn_Out");

            yield return new WaitUntil(() => !isWaiting);

            content.SetActive(true);

            for (int i = 0; i < chapterDialogDataArr.Length; i++)
            {
                ChapterData.ChapterDialogData chapterDialogData = chapterDialogDataArr[i];
                dialogData = new List<DialogData>(chapterDialogData.chapterDialogData);
                // Set the background
                Background.GetComponent<Image>().sprite = chapterDialogData.dialogBackground;
                
                // Play the music
                Music.GetComponent<AudioSource>().clip = chapter.chapterMusic;
                Music.GetComponent<AudioSource>().Play();
                yield return StartCoroutine(StartDialog());

                SetSkipFalse();

                //remove all characters
                if (character_left.transform.childCount > 0)
                {
                    Destroy(character_left.transform.GetChild(0).gameObject);
                }
                if (character_right.transform.childCount > 0)
                {
                    Destroy(character_right.transform.GetChild(0).gameObject);
                }
                if (i != chapterDialogDataArr.Length - 1)
                {
                    isWaiting = true;
                    BlackScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    blackScreenAnimator.SetTrigger("FadeIn_Out");
                    yield return new WaitUntil(() => !isWaiting);
                }
            }
            Debug.Log("Chapter " + chapter.chapterName + " finished");
            chapterIndex++;
        }
    }


    private IEnumerator StartDialog()
    {
        int dialogIndex = 0;

        while (dialogIndex < dialogData.Count)
        {
            DisplayDialog(dialogIndex);

            if (!isSkipping)
            {
                yield return new WaitUntil(() => isSkipping || Input.GetKeyDown(KeyCode.Space));

                // If skipping was enabled while waiting, exit waiting loop
                if (!isSkipping)
                {
                    yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
                }
            } else
            {
                yield return new WaitForSeconds(0.2f);
            }

            dialogIndex++;
        }
        
    }

    private void DisplayDialog(int dialogIndex)
    {
        DialogData dialog = dialogData[dialogIndex];

        // Set the name plate
        if (dialog.characterData.namePlate != null)
        {
            namePlate.GetComponent<Image>().sprite = dialog.characterData.namePlate;
            namePlate.gameObject.SetActive(true);
        }
        else
        {
            namePlate.gameObject.SetActive(false);
        }

        Color grayColor;
        ColorUtility.TryParseHtmlString("#8F8F8F", out grayColor);
        // Set the character
        if (dialog.dialogType == DialogData.DialogType.Narrate)
        {

            SetColor(character_left, grayColor);
            DecreaseDrawOrder(1, character_left);
            SetDefaultEmotion(character_left);

            SetColor(character_right, grayColor);
            DecreaseDrawOrder(1, character_right);
            SetDefaultEmotion(character_right);

        }
        else
        {
            // left character
            if (dialog.character_Position == DialogData.characterPosition.Left)
            {
                SetColor(character_right, grayColor);
                SetColor(character_right_2, grayColor);

                DecreaseDrawOrder(1, character_right);
                DecreaseDrawOrder(1, character_right_2);

                SetDefaultEmotion(character_right);
                SetDefaultEmotion(character_right_2);

                SetCharacterExpression(dialog, character_left);

            }

            // right character
            else if (dialog.character_Position == DialogData.characterPosition.Right)
            {
                SetColor(character_left, grayColor);
                SetColor(character_right_2, grayColor);

                DecreaseDrawOrder(1, character_left);
                DecreaseDrawOrder(1, character_right_2);

                SetDefaultEmotion(character_left);
                SetDefaultEmotion(character_right_2);

                SetCharacterExpression(dialog, character_right);

            }
            else if (dialog.character_Position == DialogData.characterPosition.Right_2)
            {
                SetColor(character_left, grayColor);
                SetColor(character_right, grayColor);

                DecreaseDrawOrder(1, character_left);
                DecreaseDrawOrder(1, character_right);

                SetDefaultEmotion(character_left);
                SetDefaultEmotion(character_right);

                SetCharacterExpression(dialog, character_right_2);
            }
        }

            // Set the dialog text
            textBox.GetComponent<TextMeshProUGUI>().text = dialog.dialogText;
        if (dialog.dialogType == DialogData.DialogType.Talk)
        {
            textBox.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
        else if (dialog.dialogType == DialogData.DialogType.Think)
        {
            textBox.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Italic;
        }
        else if (dialog.dialogType == DialogData.DialogType.Whisper)
        {
            // set text style
        }
        else if (dialog.dialogType == DialogData.DialogType.Shout)
        {
            // set text style
        }

    }

    private void IncreseDrawOrder(int increseBy, GameObject character)
    {
        if (character != null)
        {
            if (character.transform.childCount > 0)
            {
                Transform characterChild = character.transform.GetChild(0);
                SpriteRenderer spriteRenderer = characterChild.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sortingOrder += increseBy;
                }
                foreach (Transform child in characterChild)
                {
                    SpriteRenderer spriteRendererChild = child.GetComponent<SpriteRenderer>();
                    if (spriteRendererChild != null)
                    {
                        spriteRendererChild.sortingOrder += increseBy;
                    }
                }
            }
        }
    }

    private void DecreaseDrawOrder(int decreaseBy, GameObject character)
    {
        if (character != null)
        {
            if (character.transform.childCount > 0)
            {
                Transform characterChild = character.transform.GetChild(0);
                SpriteRenderer spriteRenderer = characterChild.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sortingOrder -= decreaseBy;
                }
                foreach (Transform child in characterChild)
                {
                    SpriteRenderer spriteRendererChild = child.GetComponent<SpriteRenderer>();
                    if (spriteRendererChild != null)
                    {
                        spriteRendererChild.sortingOrder -= decreaseBy;
                    }
                }
            }
        }
    }

    private void SetColor(GameObject character, Color color)
    {
        if (character != null)
        {
            if (character.transform.childCount > 0)
            {
                Transform characterChild = character.transform.GetChild(0);
                SpriteRenderer spriteRenderer = characterChild.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = color;
                }
                foreach (Transform child in characterChild)
                {
                    SpriteRenderer spriteRendererChild = child.GetComponent<SpriteRenderer>();

                    if (spriteRendererChild != null)
                    {
                        spriteRendererChild.color = color;
                    }
                }
            }
        }
    }

    private void SetDefaultEmotion(GameObject character)
    {
        if (character != null)
        {
            if (character.transform.childCount > 0)
            {
                Transform characterChild = character.transform.GetChild(0);
                foreach (Transform child in characterChild)
                {
                    characterChild.GetComponent<CharacterScript>().setDefaultEmotions();
                }
            }
        }
    }

    // Set the character's expression
    private void SetCharacterExpression(DialogData dialog, GameObject character)
    {
        if (!(character.transform.childCount > 0 && character.transform.GetChild(0).name.Replace("(Clone)", "") == dialog.characterData.character.name))
        {
            if (character.transform.childCount > 0) Destroy(character.transform.GetChild(0).gameObject);
            Instantiate(dialog.characterData.character, character.transform);
        }
        // set color to white
        SetColor(character, Color.white);
        IncreseDrawOrder(1, character);

        if (dialog.isCharacterFlipped)
        {
            character.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            character.transform.localScale = new Vector3(1, 1, 1);
        }

        // Set the character's expression
        for (int i = 0; i < dialog.GetExpressionSelections().Count; i++)
        {
            DialogData.ExpressionSelection selection = dialog.GetExpressionSelections()[i];
            string partName = selection.expressionPartName;
            int spriteIndex = selection.spriteIndex;
            Transform part = character.transform.GetChild(0).Find(partName);
            if (part != null)
            {
                SpriteRenderer spriteRendererPart = part.GetComponent<SpriteRenderer>();
                if (spriteRendererPart != null)
                {
                    spriteRendererPart.sprite = dialog.characterData.characterParts[i].sprites[spriteIndex];
                }
            }
        }
    }

}
