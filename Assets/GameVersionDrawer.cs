using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVersionDrawer : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI textVersion;
    // Start is called before the first frame update
    void Start()
    {
        textVersion.text += Application.version;
    }

}
