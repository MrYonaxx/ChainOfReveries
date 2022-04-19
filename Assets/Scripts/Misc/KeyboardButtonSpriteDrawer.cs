using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardButtonSpriteDrawer : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer button;
    [SerializeField]
    Sprite keyboardButtons;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(GameSettings.Keyboard)
        {
            button.sprite = keyboardButtons;
        }
    }


}
