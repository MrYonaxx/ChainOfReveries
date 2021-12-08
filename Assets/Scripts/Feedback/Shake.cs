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

namespace VoiceActing
{
	public class Shake : MonoBehaviour
	{
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        bool isRectTransform = false;


        RectTransform rectTransform;

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

        protected void Start()
        {
            if(isRectTransform)
                rectTransform = GetComponent<RectTransform>();
        }


        [ContextMenu("Shake")]
        public void ShakeEffect()
        {
            if (isRectTransform)
                ShakeRectEffect();
            else
                StartCoroutine(ShakeCoroutine(0.1f, 30));
        }

        public void ShakeEffect(float power, int time)
        {
            StartCoroutine(ShakeCoroutine(power, time));
        }
        
        private IEnumerator ShakeCoroutine(float power, int time)
        {
            Vector3 start = this.transform.position;
            float speed = power / time;
            while (time != 0)
            {
                power -= speed;
                this.transform.position = new Vector3(start.x + Random.Range(-power, power), start.y + Random.Range(-power, power), start.z + Random.Range(-power, power));
                time -= 1;
                yield return null;
            }
            this.transform.position = start;
        }





        [ContextMenu("ShakeRect")]
        public void ShakeRectEffect()
        {
            StartCoroutine(ShakeCoroutineRectTransform(20, 30));
        }

        public void ShakeRectEffect(int power, int time)
        {
            StartCoroutine(ShakeCoroutineRectTransform(power, time));
        }

        private IEnumerator ShakeCoroutineRectTransform(float power, int time)
        {
            Vector3 start = rectTransform.anchoredPosition;
            float speed = power / time;
            while (time != 0)
            {
                power -= speed;
                rectTransform.anchoredPosition = new Vector3(start.x + Random.Range(-power, power), start.y + Random.Range(-power, power), start.z + Random.Range(-power, power));
                time -= 1;
                yield return null;
            }
            rectTransform.anchoredPosition = start;
        }

        #endregion

    } // Shake class
	
}// #PROJECTNAME# namespace
