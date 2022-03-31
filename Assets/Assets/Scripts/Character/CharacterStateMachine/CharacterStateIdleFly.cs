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
    public class CharacterStateIdleFly : CharacterStateIdle
    {
        [SerializeField]
        Vector2 flyHeight = new Vector2(1, 2);
        [SerializeField]
        float speedZ = 0.5f;

        float targetHeight = 0;

        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            base.StartState(character, oldState);

            targetHeight = Random.Range(1,2);
        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {
            if (character.CharacterMovement.PosZ < targetHeight)
            {
                character.CharacterMovement.SetSpeedZ(speedZ);
            }

            base.UpdateState(character);
        }

        
    } 

} // #PROJECTNAME# namespace