using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [System.Serializable]
    public class SoundParameter
    {
        [SerializeField]
        [HideLabel]
        AudioClip sound = null;

        [SerializeField]
        [HorizontalGroup(LabelWidth = 80)]
        float volume = 0.8f;

        [SerializeField]
        [HorizontalGroup(LabelWidth = 80)]
        Vector2 pitch = new Vector2(1,1);

        public void PlaySound()
        {
            if (sound == null)
                return;

            if (pitch.x == 1 && pitch.y == 1)
                AudioManager.Instance.PlaySound(sound, volume);
            else
                AudioManager.Instance.PlaySound(sound, volume, pitch.x, pitch.y);
        }

        public void PlaySound(float pitchMultiplier)
        {
            if (sound == null)
                return;
            AudioManager.Instance.PlaySound(sound, volume, pitch.x * pitchMultiplier, pitch.y * pitchMultiplier);
        }
    }
}

