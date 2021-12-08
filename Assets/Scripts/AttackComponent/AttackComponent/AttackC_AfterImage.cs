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
    public class AttackC_AfterImage: AttackComponent
    {
        AfterImageEffect afterImageEffect;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            
            afterImageEffect = character.FeedbacksComponents.GetComponent<AfterImageEffect>();
            afterImageEffect.StartAfterImage();

        }

        public override void EndComponent(CharacterBase character)
        {
            afterImageEffect.EndAfterImage();
        }





    } 

} // #PROJECTNAME# namespace