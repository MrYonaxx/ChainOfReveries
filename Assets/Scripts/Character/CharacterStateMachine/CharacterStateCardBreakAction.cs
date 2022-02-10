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
    public class CharacterStateCardBreakAction : CharacterState
    {

        [SerializeField]
        AttackManager attack = null;



        float t = 0f;


        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterAction.CancelAction();
            character.CharacterAction.Action(attack);
        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {

        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterKnockback.ReversalTime = 0;
        }



    } 

} // #PROJECTNAME# namespace