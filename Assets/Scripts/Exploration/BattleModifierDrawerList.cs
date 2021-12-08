using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class BattleModifierDrawerList : MonoBehaviour
    {
        [SerializeField]
        RectTransform transformDeck = null;
        [SerializeField]
        RectTransform transformBattleModifier = null;

        [SerializeField]
        BattleModifierDrawer battleModifierDrawerPrefab =  null;

        List<BattleModifierDrawer> listBattleModifiers = new List<BattleModifierDrawer>();
        int maxIndex = 0;


        public void DrawBattleModifiers(List<BattleModifiers> battleModifiers, int indexFloor)
        {
            transformBattleModifier.sizeDelta = new Vector2(transformDeck.rect.width - ((143.5f+5) * indexFloor), transformBattleModifier.sizeDelta.y);
            maxIndex = battleModifiers.Count;

            // Dessine les battle Modifiers
            for (int i = 0; i < battleModifiers.Count; i++)
            {
                DrawBattleModifier(battleModifiers[i], i);
            }

            // On désaffiche les battleModifiers en trop
            for (int i = battleModifiers.Count; i < listBattleModifiers.Count; i++)
            {
                listBattleModifiers[i].gameObject.SetActive(false);
            }
        }

        private void DrawBattleModifier(BattleModifiers battleModifier, int index)
        {
            if (listBattleModifiers.Count <= index)
            {
                listBattleModifiers.Add(Instantiate(battleModifierDrawerPrefab, transformBattleModifier));
            }
            listBattleModifiers[index].gameObject.SetActive(true);

            Color c = Color.blue;
            if (battleModifier.battleModifierTargets == BattleModifierTargets.Enemies)
                c = Color.red;
            else if (battleModifier.battleModifierTargets == BattleModifierTargets.Both)
                c = Color.red;

            listBattleModifiers[index].DrawBattleModifiers(battleModifier.label, c, battleModifier.nb);
        }


        public void DrawBattleModifiersPreview(List<BattleModifiers> battleModifiers)
        {
            for (int i = maxIndex; i < maxIndex + battleModifiers.Count; i++)
            {
                DrawBattleModifier(battleModifiers[i - maxIndex], i);
            }
        }

        public void HideBattleModifiersPreview(List<BattleModifiers> battleModifiers)
        {
            for (int i = maxIndex; i < maxIndex + battleModifiers.Count; i++)
            {
                if (i < listBattleModifiers.Count)
                {
                    listBattleModifiers[i].gameObject.SetActive(false);
                }
            }
        }

    }
}
