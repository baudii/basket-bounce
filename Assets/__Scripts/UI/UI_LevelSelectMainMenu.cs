using BasketBounce.Gameplay.Levels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BasketBounce.UI
{
    public class UI_LevelSelectMainMenu : MonoBehaviour
    {
        [SerializeField] Transform iconParent;
        [SerializeField] UI_LevelIcon iconPrefab;
        [SerializeField] LevelSet levelSet;

        List<UI_LevelIcon> levelIcons;

        public void Init(UI_MainMenuPlayButton playButton)
        {
            levelIcons = new List<UI_LevelIcon>();
            int levelCount = levelSet.Chunks.Sum(x => x.LevelCount);
            var lastDiscoveredKey = LevelManager.GetLastDiscoveredLevelKey(levelSet.LevelSetId);
            var lastLevelSetKey = LevelManager.GetLastLevelSetIdKey();
            int lastLevelSet = PlayerPrefs.GetInt(lastLevelSetKey, 0);
            int lastLevel = PlayerPrefs.GetInt(lastDiscoveredKey, 0);
            for (int i = 0; i < levelCount; i++)
            {
                var levelIcon = Instantiate(iconPrefab, iconParent);
                var earnedStarsKey = LevelManager.GetEarnedStarsKey(i, levelSet.LevelSetId);
                int stars = PlayerPrefs.GetInt(earnedStarsKey, 0);
                bool isOpen = levelSet.LevelSetId <= lastLevelSet - 1;
                if (!isOpen && levelSet.LevelSetId == lastLevelSet)
                {
                    isOpen = i <= lastLevel;
                }

                levelIcon.UpdateCell(stars, i, isOpen, false);
                var button = levelIcon.GetComponent<Button>();
                int level = i;
                button.onClick.AddListener(() => 
                {
                    playButton.CacheLevel(levelSet.LevelSetId, level);
                });
                levelIcons.Add(levelIcon);
            }
        }

        public void DisableButtons()
        {
            var index = UI_LevelIcon.SelectedLevel;
            if (index < 0 || index >= levelIcons.Count)
            {
                return;
            }
            levelIcons[index].Deselect();
        }
    }
}
