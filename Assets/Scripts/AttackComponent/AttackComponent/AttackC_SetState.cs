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

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);

            character.LockController.TargetLocked.SetState(state);
        }

        public override void EndComponent(CharacterBase character)
        {
            if (character.State == state)
                character.ResetToIdle();
        }

    } 

} // #PROJECTNAME# namespace