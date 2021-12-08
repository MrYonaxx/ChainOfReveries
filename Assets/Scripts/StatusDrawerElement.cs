using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class StatusDrawerElement : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Data")]
        [SerializeField]
        CardController card = null;

        [SerializeField]
        TextMeshProUGUI statusName = null;
        [SerializeField]
        TextMeshProUGUI statusLabel = null;



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

        public void DrawEquipmentStatus(Status status)
        {
            this.gameObject.SetActive(true);
            // On utilise la valeur du premier status update (à voir si ça pose problème)
            card.DrawCard(status.StatusEffect.StatusIconSprite, Color.black, status.StatusUpdate[0].ValueToDraw());
            statusName.text = status.StatusEffect.StatusName;
            statusLabel.text = status.StatusEffect.StatusConditionLabel;
        }





        #endregion

    }

}
