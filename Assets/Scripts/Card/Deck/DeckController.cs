/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class DeckController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Debug Data")]
        [SerializeField]
        List<Card> deckData;

        [Title("Debug")]
        [SerializeField]
        List<Card> deck;
        [SerializeField]
        List<Card> cemetery;
        [SerializeField]
        List<Card> banned;


        [Title("Move Deck")]
        [SerializeField]
        float timeFirstMoveCardInterval = 0.5f;
        [SerializeField]
        float timeMoveCardInterval = 0.1f;
        [SerializeField]
        float timeMoveCategoryInterval = 0.2f;

        [Title("Reload Parameter")]
        [SerializeField]
        bool addReload = true;
        [SerializeField]
        CharacterState stateReload = null;
        [SerializeField]
        CardData cardReload = null;
        [SerializeField]
        int reloadAnimationOffset = 7; // idéalement à set a la meme valeur que le nombre de transform du deck drawer pour un effet propre
        [SerializeField]
        int reloadMaxLevel = 3;
        [SerializeField]
        int reloadMaxAmount = 100;



        bool firstTimeMoveCard = false;
        float timeMoveCard = 0f;
        float timeMoveCategory = 0f;

        int reloadCurrentMaxLevel = 2;
        int reloadCurrentLevel = 1;
        float reloadCurrentAmount = 0;

        [ReadOnly]
        public int currentIndex = 1;
        bool canPlayCard = true;
        bool inReload = false;


        public delegate void Action();
        public delegate void ActionDeck(int i, List<Card> deck);
        public delegate void ActionReload(int reloadCurrentLevel, float reloadCurrentAmount, int reloadMaxAmount);

        public event Action OnReload;
        public event ActionReload OnReloadChanged;

        public event ActionDeck OnDeckChanged;
        public event ActionDeck OnCardPlayed;
        public event ActionDeck OnCardMoved;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */
        public int CurrentIndex
        {
            get { return currentIndex; }
        }

        public List<Card> Deck
        {
            get { return deck; }
        }

        public List<Card> DeckData
        {
            get { return deckData; }
        }

        public Card GetCurrentCard()
        {
            return deck[currentIndex];
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public bool GetCanPlayCard()
        {
            return canPlayCard;
        }
        public bool GetInReload()
        {
            return inReload;
        }

        public List<Card> GetDeck()
        {
            return deck;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public void SetDeck(List<Card> cards)
        {
            deckData.Clear();
            deck.Clear();
            banned.Clear();

            currentIndex = 0;
            reloadCurrentLevel = 1;
            reloadCurrentMaxLevel = 2;

            // Ajoute la carte reload
            if (addReload)
            {
                deck.Add(new CardReload(cardReload));
                deck[0].CardID = 0;
                deckData.Add(deck[0]);
                currentIndex = 1;
            }

            // ajoute les cartes au deck
            for (int i = 0; i < cards.Count; i++)
            {
                deck.Add(cards[i]);
                deck[deck.Count - 1].CardID = i+1; // Utilisé pour replacé les cartes dans le bon ordre
                deck[deck.Count - 1].ResetCard();
                deckData.Add(deck[deck.Count - 1]);
            }
            OnDeckChanged?.Invoke(currentIndex, deck);
        }

        /// <summary>
        /// Utilisé pour redessiné le deck 
        /// </summary>
        public void RefreshDeck()
        {
            OnDeckChanged?.Invoke(currentIndex, deck);
        }


        public void UpdateCard(CharacterBase character)
        {
            if (deck.Count == 0)
                return;
            deck[currentIndex].UpdateCard(character);
        }




        public void MoveHand(bool left, bool right)
        {
            if (inReload)
                return;
            if (left && right)
                return;

            if (left || right)
            {
                if (timeMoveCard <= 0)
                {
                    if(right)
                        MoveHandRight();
                    else
                        MoveHandLeft();
                    if (firstTimeMoveCard == false)
                    {
                        firstTimeMoveCard = true;
                        timeMoveCard = timeFirstMoveCardInterval;
                    }
                    else
                    {
                        timeMoveCard = timeMoveCardInterval;
                    }
                }
            }

            if (left == false && right == false)
            {
                firstTimeMoveCard = false;
                timeMoveCard = 0;
            }

            if (timeMoveCard > 0)
                timeMoveCard -= Time.deltaTime;
        }

        private void MoveHandLeft()
        {
            currentIndex -= 1;
            if(currentIndex <= -1)
            {
                currentIndex = deck.Count - 1;
            }
            OnCardMoved?.Invoke(currentIndex, deck);
        }

        private void MoveHandRight()
        {
            currentIndex += 1;
            if (currentIndex >= deck.Count)
            {
                currentIndex = 0;
            }
            OnCardMoved?.Invoke(currentIndex, deck);
        }






        public void MoveCategory(bool left, bool right)
        {
            if (left && right)
            {
                currentIndex = 0;
                OnDeckChanged?.Invoke(currentIndex, deck);
                return;
            }

            if (left || right)
            {
                if (timeMoveCategory <= 0)
                {
                    if (right)
                        MoveCategoryRight();
                    else
                        MoveCategoryLeft();

                    OnDeckChanged?.Invoke(currentIndex, deck);

                    timeMoveCategory = timeMoveCategoryInterval;
                }
            }

            if (left == false && right == false)
            {
                timeMoveCategory = 0;
            }

            if (timeMoveCategory > 0)
                timeMoveCategory -= Time.deltaTime;
        }

        private void MoveCategoryLeft()
        {
            CardData currentCard = GetCurrentCard().CardData;
            CardData cardFound = null;
            if (currentIndex == 0)
                currentIndex = deck.Count - 1;
            for (int i = currentIndex; i >= 0; i--)
            {
                if (deck[i].CardData != currentCard && cardFound == null)
                {
                    cardFound = deck[i].CardData;
                }
                else if (deck[i].CardData != cardFound && cardFound != null)
                {
                    currentIndex = i+1;
                    return;
                }
            }
            currentIndex = 0;
        }

        private void MoveCategoryRight()
        {
            CardData currentCard = GetCurrentCard().CardData;

            for (int i = currentIndex; i < deck.Count; i++)
            {
                if (deck[i].CardData != currentCard)
                {
                    currentIndex = i;
                    return;
                }
            }
            currentIndex = 0;
        }



        public Card SelectCard()
        {
            Card res = deck[currentIndex];
            cemetery.Add(deck[currentIndex]);
            deck.RemoveAt(currentIndex);
            if (currentIndex >= deck.Count)
            {
                currentIndex = 0;
            }
            OnCardPlayed?.Invoke(currentIndex, deck);
            return res;
        }

        // Replace les cartes sélectionnés dans le deck
        public void ReplaceCard(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                ReplaceCard(cards[i]);
            }
            OnDeckChanged?.Invoke(currentIndex, deck);
        }

        public void ReplaceCard(Card card)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                if (card.CardID < deck[i].CardID)
                {
                    deck.Insert(i, card);
                    if (i <= currentIndex) // Si on replace avant l'index on doit décaler pour rester sur la même carte
                        currentIndex += 1;
                    return;
                }
                    
            }
            // Si on a jamais trouvé, on replace à la toute fin
            deck.Add(card);
        }


        // Banni les cartes
        public void BanishCard(Card card)
        {
            banned.Add(card);
        }

        public void UnbanishCard(Card card)
        {
            banned.Remove(card);
        }


        public void Remove(int index)
        {
            deck.RemoveAt(index);
            if (index <= currentIndex)
                currentIndex -= 1;
            if (currentIndex < 0)
                currentIndex = 0;
        }








        public CharacterState GetStateReload()
        {
            return stateReload;
        }


        public bool AddReload(float reloadAmount)
        {
            reloadCurrentAmount += (reloadAmount * Time.deltaTime);
            OnReloadChanged?.Invoke(reloadCurrentMaxLevel - reloadCurrentLevel, reloadCurrentAmount, reloadMaxAmount);
            if (reloadCurrentAmount >= reloadMaxAmount)
            {
                reloadCurrentAmount = 0;
                reloadCurrentLevel += 1;
                if (reloadCurrentLevel >= reloadCurrentMaxLevel)
                {
                    reloadCurrentLevel = 1;
                    reloadCurrentMaxLevel += 1;
                    reloadCurrentMaxLevel = Mathf.Clamp(reloadCurrentMaxLevel, 0, reloadMaxLevel);
                    ReloadDeck();
                    return true;
                }
            }
            return false;
            /*if (deckDrawer != null)
                deckDrawer.DrawReload(reloadCurrentLevel, reloadCurrentAmount, reloadMaxAmount);*/
        }

        public void ReloadDeck()
        {

            OnReloadChanged?.Invoke(reloadCurrentMaxLevel - reloadCurrentLevel, reloadCurrentAmount, reloadMaxAmount);
            deck.Clear();

            // On rajoute les cartes du deck Data dans l'ordre du début de combat
            for(int i = 0; i < deckData.Count; i++)
            {
                deck.Add(deckData[i]);
            }

            // On enlève les cartes bannies
            for(int i = 0; i < banned.Count; i++)
            {
                deck.Remove(banned[i]);
            }
            cemetery.Clear();
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            canPlayCard = false;
            inReload = true;
            currentIndex = 1 + reloadAnimationOffset;
            currentIndex = currentIndex % deck.Count;
            int shuffleTime = deck.Count + reloadAnimationOffset;
            OnDeckChanged?.Invoke(currentIndex, deck);
            OnReload?.Invoke();
            while (shuffleTime > 0)
            {
                yield return new WaitForSeconds(0.05f);
                MoveHandLeft();
                shuffleTime -= 1;
            }
            inReload = false;
            canPlayCard = true;
        }








        // Util

        public int CalculateNearestPath(int cardIndex)
        {
            int leftPath = currentIndex - cardIndex;
            if (leftPath < 0)
                leftPath += Deck.Count;

            int rightPath = cardIndex - currentIndex;
            if (rightPath < 0)
                rightPath += Deck.Count;

            if (leftPath < rightPath)
                return -leftPath;
            else
                return rightPath;
        }

        public List<int> SearchCards(CardData cardToSearch, int initialIndex, Vector2Int range)
        {
            List<int> cardsIndex = new List<int>();

            int searchIndex = initialIndex + range.x;
            if (searchIndex < 0)
                searchIndex += deck.Count;
            searchIndex = Mathf.Max(0, searchIndex);

            for (int i = 0; i < (range.y - range.x); i++)
            {
                if (searchIndex >= deck.Count)
                    searchIndex = 0;
                if (deck[searchIndex].CardData == cardToSearch)
                    cardsIndex.Add(searchIndex);
                searchIndex += 1;
            }

            return cardsIndex;
        }



        #endregion

    } 

} // #PROJECTNAME# namespace