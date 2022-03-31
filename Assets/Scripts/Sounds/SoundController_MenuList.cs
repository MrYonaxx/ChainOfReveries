using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu;

namespace VoiceActing
{
    public class SoundController_MenuList : MonoBehaviour
    {
        [SerializeField]
        MenuList menu = null;

        [SerializeField]
        SoundParameter soundSelect;
        [SerializeField]
        SoundParameter soundValidate;
        [SerializeField]
        SoundParameter soundQuit;

        // Start is called before the first frame update
        void Start()
        {
            menu.OnSelected += SoundSelect;
            menu.OnValidate += SoundValidate;
            menu.OnEnd += SoundQuit;
        }

        // Update is called once per frame
        void OnDestroy()
        {
            menu.OnSelected -= SoundSelect;
            menu.OnValidate -= SoundValidate;
            menu.OnEnd -= SoundQuit;
        }

        void SoundSelect(int id)
        {
            soundSelect.PlaySound();
        }
        void SoundValidate(int id)
        {
            soundValidate.PlaySound();
        }
        void SoundQuit()
        {
            soundQuit.PlaySound();
        }
    }
}
