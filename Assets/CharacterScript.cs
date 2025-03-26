using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameObjectSpritePair
{
    public GameObject gameObject;
    public Sprite defaultSprite;
}

public class CharacterScript : MonoBehaviour
{
    [Header("Assign GameObjects and their Default Sprites")]
    [SerializeField] private List<GameObjectSpritePair> objectSpriteList = new List<GameObjectSpritePair>();

    private Dictionary<GameObject, Sprite> objectSpriteMap = new Dictionary<GameObject, Sprite>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        // Convert List to Dictionary for fast lookups
        foreach (var pair in objectSpriteList)
        {
            if (pair.gameObject != null && pair.defaultSprite != null)
            {
                objectSpriteMap[pair.gameObject] = pair.defaultSprite;
            }
        }
    }

    public void setDefaultEmotions()
    {
        foreach (var pair in objectSpriteMap)
        {
            GameObject obj = pair.Key;
            Sprite sprite = pair.Value;

            if (obj != null)
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = sprite;
                }
            }
        }
    }
}
