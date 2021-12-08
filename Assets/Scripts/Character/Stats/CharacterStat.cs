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
using UnityEngine.Events;

namespace VoiceActing
{
    // Ajouter une stat à la fin pour pas casser l'ordre de l'enum des objets déjà existants
    // (C'est moi bien rangé du coup ;-;)
    public enum CharStatEnum
    {
        HPMax,
        Attack,
        Defense,
        Magic,
        MagicDefense,

        ReloadAmount,
        ReloadMax,
        CardBreakTime,
        CardBreakKnockback,

        KnockbackTime,

        Speed,
        Gravity,
        GravityMax,
        Mass,

        MotionSpeed,

        RevengeValue
    }

    [System.Serializable]
    public class CharacterStat
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [HideLabel]
        [ProgressBar(0, "HpMax", Height = 20)]
        [Title("HP")]
        [SerializeField]
        private float hp = 1;
        public float HP
        {
            get { return hp; }
            set { hp = Mathf.Clamp(value, 0, HPMax.Value); OnHPChanged?.Invoke(hp, HPMax.Value); }
        }


        private float HpMax = 1;
        private void SetHpMax()
        {
            HpMax = HPMax.BaseValue;
        }


        [HorizontalGroup("Stat", LabelWidth = 100, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Statistiques")]
        [VerticalGroup("Stat/Left")]
        [InlineProperty(LabelWidth = 100)]
        [OnValueChanged("SetHpMax", true)]
        [SerializeField]
        public Stat HPMax;

        [VerticalGroup("Stat/Left")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat Attack;

        [VerticalGroup("Stat/Left")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat Defense;

        [VerticalGroup("Stat/Left")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat Magic;

        [VerticalGroup("Stat/Left")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat MagicDefense;


        [HorizontalGroup("Stat", LabelWidth = 100, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Card")]
        [VerticalGroup("Stat/Center")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat RevengeValue;

        [VerticalGroup("Stat/Center")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat KnockbackTime;

        [VerticalGroup("Stat/Center")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat ReloadAmount;

        [VerticalGroup("Stat/Center")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat CardBreakTime;

        [VerticalGroup("Stat/Center")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat CardBreakKnockback;


        [HorizontalGroup("Stat", LabelWidth = 100, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Character")]
        [VerticalGroup("Stat/Right")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat Speed;

        [VerticalGroup("Stat/Right")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat Gravity;

        [VerticalGroup("Stat/Right")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat GravityMax;

        [VerticalGroup("Stat/Right")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat MotionSpeed;

        [VerticalGroup("Stat/Right")]
        [InlineProperty(LabelWidth = 100)]
        [SerializeField]
        public Stat Mass;




        [Title("Resistances")]
        [HorizontalGroup("CharacterResistance")]
        [SerializeField]
        public List<ElementalResistance> ElementalResistances = new List<ElementalResistance>();

        [Title(" ")]
        [HorizontalGroup("CharacterResistance")]
        [SerializeField]
        public List<StatusResistance> StatusResistances = new List<StatusResistance>();


        public delegate void ActionDoubleFloat(float a, float b);
        public event ActionDoubleFloat OnHPChanged;

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
        public CharacterStat()
        {

        }


        public CharacterStat(CharacterStat characterStat)
        {
            HPMax = new Stat(characterStat.HPMax.Value);
            Attack = new Stat(characterStat.Attack.Value);
            Defense = new Stat(characterStat.Defense.Value);
            Magic = new Stat(characterStat.Magic.Value);
            MagicDefense = new Stat(characterStat.MagicDefense.Value);

            RevengeValue = new Stat(characterStat.RevengeValue.Value);
            KnockbackTime = new Stat(characterStat.KnockbackTime.Value);
            ReloadAmount = new Stat(characterStat.ReloadAmount.Value);
            CardBreakTime = new Stat(characterStat.CardBreakTime.Value);
            CardBreakKnockback = new Stat(characterStat.CardBreakKnockback.Value);

            Speed = new Stat(characterStat.Speed.Value);
            Gravity = new Stat(characterStat.Gravity.Value);
            GravityMax = new Stat(characterStat.GravityMax.Value);
            MotionSpeed = new Stat(characterStat.MotionSpeed.Value);
            Mass = new Stat(characterStat.Mass.Value);


            for (int i = 0; i < characterStat.ElementalResistances.Count; i++)
            {
                ElementalResistances.Add(new ElementalResistance(characterStat.ElementalResistances[i].Element, characterStat.ElementalResistances[i].BaseValue));
            }

            HP = HPMax.Value;
        }


        public void AddStat(StatModifier stat)
        {
            GetStat(stat.CharStat).AddStatBonus(stat.Value, stat.ModifierType);
        }
        public void RemoveStat(StatModifier stat)
        {
            GetStat(stat.CharStat).AddStatBonus(-stat.Value, stat.ModifierType);
        }

        public Stat GetStat(CharStatEnum charStat)
        {
            switch (charStat)
            {
                case CharStatEnum.HPMax:
                    return HPMax;
                case CharStatEnum.Attack:
                    return Attack;
                case CharStatEnum.Defense:
                    return Defense;
                case CharStatEnum.Magic:
                    return Magic;
                case CharStatEnum.MagicDefense:
                    return MagicDefense;

                case CharStatEnum.ReloadAmount:
                    return ReloadAmount;
                case CharStatEnum.CardBreakTime:
                    return CardBreakTime;
                case CharStatEnum.CardBreakKnockback:
                    return CardBreakKnockback;

                case CharStatEnum.RevengeValue:
                    return RevengeValue;
                case CharStatEnum.KnockbackTime:
                    return KnockbackTime;

                case CharStatEnum.Speed:
                    return Speed;
                case CharStatEnum.Gravity:
                    return Gravity;
                case CharStatEnum.GravityMax:
                    return GravityMax;
                case CharStatEnum.Mass:
                    return Mass;

                case CharStatEnum.MotionSpeed:
                    return MotionSpeed;
            }

            return null;
        }

        public float GetElementalResistance(int element)
        {
            for (int i = 0; i < ElementalResistances.Count; i++)
            {
                if (element == ElementalResistances[i].Element)
                {
                    return ElementalResistances[i].Value;
                }
            }
            return 0;
        }

        public float GetStatusResistance(StatusEffectData status)
        {
            for (int i = 0; i < StatusResistances.Count; i++)
            {
                if (status == StatusResistances[i].StatusData)
                {
                    return (StatusResistances[i].Value);
                }
            }
            return 0;
        }



        #endregion

    } 

} // #PROJECTNAME# namespace