using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_TeleportToTarget : CharacterAnimationEvent
    {
        [SerializeField]
        Vector3 offset;

        public override void Execute(CharacterBase character)
        {
            if (character.LockController.TargetLocked != null)
                character.transform.position = character.LockController.TargetLocked.transform.position + offset;
        }


    }
}
