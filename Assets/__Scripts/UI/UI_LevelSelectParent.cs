using UnityEngine;
using UnityEngine.Events;

namespace BasketBounce.UI
{
    public class UI_LevelSelectParent : MonoBehaviour
    {
        [SerializeField] UI_MainMenuPlayButton mainMenuPlayButton;
        [SerializeField] UnityEvent OnDisableButtons;
        UI_LevelSelectMainMenu[] levelSelectors;
        private void Start()
        {
            levelSelectors = GetComponentsInChildren<UI_LevelSelectMainMenu>();
            foreach (var levelSelector in levelSelectors)
            {
                levelSelector.Init(mainMenuPlayButton);
            }
        }

        public void DisableButtons()
        {
            foreach (var levelSelector in levelSelectors)
            {
                levelSelector.DisableButtons();
            }
            OnDisableButtons?.Invoke();
        }
    }
}
