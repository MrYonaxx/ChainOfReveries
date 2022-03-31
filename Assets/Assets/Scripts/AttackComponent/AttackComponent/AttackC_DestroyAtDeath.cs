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
    public class AttackC_DestroyAtDeath: AttackComponent
    {

        [SerializeField]
        AttackManager attackM = null;


        CharacterBase user = null;

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            user = character;
            base.StartComponent(character, attack);
        }

        public override void UpdateComponent(CharacterBase character)
        {
            if(user != null)
            {
                if (user.CharacterKnockback.IsDead && attackM != null)
                    attackM.CancelAction();
            }
        }

    } 

} // #PROJECTNAME# namespace