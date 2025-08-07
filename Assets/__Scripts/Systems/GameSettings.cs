using UnityEngine;
using UnityEngine.Audio;

namespace BasketBounce.Systems
{
	[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
    public class GameSettings : ScriptableObject
    {
		[SerializeField] bool autostartEnabled;
        [SerializeField] AudioMixerSettingsSO audioMixerSettings;

        const string musicKey = "Music-key";
        const string fxKey = "SFX-key";
        private float musicVol;
        private float fxVol;
        public float MusicVolume
        { 
            get => musicVol; 
            set
            {
                audioMixerSettings.SetConvertFrom01(musicKey, value);
                musicVol = value;
            } 
        }
        public float FxVolume
        {
            get => fxVol;
            set
            {
                audioMixerSettings.SetConvertFrom01(fxKey, value);
                fxVol = value;
            }
        }

        public bool AutoStartEnabled => autostartEnabled;

        public void Init()
        {
            MusicVolume = PlayerPrefs.GetFloat(musicKey, 1);
            FxVolume = PlayerPrefs.GetFloat(fxKey, 1);
        }

        public void SaveVolume()
        {
            PlayerPrefs.SetFloat(musicKey, MusicVolume);
            PlayerPrefs.SetFloat(fxKey, FxVolume);
        }
    }
}
