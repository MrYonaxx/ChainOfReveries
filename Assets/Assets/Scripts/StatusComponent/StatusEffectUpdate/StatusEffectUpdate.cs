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
    public abstract class StatusEffectUpdate
    {

        public virtual StatusEffectUpdate Copy()
        {
            return null;
        }

        public virtual void ApplyUpdate(CharacterBase character)
        {

        }

        // Update le StatusUpdate, renvois faux si on doit Remove 
        public virtual bool Update()
        {
            return true;
        }

        public virtual void Remove(CharacterBase character)
        {

        }

        // La valeur utilisé quand on dessine une carte equipement équipé
        public virtual int ValueToDraw()
        {
            return 0;
        }

    } 

} // #PROJECTNAME# namespace