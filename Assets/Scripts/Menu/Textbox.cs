using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using VoiceActing;

namespace Menu
{
	public class Textbox : MonoBehaviour, IControllable
	{
		[SerializeField]
		TextMeshProUGUI textBox;

		[SerializeField]
		[HideLabel]
		[TextArea(2, 3)]
		string text = "";

		[Title("Time")]
		[SerializeField]
		[SuffixLabel("en frames")]
		float timeInterval = 1;

		[SerializeField]
		[SuffixLabel("en frames")]
		float timePause = 12;

		[Title("Feedbacks")]
		[SerializeField]
		Animator textboxAnimator;

		bool show = true;
		float t = 0f;

		public event UnityAction OnTextEnd;

		public bool isTextFinished()
		{
			return textBox.maxVisibleCharacters >= text.Length;
		}

		public char CharacterDrawed()
		{
			return text[textBox.maxVisibleCharacters];
		}

		private void Awake()
		{
			timeInterval /= 60f;
			timePause /= 60f;

			textBox.text = text;
			textBox.maxVisibleCharacters = 0;

		}


		public void DrawTextbox(string text)
		{
			this.text = text;
			textBox.text = text;
			textBox.maxVisibleCharacters = 0;
			if (timeInterval < 0)
				textBox.maxVisibleCharacters = text.Length;
			show = true;
			DrawAnimator();
		}


		public void UpdateControl(InputController input)
		{
			if (show == false)
				return;

			if (textBox.maxVisibleCharacters >= text.Length)
			{
				if (input.InputA.Registered)
				{
					input.ResetAllBuffer();
					EndTextbox();
				}
				return;
			}

			if (input.InputA.Registered)
			{
				input.ResetAllBuffer();
				textBox.maxVisibleCharacters = text.Length;
			}

		}

        private void Update()
        {
			if (show == false)
				return;

			t += Time.deltaTime;
			if (t > timeInterval)
			{
				textBox.maxVisibleCharacters += 1;
				t = 0;
				if (textBox.maxVisibleCharacters > 0 && textBox.maxVisibleCharacters < text.Length)
				{
					if (text[textBox.maxVisibleCharacters - 1] == ',' && text[textBox.maxVisibleCharacters] == ' ' ||
						text[textBox.maxVisibleCharacters - 1] == '.' && text[textBox.maxVisibleCharacters] == ' ' ||
						text[textBox.maxVisibleCharacters - 1] == '?' && text[textBox.maxVisibleCharacters] == ' ' ||
						text[textBox.maxVisibleCharacters - 1] == '!' && text[textBox.maxVisibleCharacters] == ' ')
						t -= timePause;
				}
			}
		}



        private void EndTextbox()
		{
			HideTextbox();
			OnTextEnd.Invoke();
		}

		private void DrawAnimator()
		{
			if (textboxAnimator != null)
			{
				if(show)
					textboxAnimator.gameObject.SetActive(true);

				textboxAnimator.SetBool("Appear", show);
			}
		}

		public void HideTextbox()
        {
			if (show)
			{
				show = false;
				DrawAnimator();
			}
		}
	}
}
