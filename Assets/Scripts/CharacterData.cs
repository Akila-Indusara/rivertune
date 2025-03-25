using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Dialog System/Character")]
public class CharacterData : ScriptableObject
{

    [System.Serializable]
    public struct CharacterExpressionPart
    {
        public string name; //should be same as the character part name in the prefab
        public Sprite [] sprites;
    }

    public GameObject character; //make a character prefab
    public Sprite namePlate; //name plate sprite
    public CharacterExpressionPart[] characterParts;

}
