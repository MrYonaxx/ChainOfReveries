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
    public class StatusEffectUpdateCounter : StatusEffectUpdate
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
        public bool decrementOnHit = false;

        CharacterBase owner = null;
        TMPro.TextMeshPro doomCounter = null;

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
        public StatusEffectUpdateCounter()
        {

        }
        public StatusEffectUpdateCounter(StatusEffectUpdateCounter data)
        {
            CardBreakController = data.CardBreakController;
            Counter = data.Counter;
            TextPrefab = data.TextPrefab;
        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateCounter(this);
        }

        public override void ApplyUpdate(CharacterBase character)
        {
            owner = character;
            CardBreakController.OnCardBreak += DoomDecrement;

            doomCounter = character.CreateAnimation(TextPrefab).GetComponent<TMPro.TextMeshPro>();
            doomCounter.transform.SetParent(character.ParticlePoint);
            doomCounter.text = Counter.ToString();

            if (decrementOnHit)
                character.CharacterKnockback.OnHit += HitDecrement;
        }

        public override bool Update()
        {
            if (Counter <= 0)
            {
                return false;
            }
            return true;
        }

        public override void Remove(CharacterBase character)
        {
            CardBreakController.OnCardBreak -= DoomDecrement;
            if (decrementOnHit)
                character.CharacterKnockback.OnHit -= HitDecrement;
            GameObject.Destroy(doomCounter.gameObject);
        }

        public override int ValueToDraw()
        {
            return Counter;
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
                    Counter = 0;

                // On dessine
                doomCounter.text = Counter.ToString();
            }

        }
        public void HitDecrement(DamageMessage dmgMsg)
        {
            Counter -= 1;

            if (Counter <= 0)
                Counter = 0;

            // On dessine
            doomCounter.text = Counter.ToString();

        }

        #endregion

    }

} // #PROJECTNAME# namespace