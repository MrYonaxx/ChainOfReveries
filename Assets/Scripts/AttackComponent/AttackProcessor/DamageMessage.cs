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
    public class DamageMessage
    {
        public float damage = 0;

        // Si l'attaque knockback, si c'est supérieur a 0 ça knockback
        public int knockback = 0;

        // Si l'attaque est magique ou physique (ou autre)
        public int attackType = -1;

        // liste élément de l'attaque
        public List<int> elements = new List<int>();

        // List status effect
        public List<StatusEffectData> statusEffects = new List<StatusEffectData>();
        public List<int> statusEffectsChance = new List<int>();

        // Je sais pas, au cas où
        public Dictionary<string, float> customMessages; 

        public DamageMessage()
        {
            damage = 0;
            knockback = 1;
            attackType = -1;
            elements = new List<int>();

            statusEffects = new List<StatusEffectData>();
            statusEffectsChance = new List<int>();

            customMessages = new Dictionary<string, float>();
        }


    } 

} // #PROJECTNAME# namespace