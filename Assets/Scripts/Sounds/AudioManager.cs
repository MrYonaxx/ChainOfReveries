/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VoiceActing
{
    /// <summary>
    /// Definition of the AudioManager class
    /// </summary>
    public class AudioManager : MonoBehaviour
    {

        [SerializeField]
        private float musicVolumeMax = 1;
        [SerializeField]
        private float soundVolumeMax = 1;

        [SerializeField]
        private AudioSource audioMusic;
        [SerializeField]
        private AudioSource audioSound;


        [SerializeField] private AudioSource[] audioArray;
        private int audioIndex = 0;


        public static AudioManager Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }



        public AudioSource GetAudioSourceMusic()
        {
            return audioMusic;
        }


        // ===========================================================================================
        // Musique

        public void PlayMusic(AudioClip music, int timeFade = 1)
        {
            if (music == audioMusic.clip)
            {
                audioMusic.volume = musicVolumeMax;
                return;
            }
            audioMusic.clip = music;
            audioMusic.Play();

            StopAllCoroutines();
            StartCoroutine(PlayMusicCoroutine(timeFade));
        }


        private IEnumerator PlayMusicCoroutine(float timeFade)
        {
            if (timeFade <= 0)
                timeFade = 1;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / timeFade;
                audioMusic.volume = Mathf.Lerp(0, musicVolumeMax, t);
                yield return null;
            }
            audioMusic.volume = musicVolumeMax;
        }




        public void StopMusic(float timeFade = 1)
        {
            StopAllCoroutines();
            StartCoroutine(StopMusicCoroutine(timeFade));
        }

        private IEnumerator StopMusicCoroutine(float timeFade)
        {
            if (timeFade <= 0)
                timeFade = 1;
            float music1Volume = audioMusic.volume;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / timeFade;
                audioMusic.volume = Mathf.Lerp(music1Volume, 0, t);
                yield return null;
            }
            audioMusic.volume = 0;
            audioMusic.clip = null;
        }



        public void StopMusicWithScratch(float time)
        {
            StopAllCoroutines();
            StartCoroutine(StopMusicCoroutine(time));
            StartCoroutine(FadeVolumeWithPitch(time * 0.25f));
        }

        private IEnumerator FadeVolumeWithPitch(float time)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / time;
                audioMusic.pitch = Mathf.Lerp(2, 1, t);
                yield return null;
            }
            audioMusic.pitch = 1f;
        }

        // ===========================================================================================
        // Son


        public void PlaySound(AudioClip sound, float volumeMultiplier = 1)
        {
            audioSound.PlayOneShot(sound, soundVolumeMax * volumeMultiplier);
        }

        public void PlaySound(AudioClip sound, float volumeMultiplier = 1, float pitchMin = 1, float pitchMax = 1)
        {
            ++audioIndex;
            if (audioIndex >= audioArray.Length)
            {
                audioIndex = 0;
            }
            audioArray[audioIndex].clip = sound;
            audioArray[audioIndex].volume = soundVolumeMax * volumeMultiplier;
            audioArray[audioIndex].pitch = Random.Range(pitchMin, pitchMax);
            audioArray[audioIndex].Play();
        }

        /*public void PlayVoice(AudioClip sound, float volumeMultiplier = 1)
        {
            audioVoice.PlayOneShot(sound, voiceVolumeMax * volumeMultiplier);
        }*/



        public void SetMusicVolume(int value)
        {
            musicVolumeMax = (value / 10f);

            audioMusic.volume = musicVolumeMax;
            /*if (audioMusic.volume != 0)
                audioMusic.volume = musicVolumeMax;*/
        }

        public void SetSoundVolume(int value)
        {     
            soundVolumeMax = (value / 10f);
            audioSound.volume = soundVolumeMax;
        }
    } // AudioManager class

} // #PROJECTNAME# namespace