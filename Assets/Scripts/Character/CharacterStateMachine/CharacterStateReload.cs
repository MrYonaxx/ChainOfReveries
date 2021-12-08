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

    public class CharacterStateReload: CharacterState
    {

        //[SerializeField]


        public override void StartState(CharacterBase character, CharacterState oldState)
        {

        }

        public override void UpdateState(CharacterBase character)
        {
            character.CharacterMovement.ApplyGravity(character.MotionSpeed);
            character.CharacterMovement.Move(0, 0);

            if (character.DeckController.AddReload(character.CharacterStat.ReloadAmount.Value) )
            {
                character.ResetToIdle();
            }

            
            if (character.Inputs.InputA.InputValue == 0)
            {
                // Todo : Ajouter du lag pour balance en pvp
                character.ResetToIdle();
            }

        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {

        }


    } 

} // #PROJECTNAME# namespace