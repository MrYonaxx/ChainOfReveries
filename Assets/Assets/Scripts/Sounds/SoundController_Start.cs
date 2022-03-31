using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VoiceActing
{
    public class SoundController_Start : MonoBehaviour
    {
        [SerializeField]
        SoundParameter soundStart;

        // Start is called before the first frame update
        void Start()
        {
            soundStart.PlaySound();
        }

    }
}