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
    public class StatusEffectUpdateOnReload: StatusEffectUpdate
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public int NumberOfReload = 1;

        int currentNbReload = 0;
        bool canRemove = false;

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectUpdateOnReload()
        {
            
        }

        public StatusEffectUpdateOnReload(StatusEffectUpdateOnReload data)
        {
            NumberOfReload = data.NumberOfReload;
            currentNbReload = 0;
            canRemove = false;
        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateOnReload(this);
        }




        public override void ApplyUpdate(CharacterBase character)
        {
            character.DeckController.OnReload += ReloadCallback;
        }

        public override bool Update()
        {
            return !canRemove;
        }

        public override void Remove(CharacterBase character)
        {
            character.DeckController.OnReload -= ReloadCallback;
        }


        public override int ValueToDraw()
        {
            return (NumberOfReload - currentNbReload);
        }

        /*public override int LabelToDraw()
        {
            return "Reload";
        }*/

        private void ReloadCallback()
        {
            currentNbReload++;
            if (currentNbReload >= NumberOfReload)
                canRemove = true;
        }



        #endregion

    }

} // #PROJECTNAME# namespace