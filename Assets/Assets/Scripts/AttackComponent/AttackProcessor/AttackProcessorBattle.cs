using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public enum AttackProcessorEnum
    {
        CardDamage,
        CardStatus,
        CardPercentage,
        Element,
        Damage,
        BackDamage,
        BanDamage
    }

    // Comme Attack Processor Mono est un échec, je fais un nouveau attack processor qui hérite de monobehaviour et qui contient un enum pour parameter 
    // les attack processor
    // C'est 1000x + chiant puisque je suis dépendant d'un enum mais y'a pas le choix
    public class AttackProcessorBattle : MonoBehaviour
    {
        [SerializeField]
        [HideLabel]
        [OnValueChanged("ResetClass")]
        AttackProcessorEnum attackProcessor;

        [Space]
        [SerializeField]
        [HideLabel]
        [ShowIf("attackProcessor", AttackProcessorEnum.CardDamage)]
        AttackP_CardDamage cardDamage = null;

        [Space]
        [SerializeField]
        [HideLabel]
        [ShowIf("attackProcessor", AttackProcessorEnum.CardStatus)]
        AttackP_CardStatus cardStatus = null;

        [Space]
        [SerializeField]
        [HideLabel]
        [ShowIf("attackProcessor", AttackProcessorEnum.CardPercentage)]
        AttackP_CardPercentage cardPercentage = null;

        [Space]
        [SerializeField]
        [HideLabel]
        [ShowIf("attackProcessor", AttackProcessorEnum.Element)]
        AttackP_Element element = null;

        [Space]
        [SerializeField]
        [HideLabel]
        [ShowIf("attackProcessor", AttackProcessorEnum.Damage)]
        AttackP_Damage damage = null;

        [Space]
        [SerializeField]
        [HideLabel]
        [ShowIf("attackProcessor", AttackProcessorEnum.BackDamage)]
        AttackP_BackDamage backdamage = null;

        [Space]
        [SerializeField]
        [HideLabel]
        [ShowIf("attackProcessor", AttackProcessorEnum.BanDamage)]
        AttackP_BanDamage banDamage = null;

        public AttackProcessor GetAttackProcessor()
        {
            switch(attackProcessor)
            {
                case AttackProcessorEnum.CardDamage:
                    return cardDamage;
                case AttackProcessorEnum.CardStatus:
                    return cardStatus;
                case AttackProcessorEnum.CardPercentage:
                    return cardPercentage;
                case AttackProcessorEnum.Element:
                    return element;
                case AttackProcessorEnum.Damage:
                    return damage;
                case AttackProcessorEnum.BackDamage:
                    return backdamage;
                case AttackProcessorEnum.BanDamage:
                    return banDamage;
            }
            return null;
        }

        // Pour pas serializer des trucs inutiles mais jsuis pas sur que ça marche avec SerializeField
        private void ResetClass()
        {
            cardDamage = null;
            cardStatus = null;
            cardPercentage = null;
            element = null;
            damage = null;
            backdamage = null; 
            banDamage = null;
        }
    }
}
