using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasketBounce.UI
{
    public class UI_LevelSelectParent : MonoBehaviour
    {
        [SerializeField] UI_MainMenuPlayButton mainMenuPlayButton;
        private void Start()
        {
            var levelSelectors = GetComponentsInChildren<UI_LevelSelectMainMenu>();
            foreach (var levelSelector in levelSelectors)
            {
                levelSelector.Init(mainMenuPlayButton);
            }
        }
    }
}
