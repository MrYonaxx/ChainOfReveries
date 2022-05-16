using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_MoveDirectionTarget2 : CharacterAnimationEvent
    {
        [Space]
        [SerializeField]
        float speed = 3;
        [SerializeField]
        float maxTrackingY = 0.5f;
        [SerializeField]
        float stopDistance = 0.5f;

        Vector3 direction;
        bool active = true;

        public override void Execute(CharacterBase character)
        {
            direction = new Vector3(character.CharacterMovement.Direction, 0, 0);
            if (character.LockController.TargetLocked != null)
            {
                // On s'assure que la cible est dans notre direction, sinon on part tout droit
                if (Mathf.Sign((character.LockController.TargetLocked.transform.position - character.transform.position).x) == character.CharacterMovement.Direction)
                {
                    direction = (character.LockController.TargetLocked.transform.position - character.transform.position).normalized;
                    if (Mathf.Abs(direction.y) > maxTrackingY)
                    {
                        direction = new Vector3((1 - maxTrackingY) * Mathf.Sign(direction.x), maxTrackingY * Mathf.Sign(direction.y));
                    }
                }
            }
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame <= FrameEnd) 
            {
                if((character.LockController.TargetLocked.transform.position - character.transform.position).sqrMagnitude > stopDistance) 
                {
                    character.CharacterMovement.Move(direction.x * speed, direction.y * speed);
                }
                else
                {
                    character.CharacterMovement.Move(0, 0);
                }
            }
        }

        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
