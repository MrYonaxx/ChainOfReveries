using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;

namespace Menu
{
	public class MenuSleightDrawer : MenuList, IControllable
	{
		[SerializeField]
		CardType cardTypeData = null;

		[Title("Parameters")]
		[SerializeField]
		ButtonSleight sleightPrefab = null;

		[Title("Cursor")]
		[SerializeField]
		public MenuCursor menuCursor = null;

		List<ButtonSleight> sleightList = new List<ButtonSleight>();

		public void DrawSleight(SleightData[] sleights)
		{
			for (int i = 0; i < sleights.Length; i++)
			{
				if (sleightList.Count <= i)
				{
					sleightList.Add(Instantiate(sleightPrefab, listEntry.ListTransform));
				}
				sleightList[i].gameObject.SetActive(true);
				sleightList[i].DrawSleight(sleights[i], cardTypeData);

				// À optimiser pour que les liste puissent fonctionner sans forcément instancier 
				listEntry.DrawItemList(i, "");
				listEntry.ListItem[i].gameObject.SetActive(false);
			}
		}

		public override void UpdateControl(InputController input)
		{
			if (listEntry.InputListVertical(input.InputLeftStickY.InputValue)) // On s'est déplacé dans la liste
			{
				SelectEntry(listEntry.IndexSelection);
			}
			else if (input.InputA.Registered)
			{
				input.InputA.ResetBuffer();
				ValidateEntry(listEntry.IndexSelection);
			}
			else if (input.InputB.Registered || (GameSettings.Keyboard && input.InputY.Registered))
			{
				input.InputB.ResetBuffer();
				QuitMenu();
			}
		}

		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);
			menuCursor.MoveCursor(new Vector2(sleightList[id].GetRectTransform().anchoredPosition.x, sleightList[id].GetRectTransform().anchoredPosition.y));

		}
	}
}