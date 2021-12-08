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
    public class Status
    {
        public StatusEffectData StatusEffect;
        public List<StatusEffect> StatusController = new List<StatusEffect>();
        public List<StatusEffectUpdate> StatusUpdate = new List<StatusEffectUpdate>();

        public Status(StatusEffectData statusEffectData)
        {
            StatusEffect = statusEffectData;

            for (int i = 0; i < statusEffectData.StatusController.Count; i++)
                StatusController.Add(statusEffectData.StatusController[i].Copy());

            for (int i = 0; i < statusEffectData.StatusUpdates.Count; i++)
                StatusUpdate.Add(statusEffectData.StatusUpdates[i].Copy());
        }
    }


} // #PROJECTNAME# namespace