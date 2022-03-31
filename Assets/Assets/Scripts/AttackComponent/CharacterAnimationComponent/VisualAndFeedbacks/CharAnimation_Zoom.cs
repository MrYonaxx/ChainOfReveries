using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_Zoom : CharacterAnimationEvent
    {
        [HorizontalGroup("PArameter")]
        [SerializeField]
        bool zoomOnCharacter = false;
        [HorizontalGroup("PArameter")]
        [SerializeField]
        bool zoomCamera = false;

        [ShowIf("zoomCamera")]
        [HorizontalGroup]
        [SerializeField]
        float[] zoomValue = { -0.5f, -0.5f, 0 };

        [ShowIf("zoomCamera")]
        [HorizontalGroup]
        [SerializeField]
        [SuffixLabel("en frames")]
        float[] zoomTime = { 6, 12, 6 };

        [ShowIf("zoomCamera")]
        [SerializeField]
        bool smoothZoom = false;

        bool active = false;

        public override void Execute(CharacterBase character)
        {
            if(zoomCamera)
                BattleFeedbackManager.Instance?.CameraZoom(zoomValue, zoomTime, smoothZoom);
            if (zoomOnCharacter)
            {
                BattleFeedbackManager.Instance?.CameraController.AddTarget(this.transform, 10);
                active = true;
            }
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame >= FrameEnd && active)
            {
                if (zoomOnCharacter)
                    BattleFeedbackManager.Instance?.CameraController.RemoveTarget(this.transform);
                active = false;
            }
        }

        private void OnDestroy()
        {
            if (active)
            {
                if (zoomOnCharacter)
                    BattleFeedbackManager.Instance?.CameraController.RemoveTarget(this.transform);
                active = false;
            }
        }

        public override bool ShowSecondFrame()
        {
            return true;
        }
    }
}
