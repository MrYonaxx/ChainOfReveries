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
    public class AttackC_Knockdown: AttackComponent
    {
        [SerializeField]
        bool knockdown = true;
        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            base.OnHitComponent(character, target);
            target.CharacterKnockback.Knockdown = knockdown;
        }

    } 

} // #PROJECTNAME# namespace