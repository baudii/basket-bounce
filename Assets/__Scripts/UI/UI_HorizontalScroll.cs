using DG.Tweening;
using KK.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BasketBounce.UI
{
    public class UI_HorizontalScroll : MonoBehaviour
    {
        [SerializeField] UnityEvent<int> onSlide;
        [SerializeField] float slideDuration;
        [SerializeField, Tooltip("Means that some children shouldn't interfere with the bound detection algorithm. They should be placed last.")] int ignoreChildren;
        int currentChildNum;
        RectTransform curRect;
        private void Start()
        {
            Time.timeScale = 1;
            curRect = (RectTransform)transform;
            currentChildNum = 0;
        }
        public void Slide(int dir)
        {
            dir = Utils.Signum(dir);

            this.Log($"CurChildNum {currentChildNum}, dir {dir}, ignore {ignoreChildren}");

            if (dir == 0)
                return;

            if (currentChildNum + dir >= curRect.childCount - ignoreChildren || currentChildNum + dir < 0)
                return;


            var curChild = (RectTransform)transform.GetChild(currentChildNum);
            var movement = curChild.sizeDelta.x * dir;
            curRect.DOAnchorPosX(curRect.anchoredPosition.x - movement, slideDuration);

            currentChildNum += dir;
            onSlide?.Invoke(currentChildNum);

            ScrollRect scroll = curChild.GetComponent<ScrollRect>();
            scroll.StopMovement();
            ((RectTransform)curChild.GetChild(0).GetChild(0)).DOAnchorPosY(0, slideDuration);
        }
    }
}
