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
    public class AttackC_Shake: AttackComponent
    {
        [SerializeField]
        float shakePower = 0.2f;
        [SerializeField]
        float shakeTime = -1;

        ShakeSprite shakeSprite = null;

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            // C'est pas encore opti opti mais ça pptimise un peu
            if (shakeSprite == null)
                shakeSprite = target.FeedbacksComponents.GetComponent<ShakeSprite>();

            if (shakeTime > 0)
                shakeSprite.Shake(shakePower, shakeTime);
            else
                shakeSprite.Shake(shakePower, target.CharacterKnockback.KnockbackTime);

            base.OnHitComponent(character, target);
        }

    } 

} // #PROJECTNAME# namespace