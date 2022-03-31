using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_ActionMoveToPoint2 : CharacterAnimationEvent
    {
        [Space]
        [InfoBox("Move sur un point en fonction des déplacements du perso")]
        [SerializeField]
        AttackController attack = null;

        [SerializeField]
        float radius = 1;
        [SerializeField]
        bool directionBackIfNoMovement = true;
        [SerializeField]
        bool randomDirection = false;
        Vector3 point;
        float t = 0f;
        float tMax = 0f;

        public override void Execute(CharacterBase character)
        {
            CharacterBase target = character.LockController.TargetLocked;
            Vector3 direction = new Vector3(target.CharacterMovement.SpeedX, target.CharacterMovement.SpeedY, 0);
            direction.Normalize();

            if(direction.magnitude < 0.01f)
            {
                if (randomDirection)
                {
                    direction = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0);
                    direction.Normalize();
                }
                else if (directionBackIfNoMovement)
                {
                    direction = new Vector3(-target.CharacterMovement.Direction, 0, 0);
                }
            }

            point = target.transform.position + (direction * radius);

            tMax = (FrameEnd - Frame) / 60f;
            t = 0f;
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame < FrameEnd)
            {
                t += Time.deltaTime * character.MotionSpeed;
                attack.transform.position = Vector3.Lerp(attack.transform.position, point, Mathf.Min(1, t / tMax));
            } 
        }



        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
