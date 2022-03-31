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
    public class TriggerEvent: MonoBehaviour
    {
        [SerializeField]
        UnityEvent triggerEvent;
        bool on = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player") && on == false)
            {
                triggerEvent.Invoke();
                on = true;
                //this.gameObject.SetActive(false);
            }
        }

    } 

} // #PROJECTNAME# namespace