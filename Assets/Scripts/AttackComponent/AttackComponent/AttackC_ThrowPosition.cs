using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [System.Serializable]
    public class FrameTransform
    {
        public int Frame = 0;
        public Vector3 LocalPosition;
        public Vector3 LocalRotation;
    }

    public class AttackC_ThrowPosition : AttackComponent
    {
        [SerializeField]
        FrameTransform[] transforms;

        float t = 0;
        int direction = 0;
        bool reverse = false;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            t = 0f;
            direction = character.LockController.TargetLocked.CharacterMovement.Direction;
            if (direction != character.CharacterMovement.Direction)
                reverse = true;
        }

        public override void UpdateComponent(CharacterBase character)
        {
            t += Time.deltaTime * character.MotionSpeed;
            for (int i = 0; i < transforms.Length; i++)
            {
                if((t * 60) >= transforms[i].Frame)
                {
                    transform.localPosition = transforms[i].LocalPosition;
                    transform.localEulerAngles = transforms[i].LocalRotation;
                    if (reverse)
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -transform.localEulerAngles.z);
                    //transform.localEulerAngles = transforms[i].LocalRotation;
                }
            }
        }
    }
}
