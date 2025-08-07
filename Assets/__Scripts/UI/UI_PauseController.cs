using BasketBounce.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace BasketBounce.UI
{
	public class UI_PauseController : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI header;
		[SerializeField] GameObject levelSelectGO;
		[SerializeField] GameSettings settings;
		[SerializeField] Slider musicSlider;
		[SerializeField] Slider sfxSlider;

        private void Start()
        {
			if (settings == null || musicSlider == null || sfxSlider == null)
				return;

			musicSlider.value = settings.MusicVolume;
            sfxSlider.value = settings.FxVolume;
        }

        public void InitStuck()
		{
			levelSelectGO.SetActive(false);
			header.text = "Stuck?";
		}

		public void InitPause()
		{
			levelSelectGO.SetActive(true);
			header.text = "Menu";
		}
	}
}