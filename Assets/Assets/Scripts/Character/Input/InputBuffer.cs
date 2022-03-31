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
    public class InputBuffer
    {
        private float inputValue = 0;
        public float InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        private float bufferTime = 0;
        public float BufferTime
        {
            get { return bufferTime; }
            set { bufferTime = Mathf.Max(value, 0); }
        }

        public bool Registered
        {
            get { return bufferTime > 0; }
        }


        public InputBuffer()
        {
            inputValue = 0;
            bufferTime = 0;
        }

        public void BufferDownInput(float time)
        {
            bufferTime = time;
        }

        public void BufferUpInput(float time)
        {
            //bufferTime = time;
        }

        public void ResetBuffer()
        {
            bufferTime = 0;
        }

    } 

} // #PROJECTNAME# namespace