using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VoiceActing;


namespace Menu
{
	//Version avancé du MenuList
    public class MenuListCursor : MenuList, IControllable
    {
		[SerializeField]
		protected MenuCursor cursor = null;
		[SerializeField]
        protected Animator animatorMenu = null;




		public override void InitializeMenu()
		{
			base.InitializeMenu();
			ShowMenu(true);
		}

		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);
			cursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
		}

		protected override void QuitMenu()
		{
			base.QuitMenu();
			ShowMenu(false);
		}


		public void ShowMenu(bool b)
		{
			animatorMenu.gameObject.SetActive(true);
			animatorMenu.SetBool("Appear", b);
		}
	}
}
