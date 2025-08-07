using UnityEngine;
using UnityEngine.Audio;

namespace BasketBounce.Systems
{
	[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
    public class GameSettings : ScriptableObject
    {
		[SerializeField] bool autostartEnabled;
        [SerializeField] AudioMixer audioMixer;

        const string musicKey = "Music_sound";
        const string fxKey = "FX_sound";
        bool isLoadedSound;
        public float MusicVolume { get; set; }
        public float FxVolume { get; set; }
        public bool AutoStartEnabled => autostartEnabled;

        public void Init()
        {
            if (isLoadedSound)
                return;

            MusicVolume = PlayerPrefs.GetFloat(musicKey);
            FxVolume = PlayerPrefs.GetFloat(fxKey);
        }

        public void SaveVolume()
        {
            PlayerPrefs.SetFloat(musicKey, MusicVolume);
            PlayerPrefs.SetFloat(fxKey, FxVolume);
        }
    }
}
