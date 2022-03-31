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

namespace Feedbacks
{
    public class AuraSpriteEffect : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer auraSprite = null;
        [SerializeField]
        SpriteRenderer characterSprite = null;

        bool isLooping = false;

        private IEnumerator auraCoroutine;

        private void Start()
        {
            auraSprite.transform.SetParent(characterSprite.transform);
        }

        // En frame
        public void AuraFeedback(int time, float scale, Color color, bool loop = false)
        {
            AuraFeedback(time / 60f, scale, color, loop);
        }

        // En seconde
        public void AuraFeedback(float time, float scale, Color color, bool loop = false)
        {
            auraSprite.material.SetColor("_Color", color);
            isLooping = loop;

            if (auraCoroutine != null)
                StopCoroutine(auraCoroutine);
            auraCoroutine = AuraCoroutine(time, scale);
            StartCoroutine(auraCoroutine);
        }


        public void AuraFeedbackReverse(float time, float scale, Color color, bool loop = false)
        {
            auraSprite.material.SetColor("_Color", color);
            isLooping = loop;

            if (auraCoroutine != null)
                StopCoroutine(auraCoroutine);
            auraCoroutine = AuraCoroutineReverse(time, scale);
            StartCoroutine(auraCoroutine);
        }


        public void EndAuraFeedback()
        {
            isLooping = false;
        }


        private IEnumerator AuraCoroutine(float time, float scale)
        {
            float t = 0f;
            Vector3 finalScale = new Vector3(scale, scale, scale);
            Color transparent = new Color(1, 1, 1, 0);
            while(t < time)
            {
                t += Time.deltaTime;
                auraSprite.transform.localScale = Vector3.Lerp(Vector3.one, finalScale, t / time);
                auraSprite.color = Color.Lerp(Color.white, transparent, t / time);
                yield return null;
                if (t >= time && isLooping)
                    t = 0f;
            }
            auraSprite.color = transparent;
        }

        private IEnumerator AuraCoroutineReverse(float time, float scale)
        {
            float t = 0f;
            Vector3 finalScale = new Vector3(scale, scale, scale);
            Color transparent = new Color(1, 1, 1, 0);
            while (t < time)
            {
                t += Time.deltaTime;
                auraSprite.transform.localScale = Vector3.Lerp(finalScale, Vector3.one, t / time);
                auraSprite.color = Color.Lerp(Color.white, transparent, t / time);
                yield return null;
                if (t >= time && isLooping)
                    t = 0f;
            }
            auraSprite.color = transparent;
        }

        private void LateUpdate()
        {
            auraSprite.sprite = characterSprite.sprite;
        }

    }
}
