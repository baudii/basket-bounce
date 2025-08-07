using BasketBounce.Systems;
using KK.Common.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace BasketBounce.UI
{
    public class UI_VolumeSwitcher : Switcher
    {
        [SerializeField] Slider slider;
        [SerializeField] GameSettings gameSettings;
        [SerializeField] Sprite volumeOffSprite;
        [SerializeField] bool fx;
        Image volumeSwitchButton;
        Sprite volumeOnSprite;

        string fxKey = "fx-vol-cached";
        string musicKey = "music-vol-cached";
        private void Start()
        {
            volumeSwitchButton = GetComponent<Image>();
            volumeOnSprite = volumeSwitchButton.sprite;
        }

        public override void Activation()
        {
            if (IsActivated)
            {
                slider.interactable = false;
                volumeSwitchButton.sprite = volumeOffSprite;
                if (fx)
                {
                    PlayerPrefs.SetFloat(fxKey, slider.value);
                    gameSettings.FxVolume = 0;
                }
                else
                {
                    PlayerPrefs.SetFloat(musicKey, slider.value);
                    gameSettings.MusicVolume = 0;
                }
                slider.value = 0;
            }
            else
            {
                slider.interactable = true;
                volumeSwitchButton.sprite = volumeOnSprite;
                if (fx)
                {
                    slider.value = PlayerPrefs.GetFloat(fxKey, 0.5f);
                    gameSettings.FxVolume = slider.value;
                }
                else
                {
                    slider.value = PlayerPrefs.GetFloat(musicKey, 0.5f);
                    gameSettings.MusicVolume = slider.value;
                }
            }
        }
    }
}
