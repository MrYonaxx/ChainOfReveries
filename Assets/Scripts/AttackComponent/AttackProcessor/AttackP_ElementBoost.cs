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
    public class AttackP_ElementBoost: AttackProcessor
    {
        [SerializeField]
        [ValueDropdown("SelectCardElement")]
        int element = 1;

        [SerializeField]
        float damageMultiplier = 1;


        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            for (int i = 0; i < damageMessage.elements.Count; i++)
            {
                if (element == damageMessage.elements[i])
                {
                    damageMessage.damage = damageMessage.damage * damageMultiplier;
                    return;
                }
            }
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