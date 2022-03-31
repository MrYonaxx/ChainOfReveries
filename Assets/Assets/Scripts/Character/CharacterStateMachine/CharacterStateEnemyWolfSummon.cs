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
    // state particulier pour les loup fantome du jabberwock
    public class AttackC_SubEnemyController : AttackComponent
    {

        // Le jabberwock summoner
        [SerializeField]
        AttackManager attack = null;

        [SerializeField]
        GameObject animationSpawn = null;
        [SerializeField]
        CharacterAnimationData[] wolfAttack = null;

        int i = 0;


        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
        }

        public override void UpdateComponent(CharacterBase character)
        {
            wolfAttack[i].UpdateAnimation(character, attack.MotionSpeed) ;
        }


    } 

} // #PROJECTNAME# namespace