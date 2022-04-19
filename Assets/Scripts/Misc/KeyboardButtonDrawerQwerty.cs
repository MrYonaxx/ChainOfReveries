using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardButtonDrawerQwerty : MonoBehaviour
{
    [SerializeField]
    List<Image> buttons;
    [SerializeField]
    List<Sprite> keyboardButtons;
    [SerializeField]
    List<Sprite> keyboardButtonsQwerty;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(GameSettings.Keyboard && Application.systemLanguage == SystemLanguage.English)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].sprite = keyboardButtonsQwerty[i];
            }
        }
        else if (GameSettings.Keyboard)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].sprite = keyboardButtons[i];
            }
        }
    }


}
