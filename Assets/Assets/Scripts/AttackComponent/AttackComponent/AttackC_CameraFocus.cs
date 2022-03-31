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
    public class AttackC_CameraFocus: AttackComponent
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Transform focusTarget = null;
        [SerializeField]
        int priority = 0;

        #endregion


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public void Awake()
        {
            if (focusTarget == null)
                BattleFeedbackManager.Instance?.CameraController.AddTarget(this.transform, priority);
            else
                BattleFeedbackManager.Instance?.CameraController.AddTarget(focusTarget, priority);
        }

        public override void EndComponent(CharacterBase character)
        {
            base.EndComponent(character);
            if (focusTarget == null)
                BattleFeedbackManager.Instance?.CameraController.RemoveTarget(this.transform);
            else
                BattleFeedbackManager.Instance?.CameraController.RemoveTarget(focusTarget);
        }

        public void OnDestroy()
        {
            if (focusTarget == null)
                BattleFeedbackManager.Instance?.CameraController.RemoveTarget(this.transform);
            else
                BattleFeedbackManager.Instance?.CameraController.RemoveTarget(focusTarget);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace