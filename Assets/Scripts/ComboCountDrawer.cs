using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

//namespace CoR.UI

namespace VoiceActing
{
    public class ComboCountDrawer : MonoBehaviour
    {
        [SerializeField]
        CharacterBase player = null;

        [Title("UI")]
        [SerializeField]
        TextMeshProUGUI textComboDigit = null;
        [SerializeField]
        CanvasGroup canvasCombo = null;
        [SerializeField]
        Feedbacks.GenericLerp lerpCombo = null;

        [SerializeField]
        CanvasGroup canvasCounter = null;
        [SerializeField]
        Feedbacks.GenericLerp lerpCounter = null;

        int comboCount = 0;
        CharacterBase characterComboed = null;

        // Start is called before the first frame update
        void Start()
        {
            player.CharacterAction.OnAttackHit += UpdateCombo;
        }

        void OnDestroy()
        {
            player.CharacterAction.OnAttackHit -= UpdateCombo;
            if (characterComboed != null)
            {
                characterComboed.OnStateChanged -= CharacterChangeState;
            }
        }

        void UpdateCombo(AttackController attack, CharacterBase character)
        {
            if (characterComboed == null)
            {
                characterComboed = character;
                characterComboed.OnStateChanged += CharacterChangeState;
            }

            if (characterComboed == character)
            {
                comboCount += 1;
                if (comboCount >= 2)
                    DrawCombo();
            }

            // Counter hit
            if(character.OldState.ID == CharacterStateID.Acting && comboCount <= 1)
            {
                lerpCounter.StartLerp(1, 1.5f, (start, t) => { canvasCounter.alpha = Mathf.Lerp(start, 0, t); });
            }

        }

        void DrawCombo()
        {
            if (comboCount == 2)
                lerpCombo.StartLerp(0, 0.4f, (start, t) => { canvasCombo.alpha = Mathf.Lerp(start, 1, t); });
            textComboDigit.text = comboCount.ToString();
        }

        void HideCombo()
        {
            lerpCombo.StartLerp(1, 3f, (start, t) => { canvasCombo.alpha = Mathf.Lerp(start, 0, t); });
        }

        void CharacterChangeState(CharacterState oldState, CharacterState newState)
        {
            if (newState.ID != CharacterStateID.Hit)
            {
                characterComboed.OnStateChanged -= CharacterChangeState;
                comboCount = 0;
                characterComboed = null;
                HideCombo();
            }
        }
    }
}
