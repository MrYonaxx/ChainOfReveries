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
    public class StatusEffectDoom : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        public int Counter = 99;
        [SerializeField]
        public CardBreakController CardBreakController = null;
        [SerializeField]
        public GameObject TextPrefab = null;
        [SerializeField]
        public CardData AttackDoom = null;

        CharacterBase owner = null;
        CharacterBase enemy = null;
        TMPro.TextMeshPro doomCounter = null;
        bool action = false;
        bool disappear = false;
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
        public StatusEffectDoom()
        {

        }
        public StatusEffectDoom(StatusEffectDoom data)
        {
            CardBreakController = data.CardBreakController;
            Counter = data.Counter;
            TextPrefab = data.TextPrefab;
            AttackDoom = data.AttackDoom;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectDoom(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            disappear = false;
               owner = character;
            CardBreakController.OnCardBreak += DoomDecrement;

            doomCounter = character.CreateAnimation(TextPrefab).GetComponent<TMPro.TextMeshPro>();
            doomCounter.transform.SetParent(character.ParticlePoint);
            doomCounter.text = Counter.ToString();
        }

        public void DoomDecrement(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker)
        {
            if (cardBreaker == null)
                return;
            if(characterBreaked == owner) // si le perso qui se fait breaker est le notre
            {
                int sumActive = 0;
                int sumNewCards = 0;

                for (int i = 0; i < cardBreaked.Count; i++)
                    sumActive += cardBreaked[i].GetCardValue();
                for (int i = 0; i < cardBreaker.Count; i++)
                    sumNewCards += cardBreaker[i].GetCardValue();

                int difference = sumNewCards - sumActive;

                Counter -= Mathf.Abs(difference);

                if(Counter <= 0)
                {
                    action = true;
                    enemy = characterBreaker;
                    Counter = 0;
                }

                // On dessine
                doomCounter.text = Counter.ToString();
            }

        }

        public override void UpdateEffect(CharacterBase character)
        {
            base.UpdateEffect(character);
            if(action)
            {
                enemy.CharacterAction.Action(new Card(AttackDoom, 6));
                action = false; 
                CardBreakController.OnCardBreak -= DoomDecrement;
                GameObject.Destroy(doomCounter.gameObject);
                disappear = true;
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            if (disappear == false)
            {
                CardBreakController.OnCardBreak -= DoomDecrement;
                GameObject.Destroy(doomCounter.gameObject);
            }
        }

        #endregion

    }

} // #PROJECTNAME# namespace