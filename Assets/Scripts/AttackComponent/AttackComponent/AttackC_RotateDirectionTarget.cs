using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class AttackC_RotateDirectionTarget : AttackComponent
    {
        [Space]
        [SerializeField]
        float maxAngle = 20;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            Vector3 direction = new Vector3(character.CharacterMovement.Direction, 0, 0);
            Vector3 directionTarget = (character.LockController.TargetLocked.transform.position - character.transform.position).normalized;
            float angle = Mathf.Clamp(Vector3.SignedAngle(direction, directionTarget, Vector2.up), -maxAngle, maxAngle);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle * -character.CharacterMovement.Direction);
        }
    }
}
