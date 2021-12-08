using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // Utilisé sur le shining veil de Sen
    public class CharAnimation_MoveToPoint2 : CharacterAnimationEvent
    {
        [Space]
        [InfoBox("Point doit être toujours positif. Si le joueur est à gauche on se déplace en point, si le joueur est à droite on se déplace en -point.")]
        [SerializeField]
        Vector3 point;


        float t = 0f;
        float tMax = 0f;

        public override void Execute(CharacterBase character)
        {
            float sign = (character.LockController.TargetLocked.transform.position.x - BattleUtils.Instance.BattleCenter.position.x);
            sign = Mathf.Sign(sign);
            character.CharacterMovement.SetDirection((int)sign);
            point = BattleUtils.Instance.BattleCenter.position + (point * -sign);
            tMax = (FrameEnd - Frame) / 60f;
            t = 0f;
        }

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame < FrameEnd)
            {
                t += Time.deltaTime * character.MotionSpeed;
                character.transform.position = Vector3.Lerp(character.transform.position, point, Mathf.Min(1, t / tMax));
                
            } 
            else if (t > 0f)
            {
                character.CharacterMovement.SetSpeed(0, 0);
            }
        }



        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
