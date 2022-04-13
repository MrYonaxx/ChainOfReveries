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
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CardController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Card")]
        [SerializeField]
        protected RectTransform rectTransform = null;
        [SerializeField]
        protected Image cardSprite = null;
        [SerializeField]
        protected Image cardOutlineBackground = null;

        [SerializeField]
        protected Image cardOutline = null;
        [SerializeField]
        protected Animator animator = null;

        [SerializeField]
        TextMeshProUGUI textCardValue = null;
        [SerializeField]
        Image cardValueOutline = null;



        IEnumerator coroutine = null;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public RectTransform GetRectTransform()
        {
            return rectTransform;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        public void DrawCard(Card card, CardType cardTypeDictionary)
        {
            DrawCard(card.GetCardIcon(), cardTypeDictionary.GetColorType(card.GetCardType()), card.GetCardValue(), card.CardPremium);

            if(animator != null && animator.isActiveAndEnabled)
                animator.SetTrigger("Appear");
        }

        public void DrawCard(Sprite card, Color colorOutline, int value = -1, bool premium = false)
        {
            gameObject.SetActive(true);

            if (card == null)
                return;

            cardSprite.sprite = card;
            cardOutline.color = colorOutline;
            if(cardOutlineBackground != null)
                cardOutlineBackground.color = new Color(colorOutline.r, colorOutline.g, colorOutline.b, cardOutlineBackground.color.a * 0.5f);

            // Dessine la valeur (on cache l'outline si on a aucune valeur à dessiner
            if (value != -1)
            {
                textCardValue.gameObject.SetActive(true);
                cardValueOutline.gameObject.SetActive(true);
                textCardValue.text = value.ToString();
                cardValueOutline.color = colorOutline;
            }
            else
            {
                textCardValue.gameObject.SetActive(false);
                cardValueOutline.gameObject.SetActive(false);
            }

            if(premium)
                textCardValue.color = Color.yellow;
            else
                textCardValue.color = Color.white;
        }

        public void DrawCardValue(int value = -1)
        {
            textCardValue.text = value.ToString();
        }

        // déplace le sprite de la carte, utilisé pour créer une illusion avec le deck battle drawer pour toujours voir l'icone de la carte
        public void MoveCardSprite(float offset)
        {
            cardSprite.rectTransform.anchoredPosition = new Vector2(offset, 0);
        }


        // Désaffiche les cartes mais avec une anim
        public void Disappear()
        {
            if (animator != null)
                animator.SetTrigger("Disappear");
        }
        public void Appear()
        {
            if (animator != null)
                animator.SetTrigger("Appear");
        }
        public void HideCard()
        {
            gameObject.SetActive(false);
        }



        // Déplace la Carte
        public void MoveCard(RectTransform newTransform, float time)
        {
            this.transform.SetParent(newTransform);
            //rectTransform.localScale = new Vector3(1, 1, 1);
            if (gameObject.activeInHierarchy == false)
                rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            else
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
                coroutine = MoveToOrigin(time);
                StartCoroutine(coroutine);
            }
        }

        private IEnumerator MoveToOrigin(float time)
        {
            
            time = Mathf.Max(1, time);
            time /= 60f;
            //float speed = 0.1f;
            float t = 0f;
            //float blend = 0f;
            Vector2 finalPosition = Vector2.zero;
            Vector3 finalScale = Vector3.one;

            while (t < 1f && time > 0)
            {
                t += Time.deltaTime / time;
                //blend = 1f - Mathf.Pow(1f - speed, Time.deltaTime * 60);
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, finalPosition, t);
                rectTransform.localScale = Vector2.Lerp(rectTransform.localScale, finalScale, t);
                yield return null;
            }
            rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            coroutine = null;
        }

        public void MoveCard(RectTransform newTransform, float time, Vector2 offset)
        {
            this.transform.SetParent(newTransform);
            if (gameObject.activeInHierarchy == false)
                rectTransform.anchoredPosition3D = new Vector3(0 + offset.x, 0 + offset.y, 0);
            else
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
                coroutine = MoveToOrigin(time, offset);
                StartCoroutine(coroutine);
            }
        }

        private IEnumerator MoveToOrigin(float time, Vector2 offset)
        {

            time = Mathf.Max(1, time);
            time /= 60f;
            //float speed = 0.1f;
            float t = 0f;
            //float blend = 0f;
            Vector2 finalPosition = Vector2.zero + offset;
            Vector3 finalScale = Vector3.one;

            while (t < 1f && time != 0)
            {
                t += Time.deltaTime / time;
                //blend = 1f - Mathf.Pow(1f - speed, Time.deltaTime * 60);
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, finalPosition, t);
                rectTransform.localScale = Vector2.Lerp(rectTransform.localScale, finalScale, t);
                yield return null;
            }
            rectTransform.anchoredPosition3D = finalPosition;
            coroutine = null;
        }




        // Animation spécifique au Card Break
        [ContextMenu("Test")]
        public void DebugCardBreak()
        {
            CardBreakAnimation(1000, 5000, 1000, 30);
        }

        public void CardBreakAnimation(float inertiaX, float inertiaY, float gravityY, float time)
        {
            //this.transform.SetParent(newTransform);
            //rectTransform.localScale = new Vector3(1, 1, 1);
            animator.Play("Anim_CardController_CardBreak");
            if (gameObject.activeInHierarchy == true)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
                coroutine = CardBreakCoroutine(inertiaX, inertiaY, gravityY, time);
                StartCoroutine(coroutine);
            }
        }

        private IEnumerator CardBreakCoroutine(float inertiaX, float inertiaY, float gravityY, float time)
        {

            time = Mathf.Max(1, time);
            time /= 60f;
            float t = 0f;
            Vector2 currentInertia = new Vector2(inertiaX, inertiaY);
            while (t < 1f && time != 0)
            {
                t += Time.deltaTime / time;
                rectTransform.anchoredPosition += currentInertia * Time.deltaTime;
                currentInertia -= new Vector2(0, gravityY);
                yield return null;
            }
            //rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            coroutine = null;
            HideCard();
        }





        public void PlayAnimation(string animationName)
        {
            animator.Play(animationName);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace