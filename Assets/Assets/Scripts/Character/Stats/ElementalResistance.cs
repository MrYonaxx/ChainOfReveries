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
using Stats;

namespace VoiceActing
{
    [System.Serializable]
    public class ElementalResistance : Stat
    {
        [SerializeField]
        [HorizontalGroup("ElementalResistance")]
        [HideLabel]
        [ValueDropdown("SelectCardElement")]
        private int element;
        public int Element
        {
            get { return element; }
        }

         
        /*[HorizontalGroup("ElementalResistance", Width = 0.2f)]
        [HideLabel]
        [SerializeField]
        private float baseResistance = 0;



        [HorizontalGroup("ElementalResistance", Width = 0.2f)]
        [SerializeField]
        [HideLabel]
        [ReadOnly]
        private int finalResistance;
        public int Value
        {
            get { return finalResistance; }
            set { finalResistance = value; }
        }

        private float flatBonus = 0;
        private float multiplierBonus = 0;*/

        public ElementalResistance(int elementID, float baseV)
        {
            element = elementID;
            baseValue = baseV;
            flatBonus = 0;
            multiplierBonus = 1;
            CalculateFinalValue();
        }

#if UNITY_EDITOR
        private static IEnumerable SelectCardElement()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("ElementTypeData")[0]))
                .GetAllTypeID();
        }
#endif 
    }

} // #PROJECTNAME# namespace