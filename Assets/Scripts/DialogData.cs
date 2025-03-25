using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog System/Dialog")]
public class DialogData : ScriptableObject
{
    public enum DialogType
    {
        Narrate,
        Talk,
        Think,
        Whisper,
        Shout
    }
    public enum characterPosition
    {
        Left,
        Right
    }
    [System.Serializable]
    public class ExpressionSelection
    {
        public string expressionPartName; //"Mouth", "Eyes"
        public int spriteIndex; // Index of the selected sprite in CharacterExpressionPart.sprites
    }

    public CharacterData characterData; // The character speaking
    public string dialogText;
    public DialogType dialogType;
    public characterPosition character_Position;

    private List<ExpressionSelection> expressionSelections = new List<ExpressionSelection>(); // List of chosen expressions

    public List<ExpressionSelection> GetExpressionSelections()
    {
        return expressionSelections;
    }
}
