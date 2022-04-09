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
    public class AttackC_SoundOnHit: AttackComponent
    {
        [SerializeField]
        SoundParameter soundHit = null;

        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            soundHit.PlaySound();
        }


    } 

} // #PROJECTNAME# namespace