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
        [HorizontalGroup]
        [SerializeField]
        float[] zoomValue = { -0.5f, -0.5f, 0};

        [HorizontalGroup]
        [SerializeField]
        [SuffixLabel("en frames")]
        float[] zoomTime = { 6, 12, 6 };

        [SerializeField]
        bool smoothZoom = false;

        [Button]
        void DefaultZoomValue()
        {
            zoomValue = new float[] { -0.5f, -0.5f, 0};
            zoomTime = new float[] {6, 12, 6};
        }

        [Button]
        void DefaultDezoomValue()
        {
            zoomValue = new float[] { 0.8f, 0.8f, 0};
            zoomTime = new float[] { 6, 12, 6 };
        }

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void ZoomScreen()
        {
            BattleFeedbackManager.Instance?.CameraZoom(zoomValue, zoomTime, smoothZoom);
        }

        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            if(target.CharacterStat.HP > 0)
                ZoomScreen();
            base.OnHitComponent(character, target);
        }

    } 

} // #PROJECTNAME# namespace