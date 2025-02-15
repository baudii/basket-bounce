using UnityEngine;


namespace KK.Common.Gameplay
{
	public abstract class Switcher : MonoBehaviour
	{
		bool isActivated;
		public bool IsActivated
		{
			get { return isActivated; }
		}

		public void Toggle()
		{
			SetActivation(!isActivated);
		}

		public void SetActivation(bool value)
		{
			if (value == isActivated)
				return;

			isActivated = value;
			Activation();
		}

		public void Enable() => SetActivation(true);
		public void Disable() => SetActivation(false);

		protected void Deactivate()
		{
			isActivated = false;
		}

		public abstract void Activation();
	}
}