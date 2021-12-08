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
    [System.Serializable]
    public class AttackP_Element : AttackProcessor
    {
        [SerializeField]
        //[HideLabel]
        [ValueDropdown("SelectCardElement")]
        private int element;
        public int Element
        {
            get { return element; }
        }


        [SerializeField]
        bool ignoreElementalStat = false;

        // Calcule des dommages
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            // Pour pas faire le calcul 2 fois si pour x ou y raison y'a deux fois AttackP_Element avec le même élément
            if (damageMessage.elements.Contains(element))
                return;

            damageMessage.elements.Add(element);

            if (ignoreElementalStat == false)
                ElementalDefense(element, target, ref damageMessage);

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