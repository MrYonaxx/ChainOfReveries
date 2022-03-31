using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_NoCollision : CharacterAnimationEvent
    {
        bool active = true;

        [Space]
        [SerializeField]
        bool canObstacleOther = true; // Est-ce que les autres character peuvent passer au travers du character
        [SerializeField]
        LayerMask newLayer;
        CharacterRigidbody characterRigidbody;

        public override void Execute(CharacterBase character)
        {
            active = true;
            characterRigidbody = character.CharacterRigidbody;
            characterRigidbody.SetNewLayerMask(newLayer);
            characterRigidbody.CanObstacle(canObstacleOther);
            /*if (resetLayerMask)
                characterRigidbody.ResetLayerMask();
            else
                characterRigidbody.SetNewLayerMask(newLayer);*/
        }
        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame >= FrameEnd && active)
            {
                characterRigidbody.ResetLayerMask();
                characterRigidbody.CanObstacle(true);
                active = false;
            }
        }

        private void OnDestroy()
        {
            if (characterRigidbody != null)
            {
                characterRigidbody.ResetLayerMask();
                characterRigidbody.CanObstacle(true);
            }

        }

        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
