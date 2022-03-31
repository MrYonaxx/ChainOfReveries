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
    public class StatusEffectUpdateTime: StatusEffectUpdate
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        [LabelWidth(150)]
        public Vector2Int timeData;

        float time = 0;

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectUpdateTime()
        {
            
        }

        public StatusEffectUpdateTime(StatusEffectUpdateTime statusEffect)
        {
            time = Random.Range(statusEffect.timeData.x, statusEffect.timeData.y);
        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateTime(this);
        }

        public override bool Update()
        {
            time -= Time.deltaTime;
            if (time <= 0)
                return false;
            return true;

        }

        #endregion

    } 

} // #PROJECTNAME# namespace