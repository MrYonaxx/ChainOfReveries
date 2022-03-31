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
    [CreateAssetMenu(fileName = "StatusData", menuName = "Status", order = 1)]
    public class StatusEffectData: SerializedScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [HorizontalGroup("StatusBasicInfo", Width = 96)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        private Sprite statusIconSprite;
        public Sprite StatusIconSprite
        {
            get { return statusIconSprite; }
        }


        [HorizontalGroup("StatusBasicInfo")]
        [VerticalGroup("StatusBasicInfo/Right")]
        [SerializeField]
        string statusName;
        public string StatusName
        {
            get { return statusName; }
        }

        [HorizontalGroup("StatusBasicInfo")]
        [VerticalGroup("StatusBasicInfo/Right")]
        [SerializeField]
        [TextArea(4,4)]
        string statusDescription;
        public string StatusDescription
        {
            get { return statusDescription; }
        }



        /* [Title("Status Parameter")]
         [SerializeField]
         Vector2 statusTime;
         public Vector2 StatusTime
         {
             get { return statusTime; }
         }

         [SerializeField]
         private bool canRefresh = false;
         public bool CanRefresh
         {
             get { return canRefresh; }
         }

         [SerializeField]
         private bool canStack = false;
         public bool CanStack
         {
             get { return canStack; }
         }

         [SerializeField]
         private bool canAddMultiple = false;
         public bool CanAddMultiple
         {
             get { return canAddMultiple; }
         }

         [Space]
         [Space]
         [SerializeField]
         private bool statusPercentageBonus = false;
         public bool StatusPercentageBonus
         {
             get { return statusPercentageBonus; }
         }


         [SerializeField]
         CharacterStat characterStatModifier;
         public CharacterStat CharacterStatModifier
         {
             get { return characterStatModifier; }
         }*/


        //[HorizontalGroup("Status")]
        [Space]
        [Space]
        [ListDrawerSettings(AlwaysAddDefaultValue = true, Expanded = true)]

        [Sirenix.Serialization.OdinSerialize]
        List<StatusEffectUpdate> statusUpdates = new List<StatusEffectUpdate>();
        public List<StatusEffectUpdate> StatusUpdates
        {
            get { return statusUpdates; }
        }

        //[HorizontalGroup("Status")]
        [ListDrawerSettings(AlwaysAddDefaultValue = true, Expanded = true)]

        [Sirenix.Serialization.OdinSerialize]
        //[SerializeField]
        List<StatusEffect> statusController = new List<StatusEffect>();
        public List<StatusEffect> StatusController
        {
            get { return statusController; }
        }

        [Title("Drawing")]
        [SerializeField]
        bool hideStatus = true;
        public bool HideStatus
        {
            get { return hideStatus; }
        }

        [SerializeField]
        bool statusShowLabel = false;
        public bool StatusShowLabel
        {
            get { return statusShowLabel; }
        }

        [ShowIf("statusShowLabel")]
        [SerializeField]
        string statusConditionLabel;
        public string StatusConditionLabel
        {
            get { return statusConditionLabel; }
        }

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


        #endregion

    } 

} // #PROJECTNAME# namespace