using KK.Common;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BasketBounce.Systems
{
	public abstract class SceneEntryPoint : MonoBehaviour
	{
		public static CancellationTokenSource Cts;
		public abstract Task Setup();
		public abstract Task Activate();

		protected void OnSetup()
		{
            this.Log($"Entry of {GetType().Name} (Setup)");
        }

		protected void OnActivate()
		{
			//this.Log($"Entry of {GetType().Name} (Activate)");
		}
	}
}
