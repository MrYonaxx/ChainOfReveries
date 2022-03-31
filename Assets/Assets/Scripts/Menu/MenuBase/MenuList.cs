using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VoiceActing;

// Au cas où on a besoin d'une classe parent
namespace Menu
{
	//jsp si ça hérite de MenuBase, est-ce que c'est pas plus confus ?
	// réponse du futur : ça aurait du hériter de MenuBase mais bon trop tard pour tout refactor
    public class MenuList : MonoBehaviour, IControllable
    {
		[SerializeField]
		protected bool initializeOnStart = true;
		[SerializeField]
        protected MenuButtonListController listEntry;

		public delegate void EventVoid();
		public delegate void EventInt(int i);
		public event EventVoid OnStart;
		public event EventInt OnSelected;
		public event EventInt OnValidate;
		public event EventVoid OnEnd;

		private void Start()
		{
			if(initializeOnStart == true)
				InitializeMenu();
		}



		public virtual void UpdateControl(InputController input)
		{
			if (listEntry.InputListVertical(input.InputLeftStickY.InputValue) == true) // On s'est déplacé dans la liste
			{
				SelectEntry(listEntry.IndexSelection);
			}
			else if (input.InputA.Registered)
			{
				input.InputA.ResetBuffer();
				ValidateEntry(listEntry.IndexSelection);
			}
			else if (input.InputB.Registered)
			{
				input.InputB.ResetBuffer();
				QuitMenu();
			}
		}


		public virtual void InitializeMenu()
		{
			OnStart?.Invoke();
		}

		protected virtual void SelectEntry(int id)
		{
			OnSelected?.Invoke(id);
		}

		protected virtual void ValidateEntry(int id)
		{
			OnValidate?.Invoke(id);
		}

		protected virtual void QuitMenu()
		{
			OnEnd?.Invoke();
		}
	}
}
