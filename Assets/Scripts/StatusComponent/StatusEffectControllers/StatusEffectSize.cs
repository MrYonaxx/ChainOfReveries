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
    public class StatusEffectSize : StatusEffect
    {
        #region Attributes 
        public float Size = 1;
        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectSize()
        {
        }
        public StatusEffectSize(StatusEffectSize data)
        {
            Size = data.Size;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectSize(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            // Manoeuvre un peu chelou parce que si on fait +1 quand le scale vaut -1 on mets la scale à 0
            if(character.transform.localScale.x > 0)
                character.transform.localScale += new Vector3(Size, Size, 0);
            else
                character.transform.localScale += new Vector3(-Size, Size, 0);
        }

        public override void RemoveEffect(CharacterBase character)
        {
            // Manoeuvre un peu chelou parce que si on fait +1 quand le scale vaut -1 on mets la scale à 0
            if (character.transform.localScale.x > 0)
                character.transform.localScale -= new Vector3(Size, Size, 0);
            else
                character.transform.localScale -= new Vector3(-Size, Size, 0);
        }


        #endregion

    } 

} // #PROJECTNAME# namespace