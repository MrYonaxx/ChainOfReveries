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
    /// <summary>
    /// Definition of the InputController class
    /// </summary>
    public class InputControllerDebug : InputController
    {
       
        Rewired.Player player;

        [SerializeField]
        int playerID = 0;

        [Header("Debug")]
        [SerializeField]
        CharacterBase debugPlayer = null;
        [SerializeField]
        InputController inputPlayer = null;

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        protected override void Awake()
        {
            if (debugPlayer != null)
                SetControllable(debugPlayer);
            player = ReInput.players.GetPlayer(playerID);
            if(inputPlayer != null)
                inputPlayer.SetControllable(null);
            base.Awake();
        }

        protected override void Update()
        {
            UpdateBuffer();
            UpdateInput();
            if (controllable != null)
                controllable.UpdateControl(this);
        }





        private void UpdateInput()
        {
            UpdateInputButton("A", inputA);
            UpdateInputButton("B", inputB);
            UpdateInputButton("X", inputX);
            UpdateInputButton("Y", inputY);
            UpdateInputButton("RT", inputLB);
            UpdateInputButton("LT", inputRB);

            if (Mathf.Abs(player.GetAxis("RightVertical")) >= joystickThreshold)
            {
                inputLeftStickY.InputValue = player.GetAxis("RightVertical");
            }
            else
            {
                inputLeftStickY.InputValue = 0;
            }

            if (Mathf.Abs(player.GetAxis("RightHorizontal")) >= joystickThreshold)
            {
                inputLeftStickX.InputValue = player.GetAxis("RightHorizontal");
            }
            else
            {
                inputLeftStickX.InputValue = 0;
            }
            

           /*if (player.GetAxis("RT") < -triggerThreshold && inputRightTrigger == false)
            {
                inputRT.InputValue = (int)Mathf.Abs(Input.GetAxis("ControllerTriggers"));
                inputRT.BufferDownInput(bufferTime);
                inputRightTrigger = true;
            }
            else if (player.GetAxis("RT") < -triggerThreshold)
            {
                inputRT.InputValue = (int)Mathf.Abs(Input.GetAxis("ControllerTriggers"));
            }
            else if (player.GetAxis("RT") >= -triggerThreshold)
            {
                inputRightTrigger = false;
            }


            if (player.GetAxis("ControllerTriggers") > triggerThreshold && inputLeftTrigger == false)
            {
                inputLT.InputValue = (int)Mathf.Abs(Input.GetAxis("ControllerTriggers"));
                inputLT.BufferDownInput(bufferTime);
                inputLeftTrigger = true;
            }
            else if (player.GetAxis("ControllerTriggers") > triggerThreshold)
            {
                inputLT.InputValue = (int)Mathf.Abs(Input.GetAxis("ControllerTriggers"));
            }
            else if (player.GetAxis("ControllerTriggers") <= triggerThreshold)
            {
                inputLeftTrigger = false;
            }*/
        }

        private void UpdateInputButton(string buttonName, InputBuffer input)
        {
            if (player.GetButtonDown(buttonName))
                input.BufferDownInput(bufferTime);
            else if (player.GetButtonUp(buttonName))
                input.BufferUpInput(bufferTime);

            input.InputValue = 0;
            if (player.GetButton(buttonName))
            {
                input.InputValue = 1;
            }
        }


        #endregion

    } // InputController class

} // #PROJECTNAME# namespace