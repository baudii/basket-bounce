using BasketBounce.Gameplay.Levels;
using BasketBounce.Systems;
using KK.Common;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BasketBounce.UI
{
	public class UI_MainMenuPlayButton : MonoBehaviour
	{
		[SerializeField] Button button;
		(int, int) levelCached;

		public void CacheLevel(int levelSet, int level)
		{
			levelCached = (levelSet, level);
			if (!button.interactable)
				button.interactable = true;
		}

		public void StartGame()
		{
			DIContainer.GetDependency(out GameManager gameManager);
			(var levelSet, var level) = levelCached;
			gameManager.SubmitLevel(levelSet, level).SafeExectute();
		}
	}
}
