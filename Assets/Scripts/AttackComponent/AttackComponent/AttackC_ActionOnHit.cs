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
    public class AttackC_ActionOnHit: AttackComponent
    {
        [SerializeField]
        AttackManager attack = null;



        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            character.CharacterAction.Action(attack);
        }

    } 

} // #PROJECTNAME# namespace