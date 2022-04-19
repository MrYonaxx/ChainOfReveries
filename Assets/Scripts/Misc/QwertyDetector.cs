using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class QwertyDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.systemLanguage == SystemLanguage.English)
        {
            ReInput.players.GetPlayer(0).controllers.maps.SetAllMapsEnabled(false, ControllerType.Keyboard);
            ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(true, ControllerType.Keyboard, 0, 1);
        }
    }

}
