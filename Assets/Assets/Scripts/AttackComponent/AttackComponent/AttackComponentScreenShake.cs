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
    public class AttackComponentScreenShake: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [HorizontalGroup("ScreenShake")]
        [SerializeField]
        float screenShakePower = 0.1f;

        [HorizontalGroup("ScreenShake")]
        [SerializeField]
        int screenShakeTime =1;

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
        private void Awake()
        {

        }

        public void Shake()
        {
            BattleFeedbackManager.Instance?.ShakeScreen(screenShakePower, screenShakeTime);
        }



        #endregion

    } 

} // #PROJECTNAME# namespace