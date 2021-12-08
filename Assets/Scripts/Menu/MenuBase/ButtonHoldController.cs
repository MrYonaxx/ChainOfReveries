/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [System.Serializable]
    public class ButtonHoldController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Image imageAmount;

        [SerializeField]
        float maxAmount = 0.6f;
        [SerializeField]
        float amountReduction = 0.1f;

        float currentAmount = 0;

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

        public void ResetButton()
        {
            currentAmount = 0;
            imageAmount.fillAmount = currentAmount / maxAmount;
        }

        public bool HoldButton(bool hold)
        {
            if (hold)
            {
                currentAmount += Time.deltaTime;
                imageAmount.fillAmount = currentAmount / maxAmount;
                if (currentAmount >= maxAmount)
                {
                    return true;
                }
            }
            else
            {
                currentAmount -= (amountReduction * 10 * Time.deltaTime);
                if (currentAmount < 0)
                    currentAmount = 0;
                imageAmount.fillAmount = currentAmount / maxAmount;
            }
            return false;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace