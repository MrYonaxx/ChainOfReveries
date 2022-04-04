using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_MoveToPoint3 : CharacterAnimationEvent
    {
        [Space]
        [SerializeField]
        Vector3 point;
        [SerializeField]
        bool useDirection = false;
        [SerializeField]
        bool fixY = false;
        [SerializeField]
        bool fixX = false;

        float t = 0f;
        float tMax = 0f;

        public override void Execute(CharacterBase character)
        {
            if(useDirection)
                point = BattleUtils.Instance.BattleCenter.position + (point * character.CharacterMovement.Direction);
            else
                point = BattleUtils.Instance.BattleCenter.position + point;


            if (fixY)
                point.y = character.transform.position.y;
            if (fixX)
                point.x = character.transform.position.x;

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
        }



        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
