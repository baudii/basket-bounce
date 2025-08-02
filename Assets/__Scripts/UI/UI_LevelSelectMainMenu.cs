using BasketBounce.Gameplay.Levels;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BasketBounce.UI
{
    public class UI_LevelSelectMainMenu : MonoBehaviour
    {
        [SerializeField] UI_LevelIcon levelButtonPrefab;
        [SerializeField] LevelSet levelSet;

        public void Init(UI_MainMenuPlayButton playButton)
        {
            int levelCount = levelSet.Chunks.Sum(x => x.LevelCount);
            var lastDiscoveredKey = LevelManager.GetLastDiscoveredLevelKey(levelSet.LevelSetId);
            int lastLevel = PlayerPrefs.GetInt(lastDiscoveredKey, 0);
            for (int i = 0; i < levelCount; i++)
            {
                var levelIcon = Instantiate(levelButtonPrefab, transform);
                var earnedStarsKey = LevelManager.GetEarnedStarsKey(i, levelSet.LevelSetId);
                int stars = PlayerPrefs.GetInt(earnedStarsKey, 0);

                levelIcon.UpdateCell(stars, i, i <= lastLevel, false);
                var button = levelIcon.GetComponent<Button>();
                int level = i;
                button.onClick.AddListener(() => 
                {
                    playButton.CacheLevel(levelSet.LevelSetId, level);
                });
            }
        }
    }
}
