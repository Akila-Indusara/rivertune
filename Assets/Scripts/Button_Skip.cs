using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Skip : MonoBehaviour
{
    public Button button;
    public Image buttonImage;
    public Sprite normalSprite;
    public Sprite hoveredSprite;

    private bool isToggled = false;

    void Start()
    {
        buttonImage = button.GetComponent<Image>();
    }

    public void toggleState()
    {
        isToggled = !isToggled;

        // When toggled ON, use the hovered sprite
        buttonImage.sprite = isToggled ? hoveredSprite : normalSprite;
    }
}
