using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_MoveDirectionTarget : CharacterAnimationEvent
    {
        [InfoBox("Charge, pour un suivi utilisé MoveToTarget")]
        [Space]
        [SerializeField]
        float speed = 3;
        [SerializeField]
        float maxTrackingY = 0.5f;


        Vector3 direction;
        bool active = true;

        public override void Execute(CharacterBase character)
        {
            direction = new Vector3(character.CharacterMovement.Direction, 0, 0);
            if (character.LockController.TargetLocked != null)
            {
                direction = (character.LockController.TargetLocked.transform.position - character.transform.position).normalized;
                if (Mathf.Abs(direction.y) > maxTrackingY)
                {
                    direction = new Vector3((1 - maxTrackingY) * Mathf.Sign(direction.x), maxTrackingY * Mathf.Sign(direction.y));
                }
            }
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame >= FrameEnd && active) 
            {
                character.CharacterMovement.Move(direction.x * speed, direction.y * speed);
                active = false;
            }
        }

        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
