using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing 
{
    public interface IControllable
    {
        void UpdateControl(InputController inputs);
    }
}
