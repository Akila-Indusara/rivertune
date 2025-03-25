using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(DialogData))]
public class DialogDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogData dialog = (DialogData)target;

        DrawDefaultInspector(); // Draw default fields (CharacterData, dialogText)

        if (dialog.characterData == null)
        {
            EditorGUILayout.HelpBox("Select a CharacterData first.", MessageType.Warning);
            return;
        }

        // Ensure expressionSelections list size matches characterParts size
        while (dialog.GetExpressionSelections().Count < dialog.characterData.characterParts.Length)
        {
            dialog.GetExpressionSelections().Add(new DialogData.ExpressionSelection());
        }

        // Display available CharacterExpressionParts and allow sprite selection
        for (int i = 0; i < dialog.characterData.characterParts.Length; i++)
        {
            var part = dialog.characterData.characterParts[i];

            // Ensure the selection object exists
            dialog.GetExpressionSelections()[i].expressionPartName = part.name;

            EditorGUILayout.LabelField($"Expression: {part.name}", EditorStyles.boldLabel);

            if (part.sprites.Length > 0)
            {
                string[] spriteOptions = part.sprites.Select(s => s != null ? s.name : "None").ToArray();
                dialog.GetExpressionSelections()[i].spriteIndex = EditorGUILayout.Popup("Select Sprite", dialog.GetExpressionSelections()[i].spriteIndex, spriteOptions);
            }
            else
            {
                EditorGUILayout.HelpBox("No sprites assigned for this part.", MessageType.Warning);
            }

            EditorGUILayout.Space();
        }

        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(dialog);
        }
    }
}
