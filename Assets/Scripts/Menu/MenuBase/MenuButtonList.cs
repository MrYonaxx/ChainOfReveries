using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Menu
{
	public class MenuButtonList : MonoBehaviour
	{
		[SerializeField]
		Image imageButton;
		[SerializeField]
		TextMeshProUGUI mainText;

        [SerializeField]
		TextMeshProUGUI subText;

		[SerializeField]
		Animator animator;

		private RectTransform rectTransform;
		public RectTransform RectTransform
		{
			get { if (rectTransform == null) rectTransform = GetComponent<RectTransform>(); return rectTransform; } // A corriger quand j'aurai moins la flemme
		}
		public string Text
		{
			get { return mainText.text; }
		}


		public virtual void DrawButton(Sprite icon, string text, string text2)
		{
			if (imageButton != null)
			{
				if (icon == null)
					imageButton.enabled = false;
				else
					imageButton.enabled = true;
				imageButton.sprite = icon;
			}
			if(mainText != null)
				mainText.text = text;
			if(subText != null)
				subText.text = text2;
		}

		public void SetTextColor(Color c)
		{
			mainText.color = c;
		}
		public virtual void DrawSubText(string text2)
		{
			if(subText != null)
				subText.text = text2;
		}

		public virtual void SelectButton()
		{
			if(animator != null)
				animator.SetTrigger("Selected");
		}
		public virtual void UnselectButton()
		{
			if (animator != null)
				animator.SetTrigger("Unselected");
		}
	}
}
