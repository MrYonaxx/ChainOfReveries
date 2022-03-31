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
        GameObject attackController = null;

        [SerializeField]
        float speed = 1f;
        [SerializeField]
        float rotateSpeed = 1f;
        [SerializeField]
        bool rotate = false;


        Vector3 direction;
        private void Start()
        {
            direction = transform.right * attackController.transform.lossyScale.x;
        }

        public override void Execute(CharacterBase character)
        {
            if (character.LockController.TargetLocked == null)
                return;
            Vector3 directionTarget = character.LockController.TargetLocked.transform.position - attackController.transform.position;
            //float turnDirection = Vector3.Dot(Vector2.Perpendicular(direction), directionTarget);

            direction = Vector3.RotateTowards(direction, directionTarget, rotateSpeed * Time.deltaTime, 1f);
            //direction = direction * Quaternion.Euler(rotateSpeed * Mathf.Sign(turnDirection), 0, 0);

            direction.Normalize();
            direction *= speed;
            direction *= Time.deltaTime;
            attackController.transform.position += new Vector3(direction.x, direction.y, direction.y);
            if (rotate)
            {
                attackController.transform.localEulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(direction, Vector2.right * attackController.transform.lossyScale.x));
            }

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
