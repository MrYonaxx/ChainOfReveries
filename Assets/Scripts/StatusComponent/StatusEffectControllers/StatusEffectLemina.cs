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
        public CardData CardInstall = null;
        [SerializeField]
        public AttackManager AttackUninstall = null;

        int Meter = 99;
        int MaxMeter = 99;

        CharacterBase owner = null;

        GaugeDrawer gaugeMeter = null;
        bool inInstall = false;
        bool inInstallReady = false;
        bool inUninstallReady = false;
        bool firstReload = false;
        bool inBattle = false;
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
            CardInstall = data.CardInstall;
            AttackUninstall = data.AttackUninstall;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectLemina(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            owner = character;
            character.OnBattleStart += InitializeInstall;
            character.OnBattleEnd += ResetInstall;

            character.DeckController.OnReload += ReloadInstall;
            character.CharacterAction.OnAction += AttackInstall;
            character.CharacterAction.OnAttackHit += AttackHitInstall;
            character.CharacterKnockback.OnHit += HitInstall;

            CardBreakController.OnCardBreak += CardBreakInstall;

            gaugeMeter = character.CreateAnimation(GaugeInstall.gameObject).GetComponent<GaugeDrawer>();
            if(character.tag == "Enemy")
            {
                gaugeMeter.transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
            }

            Meter = 0;
            InitializeInstall();
        }


        // Début du combat
        void InitializeInstall()
        {
            Meter = Mathf.Min(Meter, MaxMeter - 1);
            MaxMeter = owner.DeckController.DeckData.Count;
            gaugeMeter.DrawGauge(Meter, MaxMeter);
            gaugeMeter.gameObject.SetActive(true);
            inBattle = true;
        }

        // Fin du combat
        void ResetInstall()
        {
            inInstall = false;
            inUninstallReady = false;
            inInstallReady = false;
            firstReload = false;
            gaugeMeter.gameObject.SetActive(false);
            inBattle = false;
        }







        public void HitInstall(DamageMessage damageMsg)
        {
            if(inInstall)
                AddEtherGauge(-3);
            else 
                AddEtherGauge(1);
        }

        public void AttackInstall(AttackManager attack, Card cardAction)
        {
            if (cardAction!= null && !inInstall)
            {
                if(cardAction.GetCardType() == 1) // C'est magie
                    AddEtherGauge(1);
            }
        }

        public void AttackHitInstall(AttackController attack, CharacterBase target)
        {
            if (!inInstall)
                AddEtherGauge(1);
            else
                AddEtherGauge(-1);
        }

        public void CardBreakInstall(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker)
        {
            if (cardBreaker == null)
                return;
            if (characterBreaker == owner && !inInstall || characterBreaked == owner && inInstall) // si le perso qui break est le notre
            {
                int sumActive = 0;
                int sumNewCards = 0;

                for (int i = 0; i < cardBreaked.Count; i++)
                    sumActive += cardBreaked[i].GetCardValue();
                for (int i = 0; i < cardBreaker.Count; i++)
                    sumNewCards += cardBreaker[i].GetCardValue();

                int difference = sumNewCards - sumActive;

                if (!inInstall)
                {
                    AddEtherGauge(Mathf.Abs(difference));

                    if (inInstallReady && cardBreaker.Count <= 1) // On entre en install
                    {
                        cardBreaker.Clear();
                        cardBreaker.Add(new Card(CardInstall, 0));
                        owner.CharacterAction.CancelSleight();
                        inInstall = true;
                        inInstallReady = false;
                    }
                }
                else
                {
                    difference = Mathf.Clamp(Mathf.Abs(difference), 0, 9);
                    AddEtherGauge(-difference);
                }
            }


        }


        public void ReloadInstall()
        {
            if(inInstall)
            {
                if(firstReload)
                {
                    AddEtherGauge(-(int)(MaxMeter * 0.33f));
                }
                firstReload = true;
            }
        }

        public override void UpdateEffect(CharacterBase character)
        {
            base.UpdateEffect(character);
            if (!inBattle)
                return;

            if (character.State.ID == CharacterStateID.Idle)
            {
                if (inInstallReady)  // On entre en install
                {
                    owner.CharacterAction.Action(new Card(CardInstall, 0));
                    owner.CharacterAction.CancelSleight();
                    inInstall = true;
                    inInstallReady = false;
                    inUninstallReady = false;
                    firstReload = false;
                }
                else if (inUninstallReady)  // On sort de l' install
                {
                    owner.CharacterAction.Action(AttackUninstall);
                    owner.CharacterAction.CancelSleight();
                    inInstall = false;
                    inInstallReady = false;
                    inUninstallReady = false; 
                    firstReload = false;
                }
            }
        }

        public void AddEtherGauge(int amount)
        {
            Meter += amount;
            Meter = Mathf.Clamp(Meter, 0, MaxMeter);

            // On dessine
            gaugeMeter.DrawGauge(Meter, MaxMeter);

            if (Meter >= MaxMeter && !inInstall)
            {
                inInstallReady = true;
            }
            if (Meter <= 0 && inInstall)
            {
                inUninstallReady = true;
            }
        }



        public override void RemoveEffect(CharacterBase character)
        {
            character.OnBattleStart -= InitializeInstall;
            character.OnBattleEnd -= ResetInstall;
            CardBreakController.OnCardBreak -= CardBreakInstall;


            character.DeckController.OnReload -= ReloadInstall;
            character.CharacterAction.OnAction -= AttackInstall;
            character.CharacterAction.OnAttackHit -= AttackHitInstall;
            character.CharacterKnockback.OnHit -= HitInstall;

            GameObject.Destroy(gaugeMeter.gameObject);

        }

        #endregion

    }

} // #PROJECTNAME# namespace