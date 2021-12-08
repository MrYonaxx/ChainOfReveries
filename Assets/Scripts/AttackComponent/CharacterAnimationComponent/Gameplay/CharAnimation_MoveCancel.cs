using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_MoveCancel : CharacterAnimationEvent
    {
        [Space]
        [HorizontalGroup]
        [SerializeField]
        bool cancelItself = false; // peux annuler l'attaque si on rejoue la même action (utile pour les target combo)

        [Space]
        [HorizontalGroup]
        [SerializeField]
        bool cancelSpecial = true;

        // A opti en flag / maskfield
        [SerializeField]
        [ValueDropdown("SelectCardType")]
        [LabelWidth(100)]
        [ListDrawerSettings(Expanded = true)]
        int[] cancelType;
        public int[] CancelType
        {
            get { return cancelType; }
        }

        public override void Execute(CharacterBase character)
        {
            character.CharacterAction.MoveCancelable();
            character.CharacterAction.CanMoveCancelItself = cancelItself;
            character.CharacterAction.CanSpecialCancel = cancelSpecial;

            for (int i = 0; i < character.CharacterAction.CanMoveCancelType.Length; i++)
            {
                character.CharacterAction.CanMoveCancelType[i] = false;
            }
            for (int i = 0; i < cancelType.Length; i++)
            {
                character.CharacterAction.CanMoveCancelType[cancelType[i]] = true;
            }
        }


#if UNITY_EDITOR
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeID();
        }
#endif 

    }
}
