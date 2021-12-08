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
    public class CardReloadController: CardController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Reload")]
        [SerializeField]
        TextMeshProUGUI textReloadLevel;
        [SerializeField]
        RectTransform transformReloadAmount;
        [SerializeField]
        Animator animatorReload;

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
        public void DrawReload(int reloadLevel, float reloadAmount, float reloadAmountMax)
        {
            gameObject.SetActive(true);
            textReloadLevel.text = reloadLevel.ToString();
            transformReloadAmount.localScale = new Vector3(1, reloadAmount / reloadAmountMax, 1);
            cardOutline.color = Color.white;
            animatorReload.SetTrigger("Feedback");
        }
        public void ShowReload()
        {
            gameObject.SetActive(true);
        }
        public void HideReload()
        {
            gameObject.SetActive(false);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace