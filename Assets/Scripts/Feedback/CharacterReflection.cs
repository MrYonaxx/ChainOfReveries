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

        Color baseColor;
        Color finalColor;
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
        private void Start()
        {
            baseColor = spriteReflection.color;
            finalColor = spriteReflection.color;
            finalColor.a = 0;


            spriteReflection.color = Color.Lerp(finalColor, baseColor, spriteToReflect.color.a);
        }

        public void Update()
        {
            this.transform.localPosition = new Vector3(spriteToReflect.transform.localPosition.x, this.transform.localPosition.y, 0);
            spriteReflection.sprite = spriteToReflect.sprite;
            spriteReflection.flipX = spriteToReflect.flipX;

            spriteReflection.color = Color.Lerp(finalColor, baseColor, spriteToReflect.enabled ? spriteToReflect.color.a : 0);
        }


        public void Disappear()
        {
            if(isActiveAndEnabled)
                StartCoroutine(LerpAlpha(disappearTime)); 
        }

        IEnumerator LerpAlpha(float time)
        {
            enabled = false;
            float t = 0f;
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