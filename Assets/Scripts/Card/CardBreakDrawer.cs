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

    // Ce truc est un échec tant pis
    public class CardBreakDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        CardBreakController cardBreakManager = null;

        [Title("Parameter")]
        [SerializeField]
        CardType cardType = null;

        [Title("Center")]
        [SerializeField]
        RectTransform cardCenter = null;
        [SerializeField]
        RectTransform cardPosPlayer = null;
        [SerializeField]
        RectTransform cardPosEnemy = null;

        [Title("Sleight")]
        [SerializeField]
        TMPro.TextMeshProUGUI textSleightName = null;
        [SerializeField]
        TMPro.TextMeshProUGUI textSleightChain = null;
        [SerializeField]
        Animator animatorSleightName = null;

        [Space]
        // Utilisé pour retrouver les positions des cartes sleight
        [SerializeField]
        SleightDrawer sleightPlayer = null;
        [SerializeField]
        SleightDrawer sleightEnemy = null;

        [Title("Player")]
        [SerializeField]
        Vector2 cardsOffset = Vector2.one;
        [SerializeField]
        int maxNumberOfCardController = 10;
        [SerializeField]
        CardController cardControllerPrefab = null;

        int showSleight = 0;
        int indexCardControllers = 0;
        public Queue<CardController> cardControllers = new Queue<CardController>();
        public List<CardController> cardControllersActive = new List<CardController>();

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

        private void Start()
        {
            for (int i = 0; i < maxNumberOfCardController; i++)
            {
                CardController card = Instantiate(cardControllerPrefab, cardCenter);
                cardControllers.Enqueue(card);
                card.HideCard();
            }
            cardBreakManager.OnCardPlayed += CallbackPlayCard;
            cardBreakManager.OnCardBreak += CallbackCardBreak;
            cardBreakManager.OnCardBreakDraw += CallbackCardBreakDraw;
            cardBreakManager.OnCardIneffective += CallbackCardIneffective;
            cardBreakManager.OnCardRemove += CallbackHideCard;

            showSleight = PlayerPrefs.GetInt("SleightName");
        }

        private void OnDestroy()
        {
            cardBreakManager.OnCardPlayed -= CallbackPlayCard;
            cardBreakManager.OnCardBreak -= CallbackCardBreak;
            cardBreakManager.OnCardBreakDraw -= CallbackCardBreakDraw;
            cardBreakManager.OnCardIneffective -= CallbackCardIneffective;
            cardBreakManager.OnCardRemove -= CallbackHideCard;
        }




        public void DisableCards()
        {
            HideCard();
            // Le truc le plus compliqué de l'histoire juste pour fix un bug visuel durant le Megido de shimérie
            for (int i = 0; i < cardControllers.Count; i++)
            {
                CardController c = cardControllers.Dequeue();
                c.gameObject.SetActive(false);
                cardControllers.Enqueue(c);
            }
            HideSleight();
        }



        public void CallbackPlayCard(CharacterBase user, List<Card> cards)
        {
            DrawCardsPlayed(user.tag, cards);

            if (cards.Count > 1)
                DrawSleight(user);
            else
                HideSleight();
        }

        public void CallbackCardIneffective(CharacterBase currentCharacter, List<Card> currentCards, CharacterBase user, List<Card> cards)
        {
            DrawCardsIneffective(cards, user.tag);

            // On check l'intégrité des cartes actuelles
            int count = cardControllersActive.Count - currentCards.Count;
            for (int i = 0; i < count; i++)
            {
                StartCoroutine(DrawCardIneffectiveCoroutine(cardControllersActive[cardControllersActive.Count-i-1], currentCharacter.tag));
            }
        }

        public void CallbackCardBreakDraw(CharacterBase characterBreaked, List<Card> cardsBreaked, CharacterBase user, List<Card> cards)
        {
            DrawCardsIneffective(cards, user.tag);
            for (int i = 0; i < cardControllersActive.Count; i++)
            {
                StartCoroutine(DrawCardIneffectiveCoroutine(cardControllersActive[i], characterBreaked.tag));
            }
            HideSleight();
        }

        public void CallbackCardBreak(CharacterBase characterBreaked, List<Card> cardsBreaked, CharacterBase user, List<Card> cards)
        {
            DrawCardBreaked(user.tag);
            AnimationCardBreak(characterBreaked);

            if (cards != null)
            {
                if (cards.Count > 1)
                    DrawSleight(user);
                else
                    HideSleight();
            }
        }

        public void CallbackHideCard()
        {
            HideCard();
            HideSleight();
        }







        public void HideCard()
        {
            for (int i = 0; i < cardControllersActive.Count; i++)
            {
                cardControllersActive[i].Disappear();
                cardControllers.Enqueue(cardControllersActive[i]);
            }
            cardControllersActive.Clear();
        }



        // ===========================================
        //   C A R D   P L A Y E D
        // ===========================================

        public void DrawCardsPlayed(string tag, List<Card> cards)
        {
            Vector2 offset = Vector2.zero;
            RectTransform origin = null;

            // On calcule l'origin de la première carte
            if(cards.Count == 1)
            {
                if (tag == "Player")
                    origin = cardPosPlayer;
                else if (tag == "Enemy")
                    origin = cardPosEnemy;
            }
            else // Si on a une liste des cartes ça provient d'une sleight
            {
                if (tag == "Player")
                    origin = sleightPlayer.SleightTransform[0];
                else if (tag == "Enemy")
                    origin = sleightEnemy.SleightTransform[0];
            }

            // On joue les cartes

            if(cards.Count == 3)
                offset -= cardsOffset; // manip pour centrer sur l'écran

            for (int i = cards.Count-1; i >= 0; i--)
            {
                DrawCardPlayed(tag, cards[i], offset, origin);
                //cardControllersActive.Add(cardControllers.Dequeue());

                offset += cardsOffset;

                if (tag == "Player")
                    origin = sleightPlayer.SleightTransform[(cards.Count - i)-1];
                else if (tag == "Enemy")
                    origin = sleightEnemy.SleightTransform[(cards.Count - i) - 1];
            }
        }

        public void DrawCardPlayed(string tag, Card card, Vector2 offset, RectTransform origin)
        {
            // On prend une carte
            cardControllersActive.Add(cardControllers.Dequeue());
            int index = cardControllersActive.Count - 1;

            cardControllersActive[index].gameObject.name = card.CardData.name;
            cardControllersActive[index].transform.position = origin.position;

            cardControllersActive[index].DrawCard(card, cardType);
            cardControllersActive[index].MoveCard(cardCenter, 8, offset);
            cardControllersActive[index].transform.SetSiblingIndex(maxNumberOfCardController);

        }




        // ===========================================
        //   C A R D   B R E A K 
        // ===========================================

        public void DrawCardBreaked(string tag)
        {
            for (int i = 0; i < cardControllersActive.Count; i++)
            {
                StartCoroutine(CardBreakCoroutine(cardControllersActive[i], tag));       
                cardControllers.Enqueue(cardControllersActive[i]);
            }
            cardControllersActive.Clear();
        }

        private IEnumerator CardBreakCoroutine(CardController card, string tag)
        {
            yield return new WaitForSeconds(0.05f);
            if (tag == "Player")
                card.CardBreakAnimation(1500, 5000, 1000, 30);
            else if (tag == "Enemy")
                card.CardBreakAnimation(-1500, 5000, 1000, 30);
        }

        public void AnimationCardBreak(CharacterBase characterBreaked)
        {
            if(characterBreaked == null)
            {
                Debug.Log("Allo?");
                return;
            }
            BattleFeedbackManager.Instance.RippleScreen(characterBreaked.transform.position.x, characterBreaked.transform.position.y);

            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0, 0.22f);
            BattleFeedbackManager.Instance.CardBreakAnimation(characterBreaked);
        }

        /*public void AnimationCardBreakLight(CharacterBase characterBreaked)
        {
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0, 0.2f);

            animatorTextCardBreak.transform.position = characterBreaked.transform.position;
            animatorTextCardBreak.SetTrigger("Feedback");

            Instantiate(cardBreakAnimation, characterBreaked.transform.position, Quaternion.identity);

            animatorPixelize.gameObject.SetActive(true);
            animatorPixelize.SetTrigger("CardBreak");
        }*/




        // ===========================================
        //   C A R D   I N E F F E C T I V E
        // ===========================================

        public void DrawCardsIneffective(List<Card> cards, string tag)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                DrawCardsIneffective(cards[i], tag);
            }
        }

        public void DrawCardsIneffective(Card card, string tag)
        {
            // On prend une carte
            CardController cardController = cardControllers.Dequeue();

            if (tag == "Player")
                cardController.transform.position = cardPosPlayer.position;
            else if (tag == "Enemy")
                cardController.transform.position = cardPosEnemy.position;

            cardController.DrawCard(card, cardType);
            cardController.MoveCard(cardCenter, 8);
            StartCoroutine(DrawCardIneffectiveCoroutine(cardController, tag));

            cardControllers.Enqueue(cardController);
        }

        private IEnumerator DrawCardIneffectiveCoroutine(CardController card, string tag)
        {
            yield return new WaitForSeconds(0.1f);
            if (tag == "Player")
                card.CardBreakAnimation(-1000, 0, 200, 30);
            else if (tag == "Enemy")
                card.CardBreakAnimation(1000, 0, 200, 30);
        }




        private void DrawSleight(CharacterBase character)
        {
            if (showSleight == 0)
                return;
            animatorSleightName.gameObject.SetActive(true);
            textSleightName.text = character.CharacterAction.CurrentAttackCard.GetCardName();

            if (character.CharacterAction.CanSpecialCancel || character.CharacterAction.SpecialCancelCount > 0)
            {
                textSleightChain.text = (character.CharacterAction.SpecialCancelCount+1).ToString();
                textSleightChain.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                textSleightChain.transform.parent.gameObject.SetActive(false);
            }
            animatorSleightName.SetBool("Appear", true);
        }

        private void HideSleight()
        {
            if(animatorSleightName.isActiveAndEnabled)
                animatorSleightName.SetBool("Appear", false);
        }


        #endregion

    } 

} // #PROJECTNAME# namespace