using BasketBounce.Gameplay.Levels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BasketBounce.UI
{
    public class UI_LevelSelectMainMenu : MonoBehaviour
    {
        [SerializeField] UI_LevelIcon levelButtonPrefab;
        [SerializeField] LevelSet levelSet;

        private void Start()
        {
            int levelCount = levelSet.Chunks.Sum(x => x.LevelCount);
            for (int i = 0; i < levelCount; i++)
            {
                Instantiate(levelButtonPrefab, transform);
            }
        }
    }
}
