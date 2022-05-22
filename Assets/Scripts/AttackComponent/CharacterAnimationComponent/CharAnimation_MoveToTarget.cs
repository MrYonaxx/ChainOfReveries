using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_MoveToTarget : CharacterAnimationEvent
    {
        [InfoBox("Suivi, pour une charge utilisé MoveToTarget")]
        [Space]
        [SerializeField]
        float speed = 3;
        [SerializeField]
        float maxTrackingY = 0.5f;

        bool once = false;


        public override void Execute(CharacterBase character)
        {
            if(Frame == FrameEnd)
            {
                if (once == false)
                {
                    // TP Instant
                    Vector3 distance = (character.LockController.TargetLocked.transform.position - character.transform.position);
                    character.CharacterMovement.Move(distance.x / Time.deltaTime, distance.y / Time.deltaTime);
                    StartCoroutine(ResetSpeed(character));
                    once = true;
                }
                return;
            }

            Vector3 direction = new Vector3(character.CharacterMovement.Direction, 0, 0);
            if (character.LockController.TargetLocked != null) 
            {
                direction = (character.LockController.TargetLocked.transform.position - character.transform.position).normalized;
                if(Mathf.Abs(direction.y) > maxTrackingY)
                {
                    direction = new Vector3((1 - maxTrackingY) * Mathf.Sign(direction.x), maxTrackingY * Mathf.Sign(direction.y));
                }

                if ((character.LockController.TargetLocked.transform.position - character.transform.position).sqrMagnitude < 0.1f)
                    direction = Vector3.zero;
            }
            character.CharacterMovement.Move(direction.x * speed, direction.y * speed);
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame < FrameEnd)
            {
                Execute(character);
            }
        }

        // Set la vitesse à zéro la frame suivante pour bien faire un seul déplacement
        private IEnumerator ResetSpeed(CharacterBase character)
        {
            yield return null;
            character.CharacterMovement.SetSpeed(0, 0);
        }

        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
