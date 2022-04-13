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
using UnityEngine.UI;

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
        int[] cardSpriteOffset = null; // offset de l'illustration de la carte en fonction de sa position dans la main
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

        [Title("Deck Thumbnail")]
        [SerializeField]
        RectTransform transformDeckThumbnail = null;
        [SerializeField]
        RectTransform cursorDeckThumbnail = null;
        [SerializeField]
        Image elementDeckThumbnailPrefab = null;

        List<Image> elementsDeckThumbnail = new List<Image>();


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
        private void Awake()
        {
            if (cardPrefab == null)
                return;

            for (int i = 0; i < transformsHand.Length; i++)
            {
                if (i >= cardControllers.Count)
                    cardControllers.Add(Instantiate(cardPrefab, transformsHand[i].transform));
            }

            // Deck thumbnail
            if (transformDeckThumbnail != null)
            {
                int showThumbnail = PlayerPrefs.GetInt("DeckThumbnail", 0);
                if (showThumbnail == 0)
                {
                    transformDeckThumbnail.gameObject.SetActive(false);
                    transformDeckThumbnail = null;
                }
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

            // Si la main est limité on ressers la taille de la main de jeu
            if (handLimited == true && deck.Count < transformsHand.Length) 
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

            for (int i = handLimitMin; i < handLimitMax; i++)
            {
                DrawCard(i, deck);
                cardControllers[i].MoveCard(transformsHand[i], 0);
            }

            if (maxCard == -1)
                maxCard = deck.Count;


            DrawDeckCount(deck);
            DrawThumbnail(index, deck);
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

            if (cardSpriteOffset.Length > index)
                cardControllers[index].MoveCardSprite(cardSpriteOffset[index]);

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
            MoveThumbnail(index, deck);
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

                // on décale le sprite comme il faut
                if (cardSpriteOffset.Length > i)
                    cardControllers[i].MoveCardSprite(cardSpriteOffset[i]);
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
                
                // on décale le sprite comme il faut
                if (cardSpriteOffset.Length > i)
                    cardControllers[i].MoveCardSprite(cardSpriteOffset[i]);
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

                    // on décale le sprite comme il faut
                    if (cardSpriteOffset.Length > i)
                        cardControllers[i].MoveCardSprite(cardSpriteOffset[i]);
                }
                cardControllers[handLimitMax - 1] = cardTemp;
                cardControllers[handLimitMax - 1].MoveCard(transformsHand[handLimitMax - 1], 0);
                DrawCard(handLimitMax - 1, deck);
            }

            DrawDeckCount(deck);
            DrawThumbnail(index, deck);
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

        private void DrawDeckCount(List<Card> deck)
        {
            if (cardCountScale)
            {
                float size = deck.Count / (float)maxCard;
                size = Mathf.Min(size, 1);
                cardCountScale.localScale = new Vector3(size, size, 1);
                textCardNumber.text = deck.Count.ToString();
            }
        }

        private void DrawThumbnail(int index, List<Card> deck)
        {
            if (deck.Count < 1 || transformDeckThumbnail == null)
                return;

            int startIndex = 0;
            int finalIndex = 0;
            int type = deck[0].GetCardType();
            int count = 0;
            float size = 0;

            for (int i = 1; i < deck.Count; i++)
            {
                if(deck[i].GetCardType() != type)
                {
                    // Draw element
                    finalIndex = i;
                    size = ((finalIndex - startIndex) / (float)deck.Count) * transformDeckThumbnail.sizeDelta.x;
                    if (elementsDeckThumbnail.Count <= count)
                        elementsDeckThumbnail.Add(Instantiate(elementDeckThumbnailPrefab, transformDeckThumbnail));

                    elementsDeckThumbnail[count].gameObject.SetActive(true);
                    elementsDeckThumbnail[count].color = cardTypeDictionary.GetColorType(type);
                    elementsDeckThumbnail[count].rectTransform.sizeDelta = new Vector2(size, elementsDeckThumbnail[count].rectTransform.sizeDelta.y);

                    // On enchaine
                    startIndex = i; 
                    type = deck[i].GetCardType();
                    count++;
                }
            }

            // On dessine le dernier élément
            finalIndex = deck.Count;
            size = ((finalIndex - startIndex) / (float)deck.Count) * transformDeckThumbnail.sizeDelta.x;
            if (elementsDeckThumbnail.Count <= count)
                elementsDeckThumbnail.Add(Instantiate(elementDeckThumbnailPrefab, transformDeckThumbnail));

            elementsDeckThumbnail[count].gameObject.SetActive(true);
            elementsDeckThumbnail[count].color = cardTypeDictionary.GetColorType(type);
            elementsDeckThumbnail[count].rectTransform.sizeDelta = new Vector2(size, elementsDeckThumbnail[count].rectTransform.sizeDelta.y);
            MoveThumbnail(index, deck);

            count++;
            for (int i = count; i < elementsDeckThumbnail.Count; i++)
            {
                elementsDeckThumbnail[i].gameObject.SetActive(false);
            }
        }

        private void MoveThumbnail(int index, List<Card> deck)
        {
            if (deck.Count < 1 || transformDeckThumbnail == null)
                return;

            float size = (1 / (float)deck.Count) * transformDeckThumbnail.sizeDelta.x; 
            cursorDeckThumbnail.sizeDelta = new Vector2(size, cursorDeckThumbnail.sizeDelta.y);
            cursorDeckThumbnail.anchoredPosition = new Vector2((index / (float)deck.Count) * transformDeckThumbnail.sizeDelta.x, cursorDeckThumbnail.anchoredPosition.y);
            cursorDeckThumbnail.SetAsLastSibling();
        }


        #endregion

    } 

} // #PROJECTNAME# namespace