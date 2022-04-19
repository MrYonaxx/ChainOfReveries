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
using Feedbacks;

namespace VoiceActing
{
    public class AttackC_ResetGravity : AttackComponent
    {

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            if (target.CharacterMovement.InAir)
                target.CharacterMovement.SetSpeedZ(0.1f);

        }
    }

} // #PROJECTNAME# namespace