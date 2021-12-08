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

        public delegate void ActionCharacterAttack(AttackManager attack, Card card);
        public event ActionCharacterAttack OnAction;

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
                List<Card> cards = new List<Card>(0);
                cards.Add(deckController.SelectCard());
                // Joue la card mais check le card break si une carte est déjà en jeu
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




        // Joue la sleight sauf si on est a 2 ou moins de carte stocké
        public bool PlaySleight()
        {
            // Cooldown QoL pour les spammers fou
            if (sleightActionCooldownT > 0)
            {
                character.Inputs.InputY.ResetBuffer();
                return false;
            }

            if (sleightController.CanPlaySleight() == false)
            {
                if(deckController.GetCurrentIndex() != 0) // On ajoute la carte au sleight
                {
                    deckController.BanishCard(deckController.GetCurrentCard()); // On la banni "temporairement" pour ne plus pouvoir la reload
                    sleightController.AddSleightCard(deckController.SelectCard());
                }
                return true;
            }

            return ForcePlaySleight();
        }

        // Joue la sleight sauf si on est a 2 ou moins de carte stocké
        public bool ForcePlaySleight()
        {
            if (sleightController.CanPlaySleight() == true && CanPlaySleight() == true)
            {
                List<Card> cards = new List<Card>(sleightController.GetSleightCards());

                // bannir la première carte si elle n'est pas premium
                //if(!cards[0].CardPremium)
                //    deckController.BanishCard(cards[0]);

                // bannir la première carte si elle n'est pas premium, unban les autres
                for (int i = 0; i < cards.Count; i++)
                {
                    if (i == 0 && !cards[i].CardPremium)
                        continue;
                    deckController.UnbanishCard(cards[i]);
                }

                if (cardBreakController.PlayCard(character, cards) == true)
                    Action(cards);
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
            if(currentAttackCard != null)
                cardBreakController.RemoveCurrentCards();
        }




        public bool CanPlaySleight()
        {
            if (currentAttackManager != null && canMoveCancel == false)
                return false;
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
                if (CanMoveCancelItself == false && currentAttackCard.CardData.Equals(action.CardData))
                {
                    return false;
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
            if (sleightController != null)
                cards = sleightController.GetSleightAction(cards); // Remplace la liste de carte par une carte sleight si possible

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


        // Fonction appelant un event pour réaliser une action quand une de nos attaque a touché
        public void AttackHit(AttackController attack, CharacterBase target)
        {
            OnAttackHit?.Invoke(attack, target);
        }

        public void CardBreak(CharacterBase breaker)
        {
            for (int i = subActionBreakable.Count-1; i >= 0; i--)
            {
                subActionBreakable[i].CancelAction();
                //subActionBreakable.RemoveAt(i);
                //Debug.Log("Sub action card breaked");
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