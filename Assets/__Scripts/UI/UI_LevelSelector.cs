using System.Collections.Generic;
using UnityEngine;
using BasketBounce.Gameplay.Levels;
using KK.Common;
using BasketBounce.Systems;

namespace BasketBounce.UI
{
	public class UI_LevelSelector : MonoBehaviour, IInitializable
	{
		[SerializeField] UI_LevelIcon levelIconPrefab;
		[SerializeField] Transform gridParent;

		List<UI_LevelIcon> levels;

		int levelAmount;
		public int MaxTotalStars;

		LevelManager levelManager;

        public void Init()
		{
			DIContainer.GetDependency(out levelManager);
			levelManager.OnLevelSetAvailable.AddListener(Setup);
		}

		public void Setup(LevelSet levelSet)
		{
			if (levels != null && levels.Count > 0)
			{
				foreach (var level in levels)
				{
					Destroy(level.gameObject);
				}
			}
			levelAmount = levelSet.LevelCount;
			levels = new List<UI_LevelIcon>();
			for (int i = 0; i < levelAmount; i++)
			{
				var levelIcon = Instantiate(levelIconPrefab, gridParent);
				levels.Add(levelIcon);
			}
		}

		public int GetTotalEarnedStars()
		{
			int totalStars = 0;

			for (int levelSetIdx = 0; levelSetIdx < levelManager.levelSetPrefabs.Count; levelSetIdx++)
			{
                for (int i = 0; i < levelManager.levelSetPrefabs[levelSetIdx].GetComponent<LevelSet>().LevelCount; i++)
                {
                    var key = LevelManager.GetEarnedStarsKey(i, levelSetIdx);
                    totalStars += PlayerPrefs.GetInt(key, 0);
					MaxTotalStars += 3;
                }
            }

			return totalStars;
		}

		public void UpdateLevelSelector()
		{
			int currentLevel = levelManager.CurrentLevel;

            var lastDiscoveredKey = LevelManager.GetLastDiscoveredLevelKey(levelManager.LevelSetId);

            int lastLevel = PlayerPrefs.GetInt(lastDiscoveredKey, 0);

            for (int i = 0; i < levels.Count; i++)
            {
                var earnedStarsKey = LevelManager.GetEarnedStarsKey(i, levelManager.LevelSetId);
                int stars = PlayerPrefs.GetInt(earnedStarsKey, 0);

                levels[i].UpdateCell(stars, i, i <= lastLevel, currentLevel == i);
            }
        }

		public void Submit()
		{
			int selectedLevel = UI_LevelIcon.SelectedLevel;
			this.Log("Loading level:", selectedLevel);

			if (selectedLevel == -1)
				return;

			levelManager.LoadLevelAsync(selectedLevel).SafeExectute();
		}
	}
}