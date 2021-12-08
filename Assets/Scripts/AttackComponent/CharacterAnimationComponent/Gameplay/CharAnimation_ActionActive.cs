using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_ActionActive : CharacterAnimationEvent
    {
        //[SerializeField]
        //int frameUnactive = 0;

        [Space]
        /*[HorizontalGroup]
        [SerializeField]*/


        /*[HorizontalGroup]
        [HideLabel]
        [SerializeField]
        [HideIf("attack")]
        int actionID = 0;*/

        [SerializeField]
        AttackController attack = null;
        bool active = true;

        public override void Execute(CharacterBase character)
        {
            active = true;
            attack.ActionActive();
            /*if (attack != null)
            {
                if(active)
                    attack.ActionActive();
                else
                    attack.ActionUnactive();
                return;
            }


            if(active)
                character.CharacterAction.ActionActive(actionID);
            else
                character.CharacterAction.ActionUnactive(actionID);*/
        }


        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if (frame >= FrameEnd && active)
            {
                attack.ActionUnactive();
                active = false;
            }
        }

        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
