﻿/*****************************************************************
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
    public class BattleFeedbackManager : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        private static BattleFeedbackManager instance = null;
        public static BattleFeedbackManager Instance
        {
            get { return instance; }
        }




        [SerializeField]
        private CameraBattleController cameraController = null;
        public CameraBattleController CameraController 
        { 
            get { return cameraController; } 
        }

        [SerializeField]
        private Shake cameraShake = null;

        [SerializeField]
        Animator zoom = null;

        [SerializeField]
        private RippleEffect rippleEffect = null;

        [SerializeField]
        GameObject animationDeath = null;
        [SerializeField]
        Animator postProcessDeath = null;


        [SerializeField]
        Animator animatorFlashBackground = null;

        [Title("CardBreakAnimation")]
        [SerializeField]
        Animator cardBreakAnimation = null;
        [SerializeField]
        Animator animatorPixelize = null;
        [SerializeField]
        Animator animatorTextCardBreak = null;



        List<IMotionSpeed> listMotionSpeed = new List<IMotionSpeed>();


        private IEnumerator motionSpeedCoroutine;

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



        public void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        // Ajoute un IMotionSpeed à la liste de ceux qui subissent le hitstop
        public void AddIMotionSpeed(IMotionSpeed character)
        {
            listMotionSpeed.Add(character);
        }
        public void RemoveIMotionSpeed(IMotionSpeed character)
        {
            listMotionSpeed.Remove(character);
        }

        /*public void AnimationDeath2(CharacterBase character)
        {
            Instantiate(animationDeath, character.transform.position, Quaternion.identity);
            cameraShake.ShakeEffect(0.2f, 20);
            zoom.SetTrigger("Zoom");
            SetBattleMotionSpeed(0, 0.6f);
        }*/

        public void AnimationDeath(CharacterBase character)
        {
            Instantiate(animationDeath, character.transform.position, Quaternion.identity);
            cameraShake.ShakeEffect(0.2f, 15);
            SetBattleMotionSpeed(0, 0.6f);
        }


        public void EndBattleMotionSpeed()
        {
            cameraShake.ShakeEffect(0.2f, 25);
            zoom.SetTrigger("BigZoom");
            for (int i = 0; i < listMotionSpeed.Count; i++)
            {
                listMotionSpeed[i].SetCharacterMotionSpeed(0);
            }

            if (motionSpeedCoroutine != null)
                StopCoroutine(motionSpeedCoroutine);
            motionSpeedCoroutine = EndBattleCoroutine(0.6f);
            StartCoroutine(motionSpeedCoroutine);
        }
        private IEnumerator EndBattleCoroutine(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            SetBattleMotionSpeed(0.2f, 2f);
        }






        public void SetBattleMotionSpeed(float motionSpeed, float time)
        {
            for(int i = 0; i < listMotionSpeed.Count; i++)
            {
                listMotionSpeed[i].SetCharacterMotionSpeed(motionSpeed);
            }

            if (motionSpeedCoroutine != null)
                StopCoroutine(motionSpeedCoroutine);
            motionSpeedCoroutine = MotionSpeedCoroutine(time);
            StartCoroutine(motionSpeedCoroutine);
        }

        private IEnumerator MotionSpeedCoroutine(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            for (int i = 0; i < listMotionSpeed.Count; i++)
            {
                listMotionSpeed[i].SetCharacterMotionSpeed(1);
            }
        }



        public void RippleScreen(float x, float y)
        {
            Vector3 pos = cameraController.Camera.WorldToViewportPoint(new Vector3(x, y, y));
            rippleEffect.EmitRipple(pos.x, pos.y);
        }

        public void ShakeScreen()
        {
            cameraShake.ShakeEffect();
        }

        public void ShakeScreen(float power, int time)
        {
            cameraShake.ShakeEffect(power, time);
        }

        public void CameraZoom()
        {
            zoom.SetTrigger("Zoom");
        }
        public void CameraDeZoom()
        {
            zoom.SetTrigger("DeZoom");
        }
        public void CameraBigZoom()
        {
            zoom.SetTrigger("BigZoom");
        }
        public void CameraBossZoom()
        {
            zoom.SetTrigger("BossZoom");
        }
        public void CameraSpecialZoom(int level)
        {
            zoom.SetTrigger("SpecialZoom");
            zoom.SetInteger("SpecialZoomLevel", level);
        }



        public void BloomDeath()
        {
            postProcessDeath.SetTrigger("Distortion");
        }
        public void BloomDeathBoss()
        {
            postProcessDeath.SetTrigger("Distortion2");
        }

        public void CardBreakAnimation(CharacterBase characterBreaked)
        {
            animatorTextCardBreak.transform.position = characterBreaked.transform.position;
            animatorTextCardBreak.SetTrigger("Feedback");

            cardBreakAnimation.transform.position = characterBreaked.transform.position;
            cardBreakAnimation.SetTrigger("Feedback");
           // Instantiate(cardBreakAnimation, characterBreaked.transform.position, Quaternion.identity);

            animatorPixelize.gameObject.SetActive(true);
            animatorPixelize.SetTrigger("CardBreak");
        }

        public void BackgroundFlash()
        {
            animatorFlashBackground.SetTrigger("Feedback");
        }

        /*public void SetCameraPriorityFocus(Transform newFocus)
        {
            cameraController.SetFocusPriority(newFocus);
        }*/

        /*public void RippleEffect(float x, float y)
        {
            Vector3 pos = cameraMain.WorldToViewportPoint(new Vector3(x, y, y));
            rippleEffect.EmitRipple(pos.x, pos.y);
        }*/

        /*public void CameraShake()
        {
            cameraShake.ShakeEffect();
        }*/

        #endregion

    } 

} // #PROJECTNAME# namespace