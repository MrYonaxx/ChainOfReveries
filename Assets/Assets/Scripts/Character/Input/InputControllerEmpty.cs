/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Rewired;

namespace VoiceActing
{
    public class InputControllerEmpty : InputController
    {
       
        [Header("Debug")]
        [SerializeField]
        CharacterBase character = null;

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        protected override void Awake()
        {
            base.Awake();
            SetControllable(character);
        }

        protected override void Update()
        {
            UpdateBuffer();
            if (controllable != null)
                controllable.UpdateControl(this);
        }


        #endregion

    } // InputController class

} // #PROJECTNAME# namespace