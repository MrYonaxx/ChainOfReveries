/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class DeckBattleDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Parameter")]
        [SerializeField]
        bool handLimited = true;

        [Title("Card Data")]
        [SerializeField]
        CardType cardTypeDictionary = null;

        [Title("Card Reload")]
        [SerializeField]
        CardController cardPrefab = null;
        [SerializeField]
        CardReloadController cardReload = null;
        [SerializeField]
        CardData cardReloadData = null;

        [Title("Card Placement")]
        [SerializeField]
        RectTransform[] transformsHand = null;
        [SerializeField]
        Transform deckParent = null;

        [SerializeField]
        [SuffixLabel("/60 pour le tps en s")]
        int cardSpeed = 10;
        [SerializeField]
        int transformCenterIndex = 2;


        [Title("Card Count")]
        [SerializeField]
        RectTransform cardCountScale = null;
        [SerializeField]
        TextMeshProUGUI textCardNumber = null;

        [Space]
        public int handLimitMin = 0;
        public int handLimitMax = 7;

        int maxCard = -1;
        int currentIndex = 1;
        List<CardController> cardControllers = new List<CardController>();

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */
        public RectTransform GetCenterHandTransform()
        {
            return transformsHand[transformCenterIndex];
        }
        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void Start()
        {
            if (cardPrefab == null)
                return;

            for (int i = 0; i < transformsHand.Length; i++)
            {
                if (i >= cardControllers.Count)
                    cardControllers.Add(Instantiate(cardPrefab, transformsHand[i].transform));
            }
        }

        public void SetCardControllers(List<CardController> controllers)
        {
            cardControllers = controllers;
        }

        public void ShowDeck(bool b)
        {
            deckParent.gameObject.SetActive(b);
        }

        public void DrawHand(int index, List<Card> deck)
        {
            currentIndex = index;
            handLimitMin = 0;
            handLimitMax = transformsHand.Length;

            if (handLimited == true && deck.Count < transformsHand.Length) // Si la main est limité on ressers la taille de la main de jeu
            {
                int sizeLeft = 0;
                int sizeRight = 0;
                while (handLimitMax - handLimitMin != deck.Count)
                {
                    sizeLeft = transformCenterIndex - handLimitMin;
                    sizeRight = handLimitMax - transformCenterIndex;
                    if (sizeRight > sizeLeft)
                    {
                        cardControllers[handLimitMax - 1].HideCard();
                        handLimitMax -= 1;
                    }
                    else
                    {
                        cardControllers[handLimitMin].HideCard();
                        handLimitMin += 1;
                    }
                }
            }

            /*if (cardReload != null)
                cardReload.HideReload();*/
            for (int i = handLimitMin; i < handLimitMax; i++)
            {
                DrawCard(i, deck);
            }

            if (maxCard == -1)
                maxCard = deck.Count;
        }


        public void DrawCard(int index, List<Card> deck)
        {
            // index est l'index par rapport à la main (0 = la carte la plus a droite de la main)
            // cardID = id réel de la card
            int cardID = currentIndex - transformCenterIndex + index;
            while (cardID >= deck.Count)
                cardID -= deck.Count;
            while (cardID <= -1)
                cardID += deck.Count;

            // Si la carte 0 est la carte reload on la dessine
            if (cardID == 0 && cardReload != null && deck[cardID].CardData == cardReloadData)
            {
                cardReload.ShowReload();
                cardReload.transform.SetParent(cardControllers[index].transform);
                cardReload.transform.localPosition = Vector3.zero;
                cardReload.transform.localScale = Vector3.one;
                cardReload.transform.localRotation = Quaternion.identity;
            }
            else if (cardID == 0 && cardReload != null && deck[cardID].CardData != cardReloadData) 
            {
                cardReload.HideReload();
            }

            // Draw the card
            cardControllers[index].DrawCard(deck[cardID].GetCardIcon(), 
                                            cardTypeDictionary.GetColorType(deck[cardID].GetCardType()),
                                            deck[cardID].GetCardValue(), deck[cardID].CardPremium);

            if (cardReload != null)
            {
                if (currentIndex > transformsHand.Length / 2 && currentIndex < deck.Count - transformsHand.Length / 2)
                    cardReload.HideReload();
            }

        }




        public void MoveHand(int index, List<Card> deck)
        {
            if (deck.Count == 0)
                return;

            if(index == ((currentIndex + 1) + deck.Count) % deck.Count)
            {
                MoveHandRight(index, deck);
            }
            else if (index == ((currentIndex - 1) + deck.Count) % deck.Count)
            {
                MoveHandLeft(index, deck);
            }
        }

        public void MoveHandLeft(int index, List<Card> deck)
        {
            currentIndex = index;
            // On décale chaque carte vers la droite
            CardController cardTemp = cardControllers[handLimitMax - 1];
            for (int i = handLimitMax - 1; i > handLimitMin; i--)
            {
                cardControllers[i] = cardControllers[i - 1];
                cardControllers[i - 1].MoveCard(transformsHand[i], cardSpeed);
            }
            // La carte la plus à gauche est remplacé par la carte la plus à droite et on redessine
            cardControllers[handLimitMin] = cardTemp;
            if (deck.Count < transformsHand.Length && handLimited == true)
                cardControllers[handLimitMin].MoveCard(transformsHand[handLimitMin], cardSpeed); // Pas Instant pour bien dire qu'il ne reste plus beaucoup de carte
            else
                cardControllers[handLimitMin].MoveCard(transformsHand[handLimitMin], 0); // Instant pour donner l'illusion que le deck est immense
            
            // On dessine la carte tout à gauche
            DrawCard(handLimitMin, deck);
        }

        public void MoveHandRight(int index, List<Card> deck)
        {
            currentIndex = index;
            // On décale chaque carte vers la gauche
            CardController cardTemp = cardControllers[handLimitMin];
            for (int i = handLimitMin; i < handLimitMax - 1; i++)
            {
                cardControllers[i] = cardControllers[i + 1];
                cardControllers[i + 1].MoveCard(transformsHand[i], cardSpeed);
            }

            // La carte la plus à droite est remplacé par la carte la plus à gauche et on redessine
            cardControllers[handLimitMax - 1] = cardTemp;
            if (deck.Count < transformsHand.Length && handLimited == true)
                cardControllers[handLimitMax - 1].MoveCard(transformsHand[handLimitMax - 1], cardSpeed); // Pas Instant pour bien dire qu'il ne reste plus beaucoup de carte
            else
                cardControllers[handLimitMax - 1].MoveCard(transformsHand[handLimitMax - 1], 0); // Instant pour donner l'illusion que le deck est immense
           
            // On dessine la carte tout à droite
            DrawCard(handLimitMax - 1, deck);
        }



        public void PlayCard(int index, List<Card> deck)
        {
            currentIndex = index;
            if (deck.Count < transformsHand.Length) // Si la main est plus grande que le deck on réduit les limites
            {
                cardControllers[transformCenterIndex].HideCard();
                CardController cardTemp = cardControllers[transformCenterIndex];
                for (int i = transformCenterIndex; i < handLimitMax - 1; i++)
                {
                    cardControllers[i] = cardControllers[i + 1];
                    cardControllers[i + 1].MoveCard(transformsHand[i], cardSpeed);
                }


                int sizeLeft = transformCenterIndex - handLimitMin;
                int sizeRight = handLimitMax - transformCenterIndex;
                if (sizeRight > sizeLeft)
                {
                    cardControllers[handLimitMax - 1] = cardTemp;
                    cardControllers[handLimitMax - 1].HideCard();
                    handLimitMax -= 1;
                }
                else
                {
                    cardControllers[handLimitMax - 1] = cardControllers[handLimitMin];
                    cardControllers[handLimitMax - 1].MoveCard(transformsHand[handLimitMax - 1], cardSpeed);
                    cardControllers[handLimitMin] = cardTemp;
                    cardControllers[handLimitMin].HideCard();
                    handLimitMin += 1;
                }
            }
            else
            {
                CardController cardTemp = cardControllers[transformCenterIndex];
                for (int i = transformCenterIndex; i < handLimitMax - 1; i++)
                {
                    cardControllers[i] = cardControllers[i + 1];
                    cardControllers[i + 1].MoveCard(transformsHand[i], cardSpeed);
                }
                cardControllers[handLimitMax - 1] = cardTemp;
                cardControllers[handLimitMax - 1].MoveCard(transformsHand[handLimitMax - 1], 0);
                DrawCard(handLimitMax - 1, deck);
            }

            if (cardCountScale)
            {
                cardCountScale.localScale = new Vector3(deck.Count / (float)maxCard, deck.Count / (float)maxCard, 1);
                textCardNumber.text = deck.Count.ToString();
            }
        }

        public void HideCards()
        {
            for (int i = 0; i < cardControllers.Count; i++)
            {
                cardControllers[i].MoveCard(transformsHand[i], 0);
                cardControllers[i].HideCard();
            }
        }


        public void DrawReload(int reloadCurrentLevel, float reloadCurrentAmount, int reloadMaxAmount)
        {
            cardReload.DrawReload(reloadCurrentLevel, reloadCurrentAmount, reloadMaxAmount);
        }





        #endregion

    } 

} // #PROJECTNAME# namespace