using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_MoveToPoint : CharacterAnimationEvent
    {
        [Space]
        [SerializeField]
        Vector3 point;
        [SerializeField]
        [ShowIf("randomPoint")]
        Vector2 pointMax;

        [Space]
        [SerializeField]
        bool randomPoint = false;


        float t = 0f;
        float tMax = 0f;

        public override void Execute(CharacterBase character)
        {
            if(randomPoint)
            {
                point = new Vector3(Random.Range(point.x, pointMax.x), Random.Range(point.y, pointMax.y), 0);
            }

            point = BattleUtils.Instance.BattleCenter.position + point;
            tMax = (FrameEnd - Frame) / 60f;
            t = 0f;

            if(Frame == FrameEnd)
                character.transform.position = point;
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
