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
    public class AttackC_Untechable: AttackComponent
    {


        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            target.CharacterKnockback.CannotTech = true;
        }

    } 

} // #PROJECTNAME# namespace