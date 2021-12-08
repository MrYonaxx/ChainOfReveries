using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_Direction : CharacterAnimationEvent
    {
        [SerializeField]
        int direction = 0;
        [SerializeField]
        bool turnToTarget = false;

        public override void Execute(CharacterBase character)
        {
            if (direction != 0)
            {
                character.CharacterMovement.SetDirection((int)Mathf.Sign(direction));
            }

            if (turnToTarget)
            {
                if (character.LockController.TargetLocked == null)
                    return;

                if (character.LockController.TargetLocked.transform.position.x > this.transform.position.x)
                    character.CharacterMovement.SetDirection(1);
                if (character.LockController.TargetLocked.transform.position.x < this.transform.position.x)
                    character.CharacterMovement.SetDirection(-1);
            }
        }
    }
}
