/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class AttackC_SetState: AttackComponent
    {
        [SerializeField]
        CharacterState state = null;

        CharacterBase target;

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            target = character.LockController.TargetLocked;
            target.SetState(state);
        }

        public override void EndComponent(CharacterBase character)
        {
            if (target.State == state)
                target.ResetToIdle();
        }

    } 

} // #PROJECTNAME# namespace