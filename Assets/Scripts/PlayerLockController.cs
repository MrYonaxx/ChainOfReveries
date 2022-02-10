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
    public class PlayerLockController: TargetController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        string targetTag = "Enemy";

        [SerializeField]
        SpriteRenderer spriteDirection = null;
        [SerializeField]
        Vector3 lockOnSize;
        [SerializeField]
        Vector3 defaultSize;

        [SerializeField]
        Transform lockMarker = null;

        [SerializeField]
        List<CharacterBase> transformsLocked = new List<CharacterBase>();

        bool targetInList = false;
        bool colliderEnabled = true;

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
        private void Update()
        {
            if (spriteDirection.flipX == true)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (spriteDirection.flipX == false)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (colliderEnabled == false)
                return;
            if(collision.tag == targetTag)
            {
                transformsLocked.Add(collision.GetComponent<CharacterBase>());
                if (targetInList == false)
                {
                    this.transform.localScale = lockOnSize;
                    targetInList = true;
                    StartCoroutine(CheckShortestCoroutine());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (colliderEnabled == false)
                return;
            if (collision.tag == targetTag)
            {
                transformsLocked.Remove(collision.GetComponent<CharacterBase>());
                if (transformsLocked.Count == 0)
                {
                    targetInList = false;
                    this.transform.localScale = defaultSize;
                }
            }
        }

        private IEnumerator CheckShortestCoroutine()
        {
            while (targetInList == true)
            {
                if (!targeting) // Met en pause le processus
                {
                    yield return null;
                    continue;
                }

                float bestLength = 999;
                int bestIndex = 0;
                for (int i = 0; i < transformsLocked.Count; i++)
                {
                    float length = Vector3.Magnitude(transformsLocked[i].transform.position - transform.position);
                    if (bestLength >= length)
                    {
                        bestLength = length;
                        bestIndex = i;
                    }
                }
                if (lockMarker)
                {
                    lockMarker.gameObject.SetActive(true);
                    lockMarker.transform.SetParent(transformsLocked[bestIndex].transform);
                    lockMarker.transform.localPosition = new Vector3(0, 0.48f, 0);
                    lockMarker.transform.localScale = Vector3.one;
                }
                TargetLocked = transformsLocked[bestIndex];
                yield return null;
            }
            yield return null;
            TargetLocked = null;
            if (lockMarker)
            {
                lockMarker.gameObject.SetActive(false);
                lockMarker.transform.SetParent(this.transform);
            }
        }


        public override void StartTargeting()
        {
            //this.enabled = true;
            colliderEnabled = true;
        }
        public override void StopTargeting()
        {
            StopAllCoroutines();
            transformsLocked.Clear();
            if(lockMarker)
            {
                lockMarker.gameObject.SetActive(false);
                lockMarker.transform.SetParent(this.transform);
            }
            targetInList = false;
            this.transform.localScale = defaultSize;
            colliderEnabled = false;
        }



        #endregion

    } 

} // #PROJECTNAME# namespace