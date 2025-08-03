using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BasketBounce.Systems;
using KK.Common;
namespace BasketBounce.UI
{
	public class UI_LevelNameController : MonoBehaviour, IInitializable
	{
		[SerializeField] MaskableGraphic target;
		[SerializeField] float targetAlpha;
		[SerializeField] float duration;
		[SerializeField] float delay;

		[Header("Level text animation")]
		[SerializeField] RectTransform textRect;
		[SerializeField] TextMeshProUGUI levelSetTextField;
		[SerializeField] TextMeshProUGUI levelTextField;
		[SerializeField] TextMeshProUGUI headerTextField;
		[SerializeField] float textAnimDuration;
		[SerializeField, Tooltip("Excluding level text duration")] float delayBetweenAppear, delayToDissapear;
		float initialAlpha;
		// float initialTextXPos; - old

		GestureDetector gestureDetector;
		bool isShowingLevelSetName;
		public void Init()
		{
			// initialTextXPos = textRect.localPosition.x; - old
			DIContainer.GetDependency(out gestureDetector);
			levelTextField.DOFade(0, 0);
			headerTextField.DOFade(0, 0);
			levelSetTextField.DOFade(0, 0);
            initialAlpha = target.color.a;
			gestureDetector.OnDragStart += SlowStop;
		}

		private void OnDestroy()
		{
			if (gestureDetector != null)
			{
				gestureDetector.OnDragStart -= SlowStop;
			}
		}

		public void ShowLevelSetName(string name)
		{
			isShowingLevelSetName = true;
			levelSetTextField.text = name;
        }

        public void StartAnimation(string header, int level)
		{
			levelTextField.text = "Level " + level.ToString();
			headerTextField.text = header;

			// Appear
			// textRect.DOLocalMoveX(0, duration).SetUpdate(true); Previous animation (text was moving from right)
			int mpl = 0;
			if (isShowingLevelSetName)
			{
                levelSetTextField.DOFade(1, textAnimDuration).SetUpdate(true);
				mpl = 1;
                isShowingLevelSetName = false;
                levelSetTextField.DOFade(0, duration).SetDelay(delay - delayBetweenAppear + textAnimDuration).SetUpdate(true);
            }
			levelTextField.DOFade(1, textAnimDuration).SetDelay(mpl * delayBetweenAppear).SetUpdate(true);
			headerTextField.DOFade(1, textAnimDuration).SetDelay(delayBetweenAppear + mpl * delayBetweenAppear).SetUpdate(true);
			target.DOFade(targetAlpha, duration).SetUpdate(true);

			// Dissapear
			// textRect.DOLocalMoveX(initialTextXPos, duration).SetDelay(duration + delay).SetUpdate(true); Previous animation dissapear
			levelTextField.DOFade(0, duration).SetDelay(delay - delayBetweenAppear + textAnimDuration).SetUpdate(true);
			headerTextField.DOFade(0, duration).SetDelay(delay - delayBetweenAppear + textAnimDuration).SetUpdate(true);


            target.DOFade(initialAlpha, duration)
				.SetDelay(delay + duration)
				.SetUpdate(true)
				.OnComplete(() => gameObject.SetActive(false));
		}

		public void SlowStop(Vector2 _)
		{
			if (isActiveAndEnabled)
			{
				target.DOKill(false);
				levelTextField.DOKill(false);
				headerTextField.DOKill(false);

				// Old animation
				// textRect.DOKill(false);
				// textRect.DOLocalMoveX(initialTextXPos, duration).SetUpdate(true);

				levelTextField.DOFade(0, duration).SetUpdate(true);
				headerTextField.DOFade(0, duration).SetUpdate(true);

				target.DOFade(initialAlpha, duration)
					.SetUpdate(true)
					.OnComplete(() =>
					{
						gameObject.SetActive(false);
						DOTween.Kill(textRect);
					});
			}
		}
	}
}