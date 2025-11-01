using System;
using MVC;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    public class LeaderboardView : BaseView<LeaderboardModel>
    {
        public event Action CloseButtonClicked;

        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _contentContainerTransform;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroupAnchor;
        [SerializeField] private Button _closeButton;
        [SerializeField] private RectTransform _containerRectTransform;

        public Transform ContentContainerTransform => _contentContainerTransform;
        public VerticalLayoutGroup VerticalLayoutGroupTransform => _verticalLayoutGroupAnchor;

        private float _containerTop;
        private float _containerBottom;

        protected override void OnShown()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _scrollRect.onValueChanged.AddListener(OnScrollChanged);
            var containerHeight = _containerRectTransform.rect.height * _containerRectTransform.lossyScale.y;
            _containerTop = _containerRectTransform.position.y + containerHeight * (1 - _containerRectTransform.pivot.y);
            _containerBottom = _containerRectTransform.position.y - containerHeight * _containerRectTransform.pivot.y;
            ScrollToCenter();
            OnScrollChanged(new Vector2(0, 0));
        }

        protected override void OnHidden()
        {
            _closeButton.onClick.RemoveAllListeners();
            _scrollRect.onValueChanged.RemoveListener(OnScrollChanged);
        }

        private void OnCloseButtonClicked() => CloseButtonClicked?.Invoke();

        private void ScrollToCenter()
        {
            var content = _scrollRect.content;
            var viewport = _scrollRect.viewport;
            Vector2 viewportLocalCenter = content.InverseTransformPoint(viewport.position);
            Vector2 targetLocalCenter = content.InverseTransformPoint(Model.PlayerItemView.transform.position);
            var offset = targetLocalCenter - viewportLocalCenter;
            content.anchoredPosition -= offset;
        }

        private void OnScrollChanged(Vector2 normalizedPos)
        {
            var target = Model.PlayerItemView.RectTransform;
            var targetHeight = target.rect.height * target.lossyScale.y;
            var targetTop = target.position.y + targetHeight * (1 - target.pivot.y);
            var targetBottom = target.position.y - targetHeight * target.pivot.y;
            var isInside = targetTop <= _containerTop && targetBottom >= _containerBottom;
            
            if (isInside)
            {
                _verticalLayoutGroupAnchor.gameObject.SetActive(false);
                return;
            }
            
            if (targetTop > _containerTop)
            {
                _verticalLayoutGroupAnchor.gameObject.SetActive(true);
                _verticalLayoutGroupAnchor.childAlignment = TextAnchor.UpperLeft;
                return;
            }
            
            _verticalLayoutGroupAnchor.gameObject.SetActive(true);
            _verticalLayoutGroupAnchor.childAlignment = TextAnchor.LowerLeft;
        }
    }
}