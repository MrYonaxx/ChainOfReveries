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
    public class GravityEffect: MonoBehaviour
    {

        private IEnumerator gravityCoroutine;

        public void Gravity(float timeTransition, float timeGravity)
        {
            if (gravityCoroutine != null)
                StopCoroutine(gravityCoroutine);
            gravityCoroutine = GravityCoroutine(timeTransition, timeGravity);
            StartCoroutine(gravityCoroutine);
        }

        private IEnumerator GravityCoroutine(float timeTransition, float timeGravity)
        {
            float t = 0f;
            float speed = 1f / timeTransition;
            Vector3 start = Vector3.one;
            Vector3 end = new Vector3(1, 0.2f, 1);
            while (t < 1f)
            {
                this.transform.localScale = Vector3.Lerp(start, end, t);
                t += Time.deltaTime * speed;
                yield return null;
            }

            yield return new WaitForSeconds(timeGravity);


            t = 0f;
            speed *= 2;
            while (t < 1f)
            {
                this.transform.localScale = Vector3.Lerp(end, start, t);
                t += Time.deltaTime * speed;
                yield return null;
            }
            this.transform.localScale = start;
        }

    } 

} // #PROJECTNAME# namespace