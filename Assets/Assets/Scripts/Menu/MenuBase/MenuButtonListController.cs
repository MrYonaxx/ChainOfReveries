/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuButtonListController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        protected bool isDataList = false;
        public bool IsDataList
        {
            get { return isDataList; }
        }

        [Title("UI")]
        [SerializeField]
        [HideIf("isDataList")]
        protected RectTransform listTransform;
        public RectTransform ListTransform
        {
            get { return listTransform; }
        }

        [SerializeField]
        [HideIf("isDataList")]
        protected MenuButtonList prefabItem;

        [SerializeField] // List soit générer par les fonction en dessous, soit rempli dans l'éditeur si fixe
        [HideIf("isDataList")]
        protected List<MenuButtonList> listItem = new List<MenuButtonList>(); 
        public List<MenuButtonList> ListItem
        {
            get { return listItem; }
        }

        protected int indexSelection = 0;
        public int IndexSelection
        {
            get { return indexSelection; }
        }

        protected int listIndexCount = 0;



        [Title("Input")]

        [SerializeField]
        protected bool listLoop = true;
        [SerializeField]
        protected float stickThreshold = 0.8f;

        [SerializeField]
        protected int timeBeforeRepeat = 10;
        [SerializeField]
        protected int repeatInterval = 3;
        [SerializeField]
        protected int scrollSize = 3;
        [SerializeField]
        protected float scrollSpeed = 1;

        protected float currentTimeBeforeRepeat = -1;
        protected float currentRepeatInterval = -1;
        protected int lastDirection = 0; // 2 c'est bas, 8 c'est haut (voir numpad)
        protected int indexLimit = 0;

        protected IEnumerator coroutineScroll = null;



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
        protected virtual void Awake()
        {
            indexLimit = scrollSize;
            if(listItem.Count != 0)
                listIndexCount = listItem.Count;
        }

        // Génère la liste
        public void DrawItemList(int i, Sprite icon, string text, string subText = "")
        {
            if (isDataList == false)
            {
                if (i >= listItem.Count)
                    listItem.Add(Instantiate(prefabItem, listTransform));
                listItem[i].DrawButton(icon, text, subText);
                listItem[i].gameObject.SetActive(true);
                listIndexCount = listItem.Count;
            }
            else
                listIndexCount = i;
        }

        // Génère la liste
        public void DrawItemList(int i, string text) => DrawItemList(i, null, text, "");

        // Génère la liste
        public void SetItemCount(int count)
        {
            listIndexCount = count;
            for (int i = count; i < listItem.Count; i++)
            {
                listItem[i].gameObject.SetActive(false);
            }
        }


        public bool InputListVertical(float inputValue)
        {
            if (inputValue > stickThreshold)
            {
                return SelectUp();
            }
            else if (inputValue < -stickThreshold)
            {
                return SelectDown();
            }
            else if (Mathf.Abs(inputValue) <= 0.2f)
            {
                StopRepeat();
                return false;
            }
            return false;
        }

        public bool InputListHorizontal(float inputValue)
        {
            if (inputValue > stickThreshold)
            {
                return SelectDown();
            }
            else if (inputValue < -stickThreshold)
            {
                return SelectUp();
            }
            else if (Mathf.Abs(inputValue) <= 0.2f)
            {
                StopRepeat();
                return false;
            }
            return false;
        }

        public bool SelectUp()
        {
            if (listIndexCount <= 1)
            {
                return false;
            }
            if (lastDirection != 8)
            {
                StopRepeat();
                lastDirection = 8;
            }

            if (CheckRepeat() == false)
                return false;

            if(isDataList == false)
                listItem[indexSelection].UnselectButton();
            indexSelection -= 1;
            if (indexSelection <= -1)
            {
                if (listLoop)
                    indexSelection = listIndexCount - 1;
                else
                    indexSelection = 0;
            }
            if (isDataList == false)
                listItem[indexSelection].SelectButton();
            MoveScrollRect();
            return true;
        }



        public bool SelectDown()
        {
            if (listIndexCount <= 1)
            {
                return false;
            }
            if (lastDirection != 2)
            {
                StopRepeat();
                lastDirection = 2;
            }
            if (CheckRepeat() == false)
                return false;

            if (isDataList == false)
                listItem[indexSelection].UnselectButton();
            indexSelection += 1;
            if (indexSelection >= listIndexCount)
            {
                if (listLoop)
                    indexSelection = 0;
                else
                    indexSelection = listIndexCount - 1;
            }
            if (isDataList == false)
                listItem[indexSelection].SelectButton();
            MoveScrollRect();
            return true;
        }


        /// <summary>
        /// Set l'index de la liste et feedback sur boutons
        /// </summary>
        /// <param name="id"></param>
        public void SelectIndexButton(int id)
        {
            listItem[indexSelection].UnselectButton();
            indexSelection = id;
            listItem[indexSelection].SelectButton();
        }

        /// <summary>
        /// Set l'index de la liste uniquement
        /// </summary>
        /// <param name="id"></param>
        public void SelectIndex(int id)
        {
            indexSelection = id;
        }


        public void UpdateCountList()
        {
            listIndexCount = listItem.Count;
        }


        // Check si on peut repeter l'input
        protected bool CheckRepeat()
        {
            if (currentRepeatInterval == -100) // Nombre magique
            {
                if (currentTimeBeforeRepeat == -100) // Nombre magique
                {
                    currentTimeBeforeRepeat = timeBeforeRepeat * 0.016f; // (0.016f = 60 fps et opti de la division)
                    return true;
                }
                else if (currentTimeBeforeRepeat <= 0)
                {
                    currentRepeatInterval = repeatInterval * 0.016f;// (0.016f = 60 fps et opti de la division)
                }
                else
                {
                    currentTimeBeforeRepeat -= Time.deltaTime;
                }
            }
            else if (currentRepeatInterval <= 0)
            {
                currentRepeatInterval = repeatInterval * 0.016f;// (0.016f = 60 fps et opti de la division)
                return true;
            }
            else
            {
                currentRepeatInterval -= Time.deltaTime;
            }
            return false;
        }

        public void StopRepeat()
        {
            currentRepeatInterval = -100; // Nombre magique
            currentTimeBeforeRepeat = -100;// Nombre magique
        }


        // Gère si la liste est dans un scroll rect
        protected void MoveScrollRect()
        {
            if (listTransform == null)
            {
                //if (selectionTransform != null)
                 //   selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
                return;
            }
            if (indexSelection > indexLimit)
            {
                indexLimit = indexSelection;
                if (coroutineScroll != null)
                    StopCoroutine(coroutineScroll);
                coroutineScroll = MoveScrollRectCoroutine();
                StartCoroutine(coroutineScroll);
            }
            else if (indexSelection < indexLimit - scrollSize + 1)
            {
                indexLimit = indexSelection + scrollSize - 1;
                if (coroutineScroll != null)
                    StopCoroutine(coroutineScroll);
                coroutineScroll = MoveScrollRectCoroutine();
                StartCoroutine(coroutineScroll);
            }
            /*else
            {
                if (selectionTransform != null)
                    selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
            }*/

        }

        private IEnumerator MoveScrollRectCoroutine()
        {
            float t = 0f;
            float speed = scrollSpeed / 0.1f;
            int ratio = indexLimit - scrollSize;
            Vector2 destination = new Vector2(0, Mathf.Clamp(ratio * prefabItem.RectTransform.sizeDelta.y, 0, (listIndexCount - scrollSize) * prefabItem.RectTransform.sizeDelta.y));
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                listTransform.anchoredPosition = Vector2.Lerp(listTransform.anchoredPosition, destination, t);
                //selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
                yield return null;
            }
            listTransform.anchoredPosition = destination;
            //selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace