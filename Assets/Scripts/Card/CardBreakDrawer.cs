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

        int indexCardControllers = 0;
        List<CardController> cardControllers = new List<CardController>();
        List<CardController> cardControllersActive = new List<CardController>();

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
                cardControllers.Add(Instantiate(cardControllerPrefab, cardCenter));
                cardControllers[i].HideCard();
            }
            cardBreakManager.OnCardPlayed += CallbackPlayCard;
            cardBreakManager.OnCardBreak += CallbackCardBreak;
            cardBreakManager.OnCardBreakDraw += CallbackCardBreakDraw;
            cardBreakManager.OnCardIneffective += CallbackCardIneffective;
            cardBreakManager.OnCardRemove += CallbackHideCard;
        }

        private void OnDestroy()
        {
            cardBreakManager.OnCardPlayed -= CallbackPlayCard;
            cardBreakManager.OnCardBreak -= CallbackCardBreak;
            cardBreakManager.OnCardBreakDraw -= CallbackCardBreakDraw;
            cardBreakManager.OnCardIneffective -= CallbackCardIneffective;
            cardBreakManager.OnCardRemove -= CallbackHideCard;
        }


        public void CallbackPlayCard(CharacterBase user, List<Card> cards)
        {
            DrawCardsPlayed(user.tag, cards);
        }

        public void CallbackCardIneffective(CharacterBase user, List<Card> cards)
        {
            DrawCardsIneffective(cards, user.tag);
        }

        public void CallbackCardBreakDraw(CharacterBase characterBreaked, List<Card> cardsBreaked, CharacterBase user, List<Card> cards)
        {
            DrawCardsIneffective(cards, user.tag);
            for (int i = 0; i < cardControllersActive.Count; i++)
            {
                StartCoroutine(DrawCardIneffectiveCoroutine(cardControllersActive[i], characterBreaked.tag));
            }
        }

        public void CallbackCardBreak(CharacterBase characterBreaked, List<Card> cardsBreaked, CharacterBase user, List<Card> cards)
        {
            DrawCardBreaked(user.tag);
            AnimationCardBreak(characterBreaked);
        }

        public void CallbackHideCard()
        {
            HideCard();
        }






        public void HideCard()
        {
            for (int i = 0; i < cardControllersActive.Count; i++)
            {
                cardControllersActive[i].Disappear();
            }
            cardControllersActive.Clear();
        }





        public void DrawCardsPlayed(string tag, List<Card> cards)
        {
            Vector2 offset = Vector2.zero;
            RectTransform origin = null;

            if(cards.Count == 1)
            {
                if (tag == "Player")
                    origin = cardPosPlayer;
                else if (tag == "Enemy")
                    origin = cardPosEnemy;
            }
            else
            {
                if (tag == "Player")
                    origin = sleightPlayer.SleightTransform[0];
                else if (tag == "Enemy")
                    origin = sleightEnemy.SleightTransform[0];
            }

            for(int i = 0; i < cards.Count; i++)
            {
                DrawCardPlayed(tag, cards[i], offset, origin);
                cardControllersActive.Add(cardControllers[indexCardControllers]);
                offset += cardsOffset;

                if(i<cards.Count-1)
                {
                    if (tag == "Player")
                        origin = sleightPlayer.SleightTransform[i + 1];
                    else if (tag == "Enemy")
                        origin = sleightEnemy.SleightTransform[i + 1];
                }
            }
        }

        public void DrawCardPlayed(string tag, Card card, Vector2 offset, RectTransform origin)
        {
            indexCardControllers += 1;
            if (indexCardControllers >= cardControllers.Count)
                indexCardControllers = 0;

            cardControllers[indexCardControllers].gameObject.name = card.CardData.name;
            cardControllers[indexCardControllers].transform.position = origin.position;

            cardControllers[indexCardControllers].DrawCard(card, cardType);
            cardControllers[indexCardControllers].MoveCard(cardCenter, 8, offset);
            cardControllers[indexCardControllers].transform.SetSiblingIndex(maxNumberOfCardController);
        }




        public void DrawCardBreaked(string tag)
        {
            for (int i = 0; i < cardControllersActive.Count; i++)
            {
                StartCoroutine(CardBreakCoroutine(cardControllersActive[i], tag));
            }
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







        public void DrawCardsIneffective(List<Card> cards, string tag)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                DrawCardsIneffective(cards[i], tag);
            }
        }

        public void DrawCardsIneffective(Card card, string tag)
        {
            indexCardControllers += 1;
            if (indexCardControllers >= cardControllers.Count)
                indexCardControllers = 0;


            if (tag == "Player")
                cardControllers[indexCardControllers].transform.position = cardPosPlayer.position;
            else if (tag == "Enemy")
                cardControllers[indexCardControllers].transform.position = cardPosEnemy.position;

            cardControllers[indexCardControllers].DrawCard(card, cardType);
            cardControllers[indexCardControllers].MoveCard(cardCenter, 8);
            StartCoroutine(DrawCardIneffectiveCoroutine(cardControllers[indexCardControllers], tag));
        }

        private IEnumerator DrawCardIneffectiveCoroutine(CardController card, string tag)
        {
            yield return new WaitForSeconds(0.1f);
            if (tag == "Player")
                card.CardBreakAnimation(-1000, 0, 200, 30);
            else if (tag == "Enemy")
                card.CardBreakAnimation(1000, 0, 200, 30);
        }




        #endregion

    } 

} // #PROJECTNAME# namespace