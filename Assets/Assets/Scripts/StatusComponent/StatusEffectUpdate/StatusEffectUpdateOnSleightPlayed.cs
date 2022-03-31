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
    public class StatusEffectUpdateOnSleightPlayed: StatusEffectUpdate
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public int playTime = 1;


        int play = 0;

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
        public StatusEffectUpdateOnSleightPlayed()
        {

        }

        public StatusEffectUpdateOnSleightPlayed(StatusEffectUpdateOnSleightPlayed statusEffect)
        {
            play = statusEffect.playTime;
        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateOnSleightPlayed(this);
        }

        CharacterBase c; // tout allait bien, puis le système s'effondre, donc obligé de garder une référence du player ici pour
                            // dessiner les status
        public override void ApplyUpdate(CharacterBase character)
        {
            c = character;
            character.CharacterAction.OnSleightPlayed += UpdateCall;
        }

        public override bool Update()
        {
            if (play <= 0)
            {
                return false;
            }
            return true;
        }

        public override void Remove(CharacterBase character)
        {
            character.CharacterAction.OnSleightPlayed -= UpdateCall;
        }

        public override int ValueToDraw()
        {
            return play;
        }



        public void UpdateCall(AttackManager attack, Card card)
        {
            if (card == null)
                return;
            play -= 1;
            c.CharacterStatusController.RefreshStatus();
        }





        #endregion

    }

} // #PROJECTNAME# namespace