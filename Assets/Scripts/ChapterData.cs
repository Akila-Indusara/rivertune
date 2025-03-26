using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "NewChapter", menuName = "Dialog System/Chapter")]
public class ChapterData : ScriptableObject
{
    [System.Serializable]
    public struct ChapterDialogData
    {
        public DialogData[] chapterDialogData;
        public Sprite dialogBackground;
    }

    public string chapterName;
    public ChapterDialogData[] chapterDialogDataArr;
    public AudioClip chapterMusic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
