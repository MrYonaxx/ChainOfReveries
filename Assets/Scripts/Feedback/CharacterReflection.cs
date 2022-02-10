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
    public class CharacterReflection: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        SpriteRenderer spriteReflection;
        [SerializeField]
        SpriteRenderer spriteToReflect;
        [SerializeField]
        float disappearTime = 5f;

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

        public void Update()
        {
            this.transform.localPosition = new Vector3(spriteToReflect.transform.localPosition.x, this.transform.localPosition.y, 0);
            spriteReflection.sprite = spriteToReflect.sprite;
            spriteReflection.flipX = spriteToReflect.flipX;
        }


        public void Disappear()
        {
            StartCoroutine(LerpAlpha(disappearTime));
        }

        IEnumerator LerpAlpha(float time)
        {
            float t = 0f;
            Color baseColor = spriteReflection.color;
            Color finalColor = spriteReflection.color;
            finalColor.a = 0;
            while (t < time)
            {
                t += Time.deltaTime;
                spriteReflection.color = Color.Lerp(baseColor, finalColor, t / time);
                yield return null;
            }

        }
        #endregion

    } 

} // #PROJECTNAME# namespace