using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_MoveToTarget : CharacterAnimationEvent
    {
        [Space]
        [SerializeField]
        float speed = 3;
        [SerializeField]
        float maxTrackingY = 0.5f;


        public override void Execute(CharacterBase character)
        {
            Vector3 direction = new Vector3(character.CharacterMovement.Direction, 0, 0);
            if (character.LockController.TargetLocked != null) 
            {
                direction = (character.LockController.TargetLocked.transform.position - character.transform.position).normalized;
                if(Mathf.Abs(direction.y) > maxTrackingY)
                {
                    direction = new Vector3((1 - maxTrackingY) * Mathf.Sign(direction.x), maxTrackingY * Mathf.Sign(direction.y));
                }
            }
            if ((character.LockController.TargetLocked.transform.position - character.transform.position).sqrMagnitude < 0.1f)
                direction = Vector3.zero;
            character.CharacterMovement.Move(direction.x * speed, direction.y * speed);
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame < FrameEnd)
            {
                Execute(character);
            }
        }



        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
