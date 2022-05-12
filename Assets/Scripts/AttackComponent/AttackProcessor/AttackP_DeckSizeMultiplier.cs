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
    [System.Serializable]
    public class AttackP_DeckSizeMultiplier: AttackProcessor
    {
        [SerializeField]
        float multiplierLower = 0.5f;
        [SerializeField]
        float multiplierHigher = -0.5f;

        [SerializeField]
        int deckSizeCondition = 20;


        // Calcule des dommages
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            if(user.DeckController.DeckData.Count <= deckSizeCondition)
            {
                damageMessage.damage += damageMessage.baseDamage * multiplierLower;
            }
            else
            {
                damageMessage.damage += damageMessage.baseDamage * multiplierHigher;
            }
        }


    } 

} // #PROJECTNAME# namespace