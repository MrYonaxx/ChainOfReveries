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
    // Un scriptable object comme ça j'ai pas a devoir setup chaque ennemi via un autre battleManager, via injection ou event.
    // ça va me permettre aussi de faire des prefab drag n dropable sans utiliser une factory ou je sais pas quoi.

    // Une autre alternative c'est le singleton

    [CreateAssetMenu(fileName = "CardBreakController", menuName = "GameSystem/CardBreakController", order = 1)]
    public class CardBreakController : ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [InfoBox("Equivalent d'un singleton")]

        CharacterBase currentCharacter = null;
        List<Card> cardsActive = new List<Card>();
        CardBreakComponent currentCBComponent = null;

        // Contient les infos du joueurs qui s'est fait card break
        // Relevant pour Zantetsuken par exemple
        CharacterBase characterBreaked = null;
        public CharacterBase CharacterBreaked
        {
            get { return characterBreaked; }
        }

        List<Card> cardsBreaked = new List<Card>();
        public List<Card> CardsBreaked
        {
            get { return cardsBreaked; }
        }

        public delegate void ActionListCard(CharacterBase user, List<Card> card);
        public event ActionListCard OnCardPlayed;


        public delegate void ActionCardBreak(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker);
        public event ActionCardBreak OnCardIneffective;
        public event ActionCardBreak OnCardBreak;
        public event ActionCardBreak OnCardBreakDraw;

        public delegate void Action();
        public event Action OnCardRemove;



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

        // Return true si la carte a bien été joué
        public bool PlayCard(CharacterBase user, List<Card> cards, CardBreakComponent cardBreakComponent = null)
        {
            if (currentCharacter != null)
            {
                if (user.tag == currentCharacter.tag && user != currentCharacter) // Si un ennemi joue une carte
                    return false;
            }

            if (cardBreakComponent == null)
                cardBreakComponent = cards[0].CardData.CardBreakComponent;

            if (currentCharacter != null)
            {
                if (user.tag != currentCharacter.tag) // Si le challenger est different du currentCharacter on check
                {
                    int cardBreak = CheckCardBreak(user, cards, cardBreakComponent);
                    if (cardBreak == -1) // Pas de cardBreak
                    {
                        OnCardIneffective?.Invoke(currentCharacter, cardsActive, user, cards);
                        return false;
                    }
                    if (cardBreak == 0) // Egalité
                    {
                        OnCardBreakDraw?.Invoke(currentCharacter, cardsActive, user, cards);
                        currentCharacter.CharacterAction.CardBreak(user);
                        user.CharacterAction.CardBreak(currentCharacter);

                        currentCharacter = null;
                        cardsActive.Clear();

                        return false;
                    }
                    else if (cardBreak == 1) // Card Break
                    {
                        OnCardBreak?.Invoke(currentCharacter, cardsActive, user, cards);
                        currentCharacter.CharacterAction.CardBreak(user);

                        characterBreaked = currentCharacter;
                        cardsBreaked = cardsActive;

                        currentCharacter = user;
                        cardsActive = new List<Card>(cards);
                        currentCBComponent = cardBreakComponent;

                        OnCardPlayed?.Invoke(user, cards);
                        return true;
                    }
                }
            }

            // Si y'a rien on joue tranquille 
            RemoveCurrentCards();
            characterBreaked = null;
            cardsBreaked = null;

            currentCharacter = user;
            cardsActive = new List<Card>(cards);
            currentCBComponent = cardBreakComponent;

            OnCardPlayed?.Invoke(user, cards);
            return true;
        }


        
        // 1 = CardBreak, 0 = égalité, -1 = Inefficace
        public int CheckCardBreak(CharacterBase challenger, List<Card> newCards, CardBreakComponent cardBreakComponent)
        {
            // Check les protections du currentCharacter
            List<CardBreakComponent> cardBreakComponents = currentCharacter.CharacterAction.cardBreakComponents;
            for (int i = 0; i < cardBreakComponents.Count; i++)
            {
                if (cardBreakComponents[i].CheckCardBreak(currentCharacter, cardsActive, challenger, newCards) == -1)
                    return -1;
            }

            // Check si la carte actuelle peut se faire card break
            int res = currentCBComponent.CheckCardBreak(currentCharacter, cardsActive, challenger, newCards);
           
            // Check si le résultat satisfait à newCards
            res = cardBreakComponent.ContestCardBreak(res, currentCharacter, cardsActive, challenger, newCards);

            return res;
        }



        public void RemoveCurrentCards()
        {
            currentCBComponent = null;
            currentCharacter = null;
            cardsActive.Clear();
            OnCardRemove?.Invoke();
        }



        public void ForceCardBreak(CharacterBase character = null)
        {
            OnCardBreak.Invoke(currentCharacter, cardsActive, character, null);
            currentCharacter.CharacterAction.CardBreak(character);

            currentCharacter = null;
            cardsActive.Clear();
        }




        public CharacterBase GetActiveCharacter()
        {
            return currentCharacter;
        }
        public List<Card> GetActiveCards()
        {
            return cardsActive;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace