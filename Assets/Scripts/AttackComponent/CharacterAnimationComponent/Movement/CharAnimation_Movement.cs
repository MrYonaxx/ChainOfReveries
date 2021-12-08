using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_Movement : CharacterAnimationEvent
    {
        [SerializeField]
        bool moveForward = false;


        [SerializeField]
        Vector2 speed;
        [SerializeField]
        float jumpImpulsion = 0;

        public override void Execute(CharacterBase character)
        {
            if (moveForward)
                character.CharacterMovement.MoveForwardFixed(speed.x);
            else
                character.CharacterMovement.Move(speed.x, speed.y);

            if (jumpImpulsion > 0)
                character.CharacterMovement.Jump(jumpImpulsion);

        }

    }
}
