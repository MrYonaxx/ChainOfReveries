using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_SetDirectionTarget : CharacterAnimationEvent
    {
        [InfoBox("à utiliser en combo avec MoveDirectionTarget")]
        [SerializeField]
        float maxTrackingY = 0.5f;

        [HideInInspector]
        public Vector3 direction;

        public override void Execute(CharacterBase character)
        {
            direction = new Vector3(character.CharacterMovement.Direction, 0, 0);
            if (character.LockController.TargetLocked != null) 
            {
                direction = (character.LockController.TargetLocked.transform.position - character.transform.position).normalized;
                if(Mathf.Abs(direction.y) > maxTrackingY)
                {
                    direction = new Vector3((1 - maxTrackingY) * Mathf.Sign(direction.x), maxTrackingY * Mathf.Sign(direction.y));
                }
            }
            //character.CharacterMovement.Move(direction.x * speed, direction.y * speed);
        }


    }
}
