using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_ActionCharged : CharacterAnimationEvent
    {

        [SerializeField]
        AttackManager actionNotCharged = null;

        bool charging = true;

        /*public override void Execute(CharacterBase character)
        {
            character.CharacterAction.EndAction();
            character.CharacterAction.Action(action);
        }*/

        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if(frame < FrameEnd)
            {
                if(character.Inputs.InputY.InputValue != 1)
                {
                    charging = false;
                }
            }
            else if (frame >= FrameEnd && !charging)
            {

                character.CharacterAction.EndAction();
                character.CharacterAction.Action(actionNotCharged);
            }
        }

        public override bool ShowSecondFrame()
        {
            return true;
        }


    }
}
