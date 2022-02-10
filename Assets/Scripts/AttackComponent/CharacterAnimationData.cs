using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{

    [System.Serializable]
    public class CharacterAnimationFrameData
    {
        [HorizontalGroup]
        [SerializeField]
        public int Frame;

        [HorizontalGroup]
        [HideLabel]
        [SerializeField]
        public CharacterAnimationEvent AnimationEvent;

    }

    // Classe qui contient toutes les infos à éxécuter lors d'une animation/action pour un character
    public class CharacterAnimationData : MonoBehaviour
    {
        [SerializeField]
        public Animator AnimatorDebug;
        [SerializeField]
        public AnimationClip AnimationClipDebug;

        [SerializeField]
        [Space]
        private List<CharacterAnimationFrameData> frameData;

        public List<CharacterAnimationFrameData> FrameData
        {
            get { return frameData; }
        }


        int id = 0;
        float t = 0f;
        private List<CharacterAnimationEvent> frameDataEvent;

        // Start is called before the first frame update
        /*void Start()
        {
            t = 0f;
            frameDataEvent = new List<CharacterAnimationEvent>(frameData.Count);
        }*/

        public void StartAnimationData()
        {
            t = 0f;
            frameDataEvent = new List<CharacterAnimationEvent>(frameData.Count);
        }


        // motionSpeed = vitesse de l'animator pour être bien calé
        public bool UpdateAnimation(CharacterBase character, float motionSpeed)
        {
            if (id >= frameData.Count)
                return false;

            t += Time.deltaTime * motionSpeed;
            int timeInFrame = (int)(t * 60f);

            // Update
            for (int i = 0; i < frameDataEvent.Count; i++)
            {
                frameDataEvent[i].UpdateComponent(character, timeInFrame);
            }

            // Start new Event
            for (int i = id; i < frameData.Count; i++)
            {
                if(timeInFrame >= frameData[i].AnimationEvent.Frame)
                {
                    id++;
                    frameData[i].AnimationEvent.Execute(character);
                    frameDataEvent.Add(frameData[i].AnimationEvent);
                }
            }

            return true;
        }

        [Button]
        public void GetFrameData()
        {
            List<CharacterAnimationEvent> frames = new List<CharacterAnimationEvent>(GetComponentsInChildren<CharacterAnimationEvent>());
            frames.Sort(CompareFrames);



            frameData.Clear();
            frameData = new List<CharacterAnimationFrameData>(frames.Count);

            for (int i = 0; i < frames.Count; i++)
            {
                frameData.Add(new CharacterAnimationFrameData());
                frameData[i].Frame = frames[i].Frame;
                frameData[i].AnimationEvent = frames[i];
            }
        }

        private int CompareFrames(CharacterAnimationEvent s1, CharacterAnimationEvent s2)
        {
            return s1.Frame.CompareTo(s2.Frame);
        }
    }
}
