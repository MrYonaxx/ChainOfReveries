using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VoiceActing;

// Au cas où on a besoin d'une classe parent
namespace Menu
{
    public class MenuBase : MonoBehaviour, IControllable
    {
		[SerializeField]
		protected bool initializeOnStart = true;

		public delegate void EventVoid();
		public delegate void EventInt(int i);
		public event EventVoid OnStart;
		public event EventVoid OnEnd;

		private void Start()
		{
			if(initializeOnStart == true)
				InitializeMenu();
		}



		public virtual void UpdateControl(InputController input)
		{
			if (input.InputB.Registered)
			{
				input.InputB.ResetBuffer();
				QuitMenu();
			}
		}


		public virtual void InitializeMenu()
		{
			OnStart?.Invoke();
		}

		protected virtual void QuitMenu()
		{
			OnEnd?.Invoke();
		}
	}
}
