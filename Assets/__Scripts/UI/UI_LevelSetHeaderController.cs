using KK.Common;
using TMPro;
using UnityEngine;

namespace BasketBounce.UI
{
    public class UI_LevelSetHeaderController : MonoBehaviour
    {
		[SerializeField] TextMeshProUGUI tmp;
		[SerializeField] string[] headers = new string[]
        {
            "Library", "Maze", "Kitchen"
        };

		public void SetHeader(int index)
        {
            if (index >= 0 && index < headers.Length)
            {
                tmp.text = headers[index];
            }
            else
            {
                this.LogError($"Index {index} is out of the bounds of the headers array.");
            }
		}
    }
}
