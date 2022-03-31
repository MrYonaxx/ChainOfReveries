using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing // 1 dollar à chaque fois que je le remet parce que flemme de modifier tout les script
{
    public enum InputEnum
    { 
        HORIZONTAL,
        VERTICAL,

        A,
        B,
        X,
        Y,

        RB,
        LB,
        RT,
        LT,

        R_HORIZONTAL,
        L_HORIZONTAL,

        DPAD_DOWN,
        DPAD_UP,

        DPAD_LEFT,
        DPAD_RIGHT,

        START
    }

    [System.Serializable]
    public class InputConfig
    {
        [SerializeField]
        public int[] Config = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };


        public void Reset()
        {
            Config = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        }

        // Prend un input et le transforme en fonction de la config
        public int GetInput(InputEnum input)
        {
            return Config[(int)input];
        }

        public void ChangeInput(int inputID, int newInput)
        {
            int tmp = Config[inputID];
            for (int i = 0; i < Config.Length; i++)
            {
                if(Config[i] == newInput)
                {
                    Config[i] = tmp;
                    break;
                }
            }
            Config[inputID] = newInput;

        }

    }
}
