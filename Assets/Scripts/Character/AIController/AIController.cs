using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using Rewired;

namespace VoiceActing
{
    public class AIController : InputController
    {


        BehaviorTree behaviorTree = null;

        [Title("AI")]
        [SerializeField]
        bool aiEnabled = true;
        [SerializeField]
        private CharacterBase character;
        public CharacterBase Character
        {
            get { return character; }
            set { character = value; }
        }




        void Awake()
        {
            behaviorTree = GetComponent<BehaviorTree>();
            behaviorTree.SetVariableValue("AI", this);
            if (aiEnabled)
            {
                StartBehavior();
            }
            player = ReInput.players.GetPlayer(0);
            SetControllable(character);
        }

        public void StartBehavior()
        {
            behaviorTree.enabled = true;
        }


        protected override void Update()
        {
            UpdateBuffer();
            if (debugOn)
                UpdateInput();
            if (controllable != null)
                controllable.UpdateControl(this);
        }




        // =========================================================================
        [Title("Debug")]
        [SerializeField]
        bool debugOn = false;

        Player player;


        private void UpdateInput()
        {
            UpdateInputButton("B", inputA);

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


    }
}
