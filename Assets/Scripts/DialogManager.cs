using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject namePlate;
    public GameObject textBox;
    public GameObject character_left;
    public GameObject character_right;
    public DialogData [] dialogData;

    void Start()
    {
        StartCoroutine(StartDialog());
    }

    private IEnumerator StartDialog()
    {
        int dialogIndex = 0;

        while (dialogIndex < dialogData.Length)
        {
            DisplayDialog(dialogIndex);

            // Wait until the player presses Space before continuing
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));

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

        // Set the character
        if (dialog.dialogType == DialogData.DialogType.Narrate)
        {
            Color grayColor;
            ColorUtility.TryParseHtmlString("#8F8F8F", out grayColor);

            if (character_left.transform.childCount > 0 )
            {
                SetColor(character_left, grayColor);
            }

            if (character_right.transform.childCount > 0)
            {
                SetColor(character_right, grayColor);
            }

        } else
        {
            // left character
            if (dialog.character_Position == DialogData.characterPosition.Left)
            {

                setCharacter(dialog, character_left);

            }

            // right character
            else if (dialog.character_Position == DialogData.characterPosition.Right)
            {

                setCharacter(dialog, character_right);

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

    private void SetColor(GameObject character, Color color)
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

    private void setCharacter(DialogData dialog, GameObject character)
    {
        if (!(character.transform.childCount > 0 && character.transform.GetChild(0).name.Replace("(Clone)", "") == dialog.characterData.character.name))
        {
            Instantiate(dialog.characterData.character, character.transform);
        }
        // set color to white
        SetColor(character, Color.white);

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
