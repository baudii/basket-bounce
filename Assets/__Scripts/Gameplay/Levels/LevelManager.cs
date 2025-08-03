using System;
using UnityEngine;
using UnityEngine.Events;
using BasketBounce.Systems;
using BasketBounce.Models;
using UnityEngine.AddressableAssets;
using KK.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BasketBounce.Gameplay.Levels
{
	public class LevelManager : MonoBehaviour
	{
		#region Unity Events

		[HideInInspector]
		public UnityEvent<LevelData> OnLevelSetupEvent;
		[HideInInspector]
		public UnityEvent<LevelData> OnLevelIsLoadedEvent;
		[HideInInspector]
		public UnityEvent OnFinishedGameEvent;
		[HideInInspector]
		public UnityEvent<ScoreData> OnFinishedLevelEvent;
		[HideInInspector]
		public UnityEvent<LevelSet> OnLevelSetAvailable;

		#endregion

		LevelSet currentLevelSet;
		public LevelData CurrentLevelData { get; private set; }
		public int CurrentLevel { get; private set; }

		GameManager gameManager;

		public int LevelSetId => currentLevelSet.LevelSetId;

		[HideInInspector]
		public IList<GameObject> levelSetPrefabs;

		public async Task Init(GameManager gameManager)
		{
			this.gameManager = gameManager;
			var op = Addressables.LoadAssetsAsync<GameObject>("LevelSets", null);
			await op.Task;
			levelSetPrefabs = op.Result;
        }

		void OnDestroy()
		{
			OnLevelSetupEvent.RemoveAllListeners();
		}


		public async Task ActivateLevelSet(int levelSet, int? level = null)
		{
			destroyCancellationToken.ThrowIfCancellationRequested();

			if (levelSetPrefabs == null)
				throw new ArgumentNullException(nameof(levelSetPrefabs), $"Level set prefabs collections is null. It should be initialized.");

			if (levelSet < 0 || levelSet >= levelSetPrefabs.Count)
				throw new ArgumentOutOfRangeException(nameof(levelSet), $"Provided levelSet={levelSet} is invalid. Should be at least between [0, {levelSetPrefabs.Count - 1}] inclusive");
			
			var levelSetPrefab = levelSetPrefabs[levelSet];
			var levelSetGo = Instantiate(levelSetPrefab, Vector3.zero, Quaternion.identity, transform);
			currentLevelSet = levelSetGo.GetComponent<LevelSet>();
			OnLevelSetAvailable?.Invoke(currentLevelSet);

			if (level == null)
			{
				var key = GetLastDiscoveredLevelKey(currentLevelSet.LevelSetId);
				var lastLevel = PlayerPrefs.GetInt(key, 0);
				await ActivateLevel(lastLevel);
			}
			else
			{
				await ActivateLevel(level.Value);
			}
		}

		public Task ActivateLevel(int level)
		{
			destroyCancellationToken.ThrowIfCancellationRequested();
			CurrentLevel = level;
			currentLevelSet.InitChunk(CurrentLevel);
			return LoadLevelAsync(CurrentLevel);
		}

		void SwapLevelTo(int level)
		{
			this.Log($"Swapping to level {level}");
			CurrentLevelData?.gameObject.SetActive(false);
			CurrentLevelData = currentLevelSet.GetLevel(level);
			CurrentLevelData.gameObject.SetActive(true);
			CurrentLevelData.Init();
			CurrentLevelData.LevelNum = level;
			CurrentLevel = level;
		}

		void CallOnFirstTimeLoad(int currentLevel)
		{
			var key = GetLastDiscoveredLevelKey(currentLevelSet.LevelSetId);
			// update player's progress
			int lastOpenedLevel = PlayerPrefs.GetInt(key, -1);
			if (currentLevel >= lastOpenedLevel)
			{
				// Level is not yet finished!
				CurrentLevelData.OnFirstTimeLoad();
			}
		}

		public void SetupLevel()
		{
			OnLevelSetupEvent?.Invoke(CurrentLevelData);
		}

		public void OnFinishedLevel(ScoreData scoreData)
		{
			if (CurrentLevel + 1 < currentLevelSet.LevelCount)
				TrySaveLastDiscoveredLevel();

			TrySaveStars(scoreData.stars);
			TrySaveLastLevelSet();

			OnFinishedLevelEvent?.Invoke(scoreData);
		}

		public void OnBallReleased()
		{
			CurrentLevelData.OnBallReleased();
		}

		public async Task NextLevel()
		{
			this.Log("Last level index:", currentLevelSet.LevelCount - 1, "Current level index:", CurrentLevel);
			if (CurrentLevel >= currentLevelSet.LevelCount - 1)
			{
				if (LevelSetId + 1 >= levelSetPrefabs.Count)
				{
                    OnFinishedGameEvent?.Invoke();
                }
				else
				{
					await ActivateLevelSet(LevelSetId + 1, 0);
				}
			}
			else
			{
				await LoadLevelAsync(CurrentLevel + 1);
			}
		}

		public async Task LoadLevelAsync(int level)
		{
			destroyCancellationToken.ThrowIfCancellationRequested();

			await gameManager.StartLoading(destroyCancellationToken);

			this.Log("Loading level: Level", level);
			if (level >= currentLevelSet.LevelCount)
				throw new ArgumentException($"Incorrect level={level} was provided");

			SwapLevelTo(level);
			SetupLevel();
			CallOnFirstTimeLoad(level);

			await Task.Delay(1000, destroyCancellationToken);

			OnLevelIsLoadedEvent?.Invoke(CurrentLevelData);
			await gameManager.ResumeGame(destroyCancellationToken);
		}

		#region Save
		// ==================================================================================================================
		public static string GetEarnedStarsKey(int currentLevel, int levelSetId) => "Level-Stars-" + currentLevel + "Set-" + levelSetId;

		public static string GetLastDiscoveredLevelKey(int levelSetId) => "Player-Last-Level-" + levelSetId;

		public static string GetLastLevelSetIdKey() => "Last-Played-Level-Set";

		void TrySaveStars(int stars)
		{
			var earnedStarsKey = GetEarnedStarsKey(CurrentLevel, currentLevelSet.LevelSetId);
			int savedProgress = PlayerPrefs.GetInt(earnedStarsKey, 0);
			if (stars > savedProgress)
				PlayerPrefs.SetInt(earnedStarsKey, stars);
		}

		void TrySaveLastDiscoveredLevel()
		{
			var lastLevelKey = GetLastDiscoveredLevelKey(currentLevelSet.LevelSetId);
			int savedProgress = PlayerPrefs.GetInt(lastLevelKey, 0);
			if (CurrentLevel + 1 > savedProgress)
				PlayerPrefs.SetInt(lastLevelKey, CurrentLevel + 1);
		}

		void TrySaveLastLevelSet()
		{
			var lastLevelSetIdKey = GetLastLevelSetIdKey();
			int savedProgress = PlayerPrefs.GetInt(lastLevelSetIdKey, 0);
			if (LevelSetId > savedProgress)
				PlayerPrefs.SetInt(lastLevelSetIdKey, LevelSetId);
		}
		// ==================================================================================================================
		#endregion

	}
}
