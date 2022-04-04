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

namespace Stats
{

    public enum StatBonusType
    {
        Flat,
        Multiplier
    }


    [System.Serializable]
    public class Stat
    {
        [HorizontalGroup("Stat", Width = 0.4f)]
        [HideLabel]
        [SerializeField]
        [OnValueChanged("CalculateFinalValue")]
        protected float baseValue = 0;
        public float BaseValue
        {
            get { return baseValue; }
            set { baseValue = value; CalculateFinalValue(); }
        }

        [HorizontalGroup("Stat", Width = 0.4f)]
        [SerializeField]
        [HideLabel]
        [ReadOnly]
        protected float finalValue;
        public float Value
        {
            get { return finalValue; }
        }

        protected float flatBonus = 0;
        protected float multiplierBonus = 1;

        public Stat()
        {
            baseValue = 0;
            flatBonus = 0;
            multiplierBonus = 1;
        }

        public Stat(float baseV)
        {
            baseValue = baseV;
            flatBonus = 0;
            multiplierBonus = 1;
            CalculateFinalValue();
        }


        public void AddStatBonus(float bonus, StatBonusType bonusType)
        {
            if(bonusType == StatBonusType.Flat)
                flatBonus += bonus;
            else if (bonusType == StatBonusType.Multiplier)
                multiplierBonus += bonus;

            CalculateFinalValue();
        }

        protected void CalculateFinalValue()
        {
            finalValue = (baseValue + flatBonus) * multiplierBonus;
        }



    } 

} // #PROJECTNAME# namespace