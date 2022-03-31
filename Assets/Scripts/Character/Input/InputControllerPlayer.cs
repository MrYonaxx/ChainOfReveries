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

        [Header("Assign Settings")]

        [SerializeField] // J1 ou J2, la variable id de inputController est l'id manette
        int playerID = 0;
        [SerializeField]
        bool useGameSettings = false;
        [SerializeField]
        [ShowIf("useGameSettings")]
        GameData gameSettings = null;

        [Header("Gameplay")]
        [SerializeField]
        bool leftPlayer = true;


        [Header("Debug")]
        [SerializeField]
        CharacterBase debugPlayer = null;

        InputConfig inputConfigDefault = new InputConfig();
        InputConfig inputConfig = new InputConfig();
        int buttonId = 0;

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        protected override void Awake()
        {
            if (debugPlayer != null)
                SetControllable(debugPlayer);

            if (gameSettings != null)
            {
                SetID(gameSettings.GetControllerID(playerID)); // Set le controller pour qu'il ait l'ID correspondant
                inputConfig = gameSettings.GetInputConfig(playerID);
            }
            else
            {
                SetID(playerID); // Sinon J1 = Manette1, J2 = Manette2 etc...
            }

            // Initialise les structures pour les inputs
            base.Awake();
        }

        public override void SetControllable(IControllable newControllable, bool menu = false)
        {
            controllable = newControllable;

            // Set les input config en fonction de si c'est un menu
            if (menu)
            {
                inputConfig = inputConfigDefault;
            }
            else if (gameSettings != null)
            {
                inputConfig = gameSettings.GetInputConfig(playerID);
            }
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
            UpdateInputButton(InputEnum.A, inputA);
            UpdateInputButton(InputEnum.B, inputB);
            UpdateInputButton(InputEnum.X, inputX);
            UpdateInputButton(InputEnum.Y, inputY);
            if(leftPlayer)
            {
                UpdateInputButton(InputEnum.RB, inputRB);
                UpdateInputButton(InputEnum.LB, inputLB);
                UpdateInputButton(InputEnum.RT, inputRT);
                UpdateInputButton(InputEnum.LT, inputLT);
            }
            else
            {
                UpdateInputButton(InputEnum.LB, inputRB);
                UpdateInputButton(InputEnum.RB, inputLB);
                UpdateInputButton(InputEnum.RT, inputLT);
                UpdateInputButton(InputEnum.LT, inputRT);
            }

            UpdateInputButton(InputEnum.DPAD_DOWN, inputPadDown);
            UpdateInputButton(InputEnum.DPAD_UP, inputPadUp);
            UpdateInputButton(InputEnum.DPAD_LEFT, inputPadLeft);
            UpdateInputButton(InputEnum.DPAD_RIGHT, inputPadRight);


            UpdateInputButton(InputEnum.START, inputStart);

            // Stick
            if (Mathf.Abs(player.GetAxis("Vertical")) >= joystickThreshold)
                inputLeftStickY.InputValue = player.GetAxis("Vertical");
            else
                inputLeftStickY.InputValue = 0;

            if (Mathf.Abs(player.GetAxis("Horizontal")) >= joystickThreshold)
                inputLeftStickX.InputValue = player.GetAxis("Horizontal");
            else
                inputLeftStickX.InputValue = 0;
        }

        private void UpdateInputButton(InputEnum input, InputBuffer inputBuffer)
        {
            buttonId = inputConfig.GetInput(input);

            // Detection pression bouton
            if (player.GetButtonDown(buttonId))
                inputBuffer.BufferDownInput(bufferTime);
            else if (player.GetButtonUp(buttonId))
                inputBuffer.BufferUpInput(bufferTime);

            // Detection bouton enfoncé
            inputBuffer.InputValue = 0;
            if (player.GetButton(buttonId))
            {
                inputBuffer.InputValue = 1;
            }
        }


        #endregion

    } // InputController class

} // #PROJECTNAME# namespace