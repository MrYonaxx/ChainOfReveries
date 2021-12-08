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
    public class AttackC_Particle: AttackComponent
    {
        [SerializeField]
        GameObject particle;


        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            if(particle != null)
                Instantiate(particle, character.ParticlePoint.transform.position, Quaternion.identity).transform.SetParent(character.ParticlePoint.transform);

            /*if (character.CharacterAction.PreviousAttackManager != null)
            {
                BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.1f, 0.6f);
                character.SetCharacterMotionSpeed(0.2f, 0.3f);
                BattleFeedbackManager.Instance.CameraSpecialZoom();
            }*/

        }


        /* public override void UpdateComponent(CharacterBase character)
         {

         }
         public override void OnHitComponent(CharacterBase character, CharacterBase target)
         {

         }
         public override void EndComponent(CharacterBase character)
         {

         }*/

    } 

} // #PROJECTNAME# namespace