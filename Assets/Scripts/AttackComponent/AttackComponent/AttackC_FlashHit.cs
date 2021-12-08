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
    public class AttackC_FlashHit: AttackComponent
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        #endregion


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            base.OnHitComponent(character, target);
            BattleFeedbackManager.Instance?.BackgroundFlash();
        }


        #endregion

    } 

} // #PROJECTNAME# namespace