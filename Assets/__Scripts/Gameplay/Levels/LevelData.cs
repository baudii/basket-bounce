using UnityEngine;
using UnityEngine.Events;
using KK.Common;
using BasketBounce.Models;
using BasketBounce.Systems;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEditor;

namespace BasketBounce.Gameplay.Levels
{
	public class LevelData : MonoBehaviour
	{
		[SerializeField] string levelHeader;
		[SerializeField] string russianLevelHeader;

		[SerializeField] int bounces3star;
		[SerializeField] int bounces2star;
		[SerializeField] bool useThreshHold;
		[SerializeField] bool canNotStuck;
		[SerializeField] float overrideStuckTime;
		[SerializeField] public UnityEvent OnBallReleasedEvent;
		[SerializeField] public UnityEvent OnFirstTimeLoadEvemt;
		[SerializeField] public UnityEvent OnClickAnywhereEvent;
		[SerializeField] public UnityEvent OnResetEvent;

		ResetableManager resetableManager; 
		Transform finTransform;
		public bool UseThreshold => useThreshHold;
		public bool CanNotStuck => canNotStuck;
		public float OverrideStuckTime => overrideStuckTime;
		public string LevelHeader => levelHeader;

		public int LevelNum;

		private static float? _screenHeight;
		private static float ScreenHeight
		{
			get
			{
				if (_screenHeight == null)
					_screenHeight = Screen.height;
				return (float)_screenHeight;
			}
		}

		private static Camera _cam;
		private static Camera Cam
		{
			get
			{
				if (_cam == null)
					_cam = Camera.main;
				return _cam;
			}
		}

#if UNITY_EDITOR
		[Header("Editor Only")]
		[SerializeField] bool validateLevel;
		private void OnValidate()
		{
			if (validateLevel)
			{
				ValidateLevel();
			}
		}

#endif
		public void ValidateLevel()
		{
			transform.ForEachDescendant(child =>
			{
				if (child.TryGetComponent(out ILevelValidator levelValidator))
				{
					levelValidator.Validate();
				}
			});
		}

		public bool IsFinishInScreen()
		{
			Vector3 screenPos = Cam.WorldToScreenPoint(finTransform.position);
			if (screenPos.y <= ScreenHeight)
			{
				return true;
			}
			return false;
		}

		public void Init(LevelManager levelManager, int levelNum)
		{
			this.Log($"Initializing Level Data of level {levelNum}");
			resetableManager = new ResetableManager(transform);
			resetableManager.ResetAll();
			LevelNum = levelNum;

			transform.ForEach(child =>
			{
				if (child.TryGetComponent(out ILevelInitializer levelValidator))
				{
					finTransform = child;
					levelValidator.Initialize(levelManager);
					return true;
				}
				return false;
			});

			if (finTransform == null)
				throw new InvalidDataException($"Could not locate Finish transform. Maybe it doesn't exist in {levelNum}. LevelSetId:{levelManager.LevelSetId}");
		}

		public void OnFirstTimeLoad()
		{
			OnFirstTimeLoadEvemt?.Invoke();
		}

		public void OnBallReleased()
		{
			OnBallReleasedEvent?.Invoke();
		}

		public void OnLevelUnload()
		{
			ResetLevel();
		}

		public void ResetLevel()
		{
			resetableManager.ResetAll();
			OnResetEvent?.Invoke();
		}

		public Vector3 GetFinPos()
		{
			return finTransform.localPosition;
		}
		public ScoreData ConvertToStars(int bounces)
		{
			if (bounces2star == 0)
				bounces2star = bounces3star + 3;

			if (bounces <= bounces3star)
				return new ScoreData(3, -1, bounces);
			if (bounces <= bounces2star)
				return new ScoreData(2, bounces3star, bounces);
			return new ScoreData(1, bounces2star, bounces);
		}
	}
}