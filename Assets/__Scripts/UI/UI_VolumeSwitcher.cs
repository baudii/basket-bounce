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
        private void Awake()
        {
            volumeSwitchButton = GetComponent<Image>();
            volumeOnSprite = volumeSwitchButton.sprite;
        }

        public void OnValueChanged(float value)
        {
            if (value > slider.minValue)
            {
                volumeSwitchButton.sprite = volumeOnSprite;
            }
            else
            {
                volumeSwitchButton.sprite = volumeOffSprite;
            }
        }

        public override void Activation()
        {
            if (IsActivated)
            {
                slider.interactable = false;
                OnValueChanged(slider.value);
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
                slider.value = slider.minValue;
            }
            else
            {
                slider.interactable = true;
                OnValueChanged(slider.value);
                if (fx)
                {
                    slider.value = PlayerPrefs.GetFloat(fxKey, 1);
                    gameSettings.FxVolume = slider.value;
                }
                else
                {
                    slider.value = PlayerPrefs.GetFloat(musicKey, 1);
                    gameSettings.MusicVolume = slider.value;
                }
            }
        }
    }
}
