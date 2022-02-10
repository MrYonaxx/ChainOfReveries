using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // Pour les moves qui track mais qui galère à tourner
    public class CharAnimation_ActionMovementTracking : CharacterAnimationEvent
    {
        [SerializeField]
        AttackController attackController = null;

        [SerializeField]
        float speed = 1f;
        [SerializeField]
        float rotateSpeed = 1f;


        Vector3 direction;

        public override void Execute(CharacterBase character)
        {
            Vector3 directionTarget = character.LockController.TargetLocked.transform.position - attackController.transform.position;
            //float turnDirection = Vector3.Dot(Vector2.Perpendicular(direction), directionTarget);

            direction = Vector3.RotateTowards(direction, directionTarget, rotateSpeed * Time.deltaTime, 1f);
            //direction = direction * Quaternion.Euler(rotateSpeed * Mathf.Sign(turnDirection), 0, 0);

            direction.Normalize();
            direction *= speed;
            direction *= Time.deltaTime;
            attackController.transform.position += new Vector3(direction.x, direction.y, direction.y);


        }
        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame <= FrameEnd)
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
