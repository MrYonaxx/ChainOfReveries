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
using Stats;

namespace VoiceActing
{

    [System.Serializable]
    public class StatusResistance : Stat
    {
        [SerializeField]
        [HorizontalGroup("StatusResistance")]
        [HideLabel]
        public StatusEffectData statusData;
        public StatusEffectData StatusData
        {
            get { return statusData; }
        }

        /*[HideLabel]
        [SerializeField]
        [HorizontalGroup("StatusResistance")]
        private int resistance;
        public int Resistance
        {
            get { return resistance; }
        }*/
    }



} // #PROJECTNAME# namespace