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
    public class StatusEffectLemina : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        public CardBreakController CardBreakController = null;
        [SerializeField]
        public GaugeDrawer GaugeInstall = null;
        [SerializeField]
        public CardData AttackInstall = null;
        [SerializeField]
        public StatusEffectData StatusInstall = null;

        int Meter = 99;
        int MaxMeter = 99;

        CharacterBase owner = null;

        GaugeDrawer gaugeMeter = null;
        bool etherInstall = false;
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
        public StatusEffectLemina()
        {

        }
        public StatusEffectLemina(StatusEffectLemina data)
        {
            CardBreakController = data.CardBreakController;
            GaugeInstall = data.GaugeInstall;
            AttackInstall = data.AttackInstall;
            StatusInstall = data.StatusInstall;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectLemina(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            disappear = false;
            owner = character;
            character.OnBattleStart += InitializeInstall;
            character.OnBattleEnd += ResetInstall;
            CardBreakController.OnCardBreak += CardBreakInstall;

            gaugeMeter = character.CreateAnimation(GaugeInstall.gameObject).GetComponent<GaugeDrawer>();

            InitializeInstall();
        }

        void InitializeInstall()
        {
            Meter = 0;
            MaxMeter = owner.DeckController.DeckData.Count;
            gaugeMeter.DrawGauge(Meter, MaxMeter);
        }

        void ResetInstall()
        {

        }

        public void AddEtherGauge(int amount)
        {
            Meter += amount; 
            // On dessine
            gaugeMeter.DrawGauge(Meter, MaxMeter);
        }

        public void CardBreakInstall(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker)
        {
            if (cardBreaker == null)
                return;
            if (characterBreaker == owner) // si le perso qui break est le notre
            {
                int sumActive = 0;
                int sumNewCards = 0;

                for (int i = 0; i < cardBreaked.Count; i++)
                    sumActive += cardBreaked[i].GetCardValue();
                for (int i = 0; i < cardBreaker.Count; i++)
                    sumNewCards += cardBreaker[i].GetCardValue();

                int difference = sumNewCards - sumActive;

                Meter += Mathf.Abs(difference);

                if(Meter >= MaxMeter) // On entre en install
                {
                    action = true;
                    cardBreaker.Clear();
                    cardBreaker.Add(new Card(AttackInstall, 0));
                    //characterBreaker.CharacterStatusController.ApplyStatus(StatusInstall, 1000);
                    etherInstall = true;
                    Meter = 0;
                }

                // On dessine
                gaugeMeter.DrawGauge(Meter, MaxMeter);
            }

        }

        public override void UpdateEffect(CharacterBase character)
        {
            base.UpdateEffect(character);
            if(etherInstall)
            {
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            character.OnBattleStart -= InitializeInstall;
            character.OnBattleEnd -= ResetInstall;
            CardBreakController.OnCardBreak -= CardBreakInstall;

        }

        #endregion

    }

} // #PROJECTNAME# namespace