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
    public class CharacterAction: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        // Référence circulaire ?
        CharacterBase character;

        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected CharacterState stateActing;
        [SerializeField]
        protected CharacterState stateCardBreak;


        // Quand on effectue une action on intéragis énormément avec les composants de cartes donc pour pouvoir lancé une action en 1 fonction
        //  je fout les composants de cartes dans Character Action
        protected DeckController deckController = null;
        protected SleightController sleightController = null;
        [Title("Cards Components")]
        [SerializeField]
        protected CardBreakController cardBreakController = null; // à bouger ou mettre en static / singleton


        // Pour les cancels
        [SerializeField]
        CardType cardTypes;
        public CardType CardTypes
        {
            get { return cardTypes; }
        }


        [Title("Sleight Action parameter")]
        [SerializeField]
        float sleightActionCooldown = 0.2f;
        float sleightActionCooldownT = 0f;


        private List<AttackProcessor> additionalAttackProcessor = new List<AttackProcessor>();
        public List<AttackProcessor> AdditionalAttackProcessor
        {
            get { return additionalAttackProcessor; }
        }

        public List<CardBreakComponent> cardBreakComponents = new List<CardBreakComponent>();

        protected List<Card> cardsCombo = new List<Card>(); // Pour les sleights et les combo d'attaque
        protected Card currentAttackCard; public Card CurrentAttackCard
        {
            get { return currentAttackCard; }
        }

        protected Card cardLastPlayed;// Données utilisés pour les combos / cancel d'animation

        protected AttackManager currentAttackManager = null;
        protected AttackManager previousAttackManager = null; public AttackManager PreviousAttackManager
        {
            get { return previousAttackManager; }
        }





        protected bool autoCombo = false;
        protected bool canMoveCancel = false;
        private bool canMoveCancelItself; public bool CanMoveCancelItself
        {
            get { return canMoveCancelItself; }
            set { canMoveCancelItself = value; }
        }
        private bool canSpecialCancel; public bool CanSpecialCancel
        {
            get { return canSpecialCancel; }
            set { canSpecialCancel = value; }
        }
        private bool[] canMoveCancelType; public bool[] CanMoveCancelType
        {
            get { return canMoveCancelType; }
            set { canMoveCancelType = value; }
        }
        private int specialCancelCount = 0; public int SpecialCancelCount
        {
            get { return specialCancelCount; }
            set { specialCancelCount = value; }
        }

        // Flag pour activer ou désactiver le card break
        private bool canCardBreak = true; public bool CanCardBreak
        {
            get { return canCardBreak; }
            set { canCardBreak = value; }
        }

        public delegate void ActionCharacterAttack(AttackManager attack, Card card);
        public event ActionCharacterAttack OnAction;

        // Mouais
        public event ActionCharacterAttack OnSleightPlayed;

        public delegate void ActionCharacterAttackHit(AttackController attack, CharacterBase character);
        public event ActionCharacterAttackHit OnAttackHit;


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


        public void InitializeComponent(CharacterBase characterBase)
        {
            character = characterBase;
            deckController = characterBase.DeckController;
            sleightController = characterBase.SleightController;

            CanMoveCancelType = new bool[cardTypes.CardTypeName.Count];
            for (int i = 0; i < cardTypes.CardTypeName.Count; i++)
            {
                CanMoveCancelType[i] = false;
            }
        }




        // Return true si la carte a été joué
        public bool PlayCard()
        {
            Card card = deckController.GetCurrentCard();
            if (CanAct(card) == true)
            {
                // Joue la carte
                List<Card> cards = new List<Card>(0);
                cards.Add(deckController.SelectCard());
                // Check le card break si une carte est déjà en jeu
                if (cardBreakController.PlayCard(character, cards) == true)
                {
                    Action(cards);
                }
                return true;
            }
            return false;
        }

        // Joue une carte sans faire de check ou quoi
        public void ForcePlayCard(Card c)
        {
            List<Card> cards = new List<Card>(0);
            cards.Add(c);
            if (cardBreakController.PlayCard(character, cards) == true)
            {
                Action(cards);
            }
        }





        /// <summary>
        /// Stock une carte pour une Sleight, si on est à 3 carte stocké, on joue la sleight
        /// </summary>
        /// <param name="playSleight"> Détermine si on j</param>
        /// <returns></returns>
        public bool PlaySleight(bool playSleight = true)
        {
            // Cooldown QoL pour les spammers fou
            if (sleightActionCooldownT > 0)
            {
                character.Inputs.InputY.ResetBuffer();
                return false;
            }

            if (sleightController.CanPlaySleight() == false) // Si on est pas à 3 cartes pour la sleight
            {
                if(deckController.GetCurrentIndex() != 0) // On ajoute la carte au sleight
                {
                    deckController.BanishCard(deckController.GetCurrentCard()); // On la banni "temporairement" pour ne plus pouvoir la reload
                    sleightController.AddSleightCard(deckController.SelectCard());
                }
                return true;
            }

            if(playSleight)
                return ForcePlaySleight();
            return false;
        }

        // Joue une sleight "complete"
        public bool ForcePlaySleight()
        {
            // Check d'abord si sleight controller est ok et si character action est ok
            if (sleightController.CanPlaySleight())
            {
                // On transforme les cartes en une éventuelle sleight
                bool isSleight = false;
                List<Card> cards = new List<Card>(sleightController.GetSleightCards());
                List<Card> sleight = sleightController.GetSleightAction(cards, out isSleight);

                // On check si la sleight est jouable au niveau des cancels
                if (!CanPlaySleight(sleight[0]))
                    return false;

                // bannir la première carte si elle n'est pas premium, unban les autres
                for (int i = 0; i < cards.Count; i++)
                {
                    if (i == 0 && !cards[i].CardPremium)
                        continue;
                    deckController.UnbanishCard(cards[i]);
                }

                if (cardBreakController.PlayCard(character, cards, sleight[0].CardData.CardBreakComponent) == true)
                {
                    OnSleightPlayed?.Invoke(sleight[0].CardData.AttackManager, sleight[0]);
                    Action(sleight);
                }

                sleightController.ResetSleightCard();
                StartCoroutine(SleightCooldown());
                return true;
            }
            return false;
        }

        private IEnumerator SleightCooldown()
        {
            while (sleightActionCooldownT < sleightActionCooldown)
            {
                sleightActionCooldownT += Time.deltaTime;
                yield return null;
            }
            sleightActionCooldownT = 0f;
        }


        // Replace les cartes en préparation d'un sleight dans le deck
        public void CancelSleight()
        {
            if (sleightController.GetIndexSleightCard() == 0)
                return;
            List<Card> cards = new List<Card>(sleightController.GetSleightCards());
            deckController.ReplaceCard(cards);
            sleightController.ResetSleightCard();

            // On débanni les cartes puisqu'on les banni temporairement quand on prépare une sleight
            for (int i = 0; i < cards.Count; i++)
            {
                deckController.UnbanishCard(cards[i]);
            }
        }




        public void RemoveCards()
        {
            // On peut remove les cartes en jeu uniquement si une action est déjà en cours et qu'elle provient de nous
            if(currentAttackCard != null && cardBreakController.GetActiveCharacter() == character)
                cardBreakController.RemoveCurrentCards();
        }




        public bool CanPlaySleight(Card action = null)
        {
            if (currentAttackManager != null && !canMoveCancel)
                return false;

            // Check au niveau des flags de cancel si on peut jouer la sleight
            if(action != null && currentAttackManager != null && canMoveCancel)
            {
                if (CanMoveCancelItself == false && currentAttackCard.CardData.Equals(action.CardData))
                {
                    return false;
                }
            }

            return true;
        }



        private bool CanAct(Card action = null)
        {
            // Check au niveau du Deck
            if (action != null)
            {
                if (deckController.GetCanPlayCard() == false)
                    return false;
            }

            // Check au niveau des cancels
            if (currentAttackManager != null && canMoveCancel == false) 
            {
                return false;
            }
            else if (currentAttackManager != null && canMoveCancel == true)
            {

                if (currentAttackCard != null) 
                {
                    if (CanMoveCancelItself == false && currentAttackCard.CardData.Equals(action.CardData))
                    {
                        return false;
                    }
                }

                if (action.CardData != null)
                {
                    // Si le flag du type de l'attaque est faux, on ne peut pas cancel
                    if (CanMoveCancelType[action.CardData.CardType] == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }








        // =============================== //
        //          A C T I O N
        // =============================== //
        // Rework le truc un jour pour pooler les prefabs d'attaques des cartes

        public void Action(List<Card> cards)
        {
            cardsCombo = cards;
            Action(cardsCombo[0]);
            cardsCombo.RemoveAt(0);

            autoCombo = false;
            if (cardsCombo.Count != 0)
                autoCombo = true;
        }

        public void Action(Card cardAction)
        {
            currentAttackCard = cardAction;
            Action(cardAction.CardData.AttackManager, cardAction);

        }

        public void Action(AttackManager attack, Card cardAction = null, bool setState = true)
        {
            canMoveCancel = false;

            // Animation de l'attaque
            // (Si un jour je découple, faire un système de frame dans l'attackController)
            animator.ResetTrigger("Idle");
            animator.Play(attack.AttackAnimation.name, 0, 0f);

            // On créer l'attaque
            previousAttackManager = currentAttackManager;

            if (currentAttackManager != null)
                currentAttackManager.CancelAction();
            currentAttackManager = Instantiate(attack, this.transform.position, Quaternion.identity);
            currentAttackManager.CreateAttack(cardAction, character);

            if (setState)
                character.SetState(stateActing);

            OnAction?.Invoke(attack, cardAction);
        }





        public void MoveCancelable()
        {
            canMoveCancel = true;
            if (autoCombo == true)
                Action(cardsCombo);
        }



        // Appelé par les anims, active le bool pour Cancel l'action à la frame suivante
        public void EndAction()
        {
            if (autoCombo == true)
            {
                Action(cardsCombo);
                return;
            }
            RemoveCards();
            CancelAction();
            character.ResetToIdle();
            //OnActionEnd.Invoke();
        }



        public void CancelAction()
        {
            if (currentAttackManager != null)
                currentAttackManager.CancelAction();
            currentAttackManager = null;
            previousAttackManager = null;
            currentAttackCard = null;
            //cardLastPlayed = null;
            cardsCombo.Clear();

            canMoveCancel = false;
            autoCombo = false;
        }

        public void SetAttackMotionSpeed(float newValue)
        {
            if (currentAttackManager != null)
                currentAttackManager.AttackMotionSpeed(newValue);
        }




        // Les sub action sont souvent des projectiles
        // De ce fait ils ont pour propriété d'exister indépendamment du character ce qui fait que si on 
        // card break le personnage, le sub action (souvent un projectile) continuera d'exister.

        // Si on ne veut PAS que ça arrive, alors il faut ajouter l'attaque dans la liste subActionBreakable
        // Ex : Le sort Glacier doit être card Breakable donc on l'ajoute a la liste
        // Ex : Une AoE au sol ne doit pas être card Breakable donc on peut se passer d'appeler cette fonction
        List<AttackManager> subActionBreakable = new List<AttackManager>();
        public void AddSubActionBreakable(AttackManager attack)
        {
            subActionBreakable.Add(attack);
        }
        public void RemoveSubActionBreakable(AttackManager attack)
        {
            subActionBreakable.Remove(attack);
        }

        // Créer une sub action
        /*public void SubAction(Card card)
        {
            AttackManager subAction = Instantiate(card.CardData.AttackManager, this.transform.position, Quaternion.identity);
            subAction.CreateAttack(card, character);
        }*/

        // Fonction appelant un event pour réaliser une action quand une de nos attaque a touché
        public void AttackHit(AttackController attack, CharacterBase target)
        {
            OnAttackHit?.Invoke(attack, target);
        }

        public void CardBreak(CharacterBase breaker)
        {
            if (!CanCardBreak)
                return;

            CancelAction();

            for (int i = subActionBreakable.Count-1; i >= 0; i--)
            {
                subActionBreakable[i].CancelAction();
            }

            if (breaker.transform.position.x < character.transform.position.x)
                character.CharacterMovement.SetDirection(-1);
            else
                character.CharacterMovement.SetDirection(1);
            character.SetState(stateCardBreak);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace