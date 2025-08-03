using DG.Tweening.Core;
using KK.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BasketBounce.Gameplay.Levels
{
    public class LevelChunk : MonoBehaviour
    {
		[SerializeField] List<LevelData> levels;
		public int levelChunkIndex { get; private set; }
		public int LevelCount => levels.Count;

#if UNITY_EDITOR

		[Header("Editor only")]
		[SerializeField] bool validate;
		[SerializeField] bool disableAll;
		private void OnValidate()
		{
			if (validate)
            {
				try
				{

                    var name = transform.name;
                    int.TryParse(name.Split(' ').Last(), out int index);

                    levelChunkIndex = index - 1;
                    ValidateChunk(disableAll);
                }
				finally
				{
                    validate = false;
                }
			}
		}


		public void ValidateChunk(bool toDisable = true)
		{
			levels = new List<LevelData>();

			int i = 1;
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent(out LevelData levelData))
				{
					levelData.gameObject.name = "Level " + i;
					levels.Add(levelData);
                    levelData.ValidateLevel();
                    if (toDisable)
                        levelData.gameObject.SetActive(false);
                    i++;
				}
			}

            if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
            {
                this.Log($"Apply changes to prefab instance: {transform.name}");
                PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.UserAction);
            }
            else if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
            {
                this.Log($"Directly editing prefab asset: {transform.name}");
                EditorUtility.SetDirty(gameObject); // Marks prefab as dirty
                                                    // No need to ApplyPrefabInstance here — changes are saved when user presses "Save" in prefab mode
            }
        }

#endif
        public LevelData GetLevel(int level)
		{
			// 0 < level < chunkSize

			this.Log("Retrieving level: ", levels[level]);
			return levels[level];
		}
    }
}
