using UnityEngine;
using UnityEngine.Audio;

namespace BasketBounce.Systems
{
    [CreateAssetMenu(fileName = "AudioMixerController", menuName = "Settings/AudioMixerController")]
    public class AudioMixerSettingsSO : ScriptableObject
    {
        [SerializeField] AudioMixer audioMixer;
        
        public void SetConvertFrom01(string key, float volume)
        {
            audioMixer.SetFloat(key, Mathf.Log10(volume) * 20);
        }
    }
}
