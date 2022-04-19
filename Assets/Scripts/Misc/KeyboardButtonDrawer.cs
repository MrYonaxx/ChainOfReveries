using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardButtonDrawer : MonoBehaviour
{
    [SerializeField]
    List<Image> buttons;
    [SerializeField]
    List<Sprite> keyboardButtons;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(GameSettings.Keyboard)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].sprite = keyboardButtons[i];
            }
        }
    }


}
