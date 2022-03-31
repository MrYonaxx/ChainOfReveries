using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public enum BattleModifierTargets
    {
        Player,
        Enemies,
        Both
    }

    [System.Serializable]
    public class BattleModifiers
    {

        public string label = "";

        [HideLabel]
        public BattleModifierTargets battleModifierTargets = 0;

        [SerializeField]
        [HideLabel]
        [HorizontalGroup]
        public StatusEffectData statusEffect = null;

        [SerializeField]
        [HideLabel]
        [HorizontalGroup(Width = 100)]
        public int nb = 0;

        


        public BattleModifiers(StatusEffectData statusEffect, int nb)
        {
            this.statusEffect = statusEffect;
            this.nb = nb;
        }

        public BattleModifiers(StatusEffectData statusEffect, int nb, string label, BattleModifierTargets target)
        {
            this.statusEffect = statusEffect;
            this.nb = nb;
            this.label = label;
            this.battleModifierTargets = target;
        }
    }
}
