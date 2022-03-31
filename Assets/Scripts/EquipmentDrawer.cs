/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EquipmentDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [SerializeField]
        CardController[] cardControllers = null;
        [SerializeField]
        CardType equipmentCardType = null;

        [HideInInspector]
        public bool FirstTime = true;
        CardEquipment[] previousEquipments = null;
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

        public void DrawEquipments(CardEquipment[] cardEquipments)
        {
            // à opti dans les boucles en dessous parce qu'on double les appel actuellement
            int isEmpty = 0;
            for (int i = 0; i < cardControllers.Length; i++)
            {
                if (cardEquipments[i] == null)
                    isEmpty++;
            }
            if (isEmpty == cardControllers.Length)
                cardControllers[0].transform.parent.gameObject.SetActive(false);
            else
                cardControllers[0].transform.parent.gameObject.SetActive(true);



            if (FirstTime)
            {
                for (int i = 0; i < cardControllers.Length; i++)
                {
                    if (cardEquipments[i] != null)
                    {
                        cardControllers[i].gameObject.SetActive(true);
                        cardControllers[i].DrawCard(cardEquipments[i].GetCardIcon(), equipmentCardType.CardTypeColor[0]);
                    }
                    else
                    {
                        cardControllers[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                for (int i = 0; i < previousEquipments.Length; i++)
                {
                    if (previousEquipments[i] != null && cardEquipments[i] == null) // On vient de jouer cette carte 
                    {
                        // Feedback
                        cardControllers[i].Disappear();
                    }
                }
            }
            previousEquipments = cardEquipments;

        }






        #endregion

    } 

} // #PROJECTNAME# namespace