using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VoiceActing
{
    public class SoundController_MusicStart : MonoBehaviour
    {
        [SerializeField]
        AudioClip music;

        // Start is called before the first frame update
        void Start()
        {
            AudioManager.Instance.PlayMusic(music);
        }

    }
}