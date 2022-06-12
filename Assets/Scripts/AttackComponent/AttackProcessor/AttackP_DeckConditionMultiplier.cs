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
    public class AttackP_DeckConditionMultiplier: AttackProcessor
    {
        [SerializeField]
        float multiplierAttack = 1;
        [SerializeField]
        float multiplierMagic = 1;

        [SerializeField]
        bool buffFullAttackDeck = false;
        [SerializeField]
        bool buffFullMagicDeck = false;

        // Calcule des dommages
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            int nbAttack = 0;
            int nbMagic = 0;
            for (int i = 0; i < user.DeckController.Deck.Count; i++)
            {
                if (user.DeckController.Deck[i].GetCardType() == 0) // c'est attaque je me fais plus chier
                    nbAttack++;
                else if (user.DeckController.Deck[i].GetCardType() == 1)
                    nbMagic++;
            }

            if(buffFullAttackDeck)
            {
                if (attack.Card.GetCardType() == 0 && user.DeckController.Deck.Count == nbAttack+1)
                    damageMessage.damage += damageMessage.baseDamage * (multiplierAttack);
            }
            else if (buffFullMagicDeck)
            {
                if (attack.Card.GetCardType() == 1 && user.DeckController.Deck.Count == nbMagic+1)
                    damageMessage.damage += damageMessage.baseDamage * (multiplierAttack);
            }
            else
            {
                if (attack.Card.GetCardType() == 0)
                    damageMessage.damage += damageMessage.baseDamage * (multiplierAttack * nbAttack);
                if (attack.Card.GetCardType() == 1)
                    damageMessage.damage += damageMessage.baseDamage * (multiplierAttack * nbMagic);
            }


        }


    } 

} // #PROJECTNAME# namespace