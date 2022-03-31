/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class AnimationEvent: MonoBehaviour
    {

        [SerializeField]
        UnityEvent unityEvent;
        [SerializeField]
        UnityEvent unityEvent2;

        public void CallEvent()
        {
            unityEvent.Invoke();
        }
        public void CallEvent2()
        {
            unityEvent2.Invoke();
        }


    } 

} // #PROJECTNAME# namespace