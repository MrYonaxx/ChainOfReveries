using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_ActionMovement : CharacterAnimationEvent
    {
        [SerializeField]
        AttackController attackController = null;


        [SerializeField]
        bool turnBack = false;
        [SerializeField]
        bool moveToTarget = false;

        public override void Execute(CharacterBase character)
        {
            if(turnBack)
                attackController.Direction = -attackController.Direction;

            if(moveToTarget)
                attackController.transform.position = character.LockController.TargetLocked.transform.position;
        }



    }
}
