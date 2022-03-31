using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharacterAnimationEvent : MonoBehaviour
    {
        [HorizontalGroup("Frames")]
        [SerializeField]
        private int frame;
        public int Frame
        {
            get { return frame; }
        }

        [HorizontalGroup("Frames", Width = 0.3f)]
        [HideLabel]
        [ShowIf("ShowSecondFrame")]
        [SerializeField]
        private int frameEnd;
        public int FrameEnd
        {
            get { return frameEnd; }
        }

        public virtual void Execute(CharacterBase character)
        {

        }

        public virtual void UpdateComponent(CharacterBase character, int frame)
        {



        }

        public virtual bool ShowSecondFrame()
        {
            return false;
        }
    }
}
