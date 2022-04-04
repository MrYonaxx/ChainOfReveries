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
    /// /!\ La prochain fois faire un Circular buffer pour que inputController prenne moins d'espace
    /// </summary>
    public class InputController : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        protected float joystickThreshold = 0.25f;
        [SerializeField]
        protected float triggerThreshold = 0.5f;
        [SerializeField]
        protected float bufferTime = 0.25f;

        
        protected int id;
        public int Id
        {
            get { return id; }
        }

        protected InputBuffer inputLeftStickX;
        public InputBuffer InputLeftStickX
        {
            get { return inputLeftStickX; }
        }

        protected InputBuffer inputLeftStickY;
        public InputBuffer InputLeftStickY
        {
            get { return inputLeftStickY; }
        }



        protected InputBuffer inputA; 
        public InputBuffer InputA
        {
            get { return inputA; }
        }

        protected InputBuffer inputB;
        public InputBuffer InputB
        {
            get { return inputB; }
        }

        protected InputBuffer inputX;
        public InputBuffer InputX
        {
            get { return inputX; }
        }

        protected InputBuffer inputY; 
        public InputBuffer InputY
        {
            get { return inputY; }
        }


        protected InputBuffer inputRB;
        public InputBuffer InputRB
        {
            get { return inputRB; }
        }

        protected InputBuffer inputLB;
        public InputBuffer InputLB
        {
            get { return inputLB; }
        }

        protected InputBuffer inputRT;
        public InputBuffer InputRT
        {
            get { return inputRT; }
        }

        protected InputBuffer inputLT;
        public InputBuffer InputLT
        {
            get { return inputLT; }
        }


        protected InputBuffer inputPadDown;
        public InputBuffer InputPadDown
        {
            get { return inputPadDown; }
        }

        protected InputBuffer inputPadUp;
        public InputBuffer InputPadUp
        {
            get { return inputPadUp; }
        }

        protected InputBuffer inputPadLeft;
        public InputBuffer InputPadLeft
        {
            get { return inputPadLeft; }
        }

        protected InputBuffer inputPadRight;
        public InputBuffer InputPadRight
        {
            get { return inputPadRight; }
        }

        protected InputBuffer inputStart;
        public InputBuffer InputStart
        {
            get { return inputStart; }
        }

        protected IControllable controllable;
        public IControllable Controllable
        {
            get { return controllable; }
        }


        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public virtual void SetControllable(IControllable newControllable, bool menu = false)
        {
            controllable = newControllable;
        }

        public virtual void SetID(int newId)
        {
            id = newId;
        }

        protected virtual void Awake()
        {
            inputLeftStickX = new InputBuffer();
            inputLeftStickY = new InputBuffer();

            inputA = new InputBuffer();
            inputB = new InputBuffer();
            inputX = new InputBuffer();
            inputY = new InputBuffer();

            inputRB = new InputBuffer();
            inputLB = new InputBuffer();

            inputRT = new InputBuffer();
            inputLT = new InputBuffer();


            inputPadDown = new InputBuffer();
            inputPadUp = new InputBuffer();
            inputPadLeft = new InputBuffer();
            inputPadRight = new InputBuffer();

            inputStart = new InputBuffer();
        }

        protected virtual void Update()
        {
            UpdateBuffer();
            if (controllable != null)
                controllable.UpdateControl(this);
        }


        protected void UpdateBuffer()
        {
            inputA.BufferTime -= Time.deltaTime;
            inputB.BufferTime -= Time.deltaTime;
            inputX.BufferTime -= Time.deltaTime;
            inputY.BufferTime -= Time.deltaTime;

            inputRB.BufferTime -= Time.deltaTime;
            inputLB.BufferTime -= Time.deltaTime;

            inputRT.BufferTime -= Time.deltaTime;
            inputLT.BufferTime -= Time.deltaTime;

            inputPadDown.BufferTime -= Time.deltaTime;
            inputPadUp.BufferTime -= Time.deltaTime;
            inputPadLeft.BufferTime -= Time.deltaTime;
            inputPadRight.BufferTime -= Time.deltaTime;

            inputStart.BufferTime -= Time.deltaTime;
        }

        public void ResetAllBuffer(bool joystickOff = false)
        {
            inputA.ResetBuffer();
            inputB.ResetBuffer();
            inputX.ResetBuffer();
            inputY.ResetBuffer();

            inputRB.ResetBuffer();
            inputLB.ResetBuffer();

            inputRT.ResetBuffer();
            inputLT.ResetBuffer();

            inputPadDown.ResetBuffer();
            inputPadUp.ResetBuffer();
            inputPadLeft.ResetBuffer();
            inputPadRight.ResetBuffer();

            inputStart.ResetBuffer();

            if (joystickOff)
            {
                inputLeftStickX.InputValue = 0;
                inputLeftStickY.InputValue = 0;
            }      
        }

        public void ResetAllValue(bool joystickOff = false)
        {
            inputA.ResetValue();
            inputB.ResetValue();
            inputX.ResetValue();
            inputY.ResetValue();

            inputRB.ResetValue();
            inputLB.ResetValue();

            inputRT.ResetValue();
            inputLT.ResetValue();

            inputPadDown.ResetValue();
            inputPadUp.ResetValue();
            inputPadLeft.ResetValue();
            inputPadRight.ResetValue();

            inputStart.ResetValue();

            if (joystickOff)
            {
                inputLeftStickX.InputValue = 0;
                inputLeftStickY.InputValue = 0;
            }
        }

        public void PressButton(InputBuffer input)
        {
            input.BufferDownInput(bufferTime);
        }
        public void HoldButton(InputBuffer input)
        {
            input.InputValue = 1;
        }
        public void ReleaseButton(InputBuffer input)
        {
            input.InputValue = 0;
        }

        public bool GetInput(InputEnum inputEnum)
        {
            switch(inputEnum)
            {
                case InputEnum.A:
                    return inputA.Registered;
                case InputEnum.B:
                    return inputB.Registered;
                case InputEnum.X:
                    return inputX.Registered;
                case InputEnum.Y:
                    return inputY.Registered;
                case InputEnum.RB:
                    return inputRB.Registered;
                case InputEnum.RT:
                    return inputRT.Registered;
                case InputEnum.LB:
                    return inputLB.Registered;
                case InputEnum.LT:
                    return inputLT.Registered;
                case InputEnum.DPAD_DOWN:
                    return inputPadDown.Registered;
                case InputEnum.DPAD_UP:
                    return inputPadUp.Registered;
                case InputEnum.DPAD_LEFT:
                    return inputPadLeft.Registered;
                case InputEnum.DPAD_RIGHT:
                    return inputPadRight.Registered;
            }
            return false;
        }

        #endregion

    } // InputController class

} // #PROJECTNAME# namespace