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
    public class AttackC_Zoom: AttackComponent
    {
        [SerializeField]
        bool dezoom = false;



        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void ZoomScreen()
        {
            if(dezoom)
                BattleFeedbackManager.Instance?.CameraDeZoom();
            else
                BattleFeedbackManager.Instance?.CameraZoom();
        }

        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            ZoomScreen();
            base.OnHitComponent(character, target);
        }

    } 

} // #PROJECTNAME# namespace