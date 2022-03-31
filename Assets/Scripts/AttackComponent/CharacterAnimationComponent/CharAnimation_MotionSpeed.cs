using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_MotionSpeed : CharacterAnimationEvent
    {
        [HorizontalGroup("Boolean")]
        [SerializeField]
        bool affectCharacter = false;
        [HorizontalGroup("Boolean")]
        [SerializeField]
        bool affectAll = false;

        [HorizontalGroup("MotionSpeed")]
        [ShowIf("affectCharacter")]
        [SerializeField]
        float motionSpeedCharacter = 0f;
        [HorizontalGroup("MotionSpeed")]
        [ShowIf("affectAll")]
        [SerializeField]
        float motionSpeedAll = 0f;

        public override void Execute(CharacterBase character)
        {
            // En gros quand on appelle SetMotionSpeed, AttackManager pour se recaler appelle le Update
            // de CharacterAnimationData, le problème c'est que cela créer un appel imbriqué ce qui va venir appeler
            // les character event à la même frame une deuxième fois
            BattleFeedbackManager.Instance?.SetBattleMotionSpeed(motionSpeedAll, (FrameEnd - Frame) / 60f);
            character.SetCharacterMotionSpeed(motionSpeedCharacter, (FrameEnd - Frame) / 60f);
        }

        public override bool ShowSecondFrame()
        {
            return true;
        }
    }
}
