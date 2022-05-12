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
    public class AttackP_BannedCardMultiplier: AttackProcessor
    {
        [SerializeField]
        float multiplierByBan = 0.02f;

        // Calcule des dommages
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            damageMessage.damage += damageMessage.baseDamage * (user.DeckController.GetBanDeck().Count * multiplierByBan);

        }


    } 

} // #PROJECTNAME# namespace