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
    public class StatusDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [SerializeField]
        Transform statusElementList = null;
        [SerializeField]
        StatusDrawerElement statusDrawerElement = null;


        List<StatusDrawerElement> statusDrawerElements = new List<StatusDrawerElement>();
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

        public void DrawStatus(List<Status> newStatus)
        {
            // ADD
            int equipCount = 0;
            for (int i = 0; i < newStatus.Count; i++)
            {
                if(!newStatus[i].StatusEffect.HideStatus)
                {
                    if (newStatus[i].StatusEffect.StatusShowLabel) // Si on affiche un label c'est que c'est une carte équipement
                    {
                        DrawEquipmentStatus(newStatus[i], equipCount);
                        equipCount++;
                    }
                    else
                    {
                        // On verra plus tard
                    }
                }
            }

            // Remove
            for (int i = equipCount; i < statusDrawerElements.Count; i++)
            {
                statusDrawerElements[i].gameObject.SetActive(false);
            }
        }

        public void DrawEquipmentStatus(Status status, int i)
        {
            if (i >= statusDrawerElements.Count)
            {
                statusDrawerElements.Add(Instantiate(statusDrawerElement, statusElementList));
                statusDrawerElements[statusDrawerElements.Count - 1].DrawEquipmentStatus(status);
            }
            else
            {
                statusDrawerElements[i].DrawEquipmentStatus(status);
            }
        }





        #endregion

    } 

} // #PROJECTNAME# namespace