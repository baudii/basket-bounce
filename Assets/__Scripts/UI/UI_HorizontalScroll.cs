using DG.Tweening;
using KK.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasketBounce.UI
{
    public class UI_HorizontalScroll : MonoBehaviour
    {
        [SerializeField] float slideDuration;
        [SerializeField, Tooltip("Means that some children shouldn't interfere with the bound detection algorithm")] int ignoreChildren;
        int currentChildNum;
        RectTransform curRect;
        private void Start()
        {
            curRect = (RectTransform)transform;
            currentChildNum = 0;
        }
        public void Slide(int dir)
        {
            dir = Utils.Signum(dir);

            if (dir == 0)
                return;

            if (currentChildNum + dir >= curRect.childCount - ignoreChildren || currentChildNum + dir < 0)
                return;

            var currentChild = (RectTransform)transform.GetChild(currentChildNum);
            var movement = currentChild.sizeDelta.x * dir;
            curRect.DOAnchorPosX(curRect.anchoredPosition.x - movement, slideDuration);

            currentChildNum += dir;
        }
    }
}
