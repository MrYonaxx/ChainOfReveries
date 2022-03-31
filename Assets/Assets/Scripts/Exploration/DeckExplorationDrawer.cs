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
    public class DeckExplorationDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public CardType cardType = null;
        public CardType CardType
        {
            get { return cardType; }
        }


        [Space]
        [SerializeField]
        RectTransform transformParent = null;
        [SerializeField]
        CardController cardController = null;
        [SerializeField]
        RectTransform cursorSelection = null;

        //[SerializeField]
        //MenuCursor menuCursor; // Remplacer le cursorSelection par MenuCursor

        int currentIndex = 0;
        int size = 0;
        List<CardController> cardsList = new List<CardController>();
        IEnumerator cursorCoroutine = null;

        #endregion


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public CardController GetCardController()
        {
            return cardsList[currentIndex];
        }
        public CardController GetCardController(int index)
        {
            // jsp j'ai eu un out of range une fois donc on secure
            if(index >= cardsList.Count)
                return cardsList[cardsList.Count-1];
            return cardsList[index];
        }

        public int GetCardCount()
        {
            return size;
        }

        public void CreateDeckExploration(List<CardExplorationData> deck)
        {
            currentIndex = 0;
            size = deck.Count;
            for (int i = 0; i < deck.Count; i++)
            {
                if (i >= cardsList.Count)
                    cardsList.Add(Instantiate(cardController, transformParent));
                cardsList[i].DrawCard(deck[i].CardSprite, cardType.GetColorType(deck[i].CardType));
            }
            for (int i = deck.Count; i < cardsList.Count; i++)
            {
                cardsList[i].gameObject.SetActive(false);
            }

            if (cursorCoroutine != null)
                StopCoroutine(cursorCoroutine);
            cursorCoroutine = MoveCursorCoroutineIntro();
            StartCoroutine(cursorCoroutine);
        }

        public void MoveCursor(int direction)
        {
            currentIndex += direction;
            if (currentIndex < 0)
                currentIndex = cardsList.Count - 1;
            if (currentIndex >= cardsList.Count || cardsList[currentIndex].gameObject.activeInHierarchy == false)
                currentIndex = 0;

            if (cursorCoroutine != null)
                StopCoroutine(cursorCoroutine);
            cursorCoroutine = MoveCursorCoroutine(new Vector2(cardsList[currentIndex].transform.position.x, cardsList[currentIndex].transform.position.y));
            StartCoroutine(cursorCoroutine);
        }

        // patch pour gérer les layout group de Unity qui s'initialise au start, donc on attend une frame pour déplace le curseur
        private IEnumerator MoveCursorCoroutineIntro()
        {
            yield return null;
            yield return null;
            yield return MoveCursorCoroutine(new Vector2(cardsList[currentIndex].transform.position.x, cardsList[currentIndex].transform.position.y));
        }

        private IEnumerator MoveCursorCoroutine(Vector2 finalPos)
        {
            if (cursorSelection == null)
                yield break;

            float time = 60f;//Mathf.Max(1, 1); // N'oublie pas de serializer ça quand t'aura plus la flemme
            time /= 60f;
            float speed = 0.1f;
            float t = 0f;
            float blend = 0f;
            Vector2 finalPosition = finalPos;

            while (t < 1f)
            {
                t += Time.deltaTime / time;
                blend = 1f - Mathf.Pow(1f - speed, Time.deltaTime * 60);
                cursorSelection.transform.position = Vector2.Lerp(cursorSelection.transform.position, finalPosition, blend);
                yield return null;
            }
            cursorSelection.transform.position = finalPos;
            cursorCoroutine = null;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace