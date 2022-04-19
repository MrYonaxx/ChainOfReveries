using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VoiceActing
{
	public class TutorialPanelDrawer : MonoBehaviour
	{
		[SerializeField]
		GameData gameData = null;
		[SerializeField]
		SpriteDictionary buttonSpriteDictionary = null;
		[SerializeField]
		SpriteDictionary keyboardSpriteDictionary = null;

		[SerializeField]
		Image inputButton = null;
		[SerializeField]
		TextMeshProUGUI textMain = null;
		[SerializeField]
		Image[] imageSleight = null;

		[SerializeField]
		Animator animator;


		public void DrawButton(bool show, int input)
		{
			inputButton.enabled = show;

			if(show)
            {
				InputConfig config = gameData.GetInputConfig(0);
				int id = config.GetInput((InputEnum)input);

				if (GameSettings.Keyboard)
					inputButton.sprite = keyboardSpriteDictionary.GetSprite(id);
				else
					inputButton.sprite = buttonSpriteDictionary.GetSprite(id);
			}
		
		}

		public void DrawItem(string text)
		{
			textMain.text = text;
		}

		public void DrawSleight(SleightData sleight)
		{
			if (sleight == null)
			{
				for (int i = 0; i < imageSleight.Length; i++)
				{
					imageSleight[i].enabled = false;
				}
			}
			else
			{
				for (int i = 0; i < sleight.SleightRecipe.Length; i++)
				{
					imageSleight[i].enabled = true;
					imageSleight[i].sprite = sleight.SleightRecipe[i].CardSprite;
				}
			}
		}

		public void ValidateButton()
		{
			animator.SetTrigger("Validate");
		}

		public void ResetButton()
		{
			animator.SetTrigger("Reset");
		}
		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}
