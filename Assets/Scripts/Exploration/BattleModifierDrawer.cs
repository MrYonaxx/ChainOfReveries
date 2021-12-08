using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class BattleModifierDrawer : MonoBehaviour
    {
        [SerializeField]
        public RectTransform RectTransform = null;
        [SerializeField]
        Image imageBM = null;
        [SerializeField]
        TextMeshProUGUI textLabel = null;

        float originalSize = 0;

        private void Awake()
        {
            originalSize = RectTransform.sizeDelta.x;
        }

        public void DrawBattleModifiers(string bmName, Color c, int nbTurn)
        {
            textLabel.text = bmName;
            imageBM.color = c;
            RectTransform.sizeDelta = new Vector2(nbTurn * (originalSize + 5), RectTransform.sizeDelta.y);
        }
    }
}
