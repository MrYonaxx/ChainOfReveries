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
    public class InputControllerPlayer : InputController
    {
       
        Rewired.Player player;


        [SerializeField]
        int playerID = 0;
        [SerializeField]
        bool leftPlayer = true;

        [Header("Debug")]
        [SerializeField]
        CharacterBase debugPlayer = null;

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        protected override void Start()
        {
            if (debugPlayer != null)
                SetControllable(debugPlayer);

            SetID(playerID);
            base.Start();
        }

        public override void SetID(int newId)
        {
            id = newId;
            if (id < 0)
                player = null;
            else
                player = ReInput.players.GetPlayer(id);
        }

        protected override void Update()
        {
            if (player == null)
                return;

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
            if(leftPlayer)
            {
                UpdateInputButton("RB", inputRB);
                UpdateInputButton("LB", inputLB);
            }
            else
            {
                UpdateInputButton("LB", inputRB);
                UpdateInputButton("RB", inputLB);
            }
            UpdateInputButton("RT", inputRT);
            UpdateInputButton("LT", inputLT);

            UpdateInputButton("DpadDown", inputPadDown);
            UpdateInputButton("DpadUp", inputPadUp);

            if (Mathf.Abs(player.GetAxis("Vertical")) >= joystickThreshold)
            {
                inputLeftStickY.InputValue = player.GetAxis("Vertical");
            }
            else
            {
                inputLeftStickY.InputValue = 0;
            }

            if (Mathf.Abs(player.GetAxis("Horizontal")) >= joystickThreshold)
            {
                inputLeftStickX.InputValue = player.GetAxis("Horizontal");
            }
            else
            {
                inputLeftStickX.InputValue = 0;
            }


           /* if (player.GetAxis("RT") < -triggerThreshold && inputRightTrigger == false)
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