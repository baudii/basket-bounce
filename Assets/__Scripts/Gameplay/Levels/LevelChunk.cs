using KK.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BasketBounce.Gameplay.Levels
{
    public class LevelChunk : MonoBehaviour
    {
		[SerializeField] List<LevelData> levels;
		public int levelChunkIndex { get; private set; }
		public int LevelCount => levels.Count;

#if UNITY_EDITOR

		[SerializeField] bool validate;
		private void OnValidate()
		{
			var name = transform.name;

			if (validate)
			{
				int.TryParse(name.Split(' ').Last(), out int index);

				levelChunkIndex = index - 1;
				ValidateChunk();
				validate = false;
			}
		}

#endif

		public void ValidateChunk()
		{
			levels = new List<LevelData>();

			int i = 1;
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent(out LevelData levelData))
				{
					levelData.gameObject.name = "Level " + i;
					levels.Add(levelData);
					if (levelData.gameObject.activeSelf)
					{
						levelData.ValidateLevel();
						levelData.gameObject.SetActive(false);
					}
					i++;
				}
			}
		}

		public LevelData GetLevel(int level)
		{
			this.Log("Retrieving level: ", levels[level]);
			return levels[level];
		}
    }
}
