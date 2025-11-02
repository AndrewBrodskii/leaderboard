using System;
using Cysharp.Threading.Tasks;
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
        public RectTransform PlayerContainer => _containerRectTransform;

        private float _containerTop;
        private float _containerBottom;

        protected override async UniTask OnShownAsync()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _scrollRect.onValueChanged.AddListener(OnScrollChanged);

            var containerHeight = _containerRectTransform.rect.height * _containerRectTransform.lossyScale.y;
            _containerTop = _containerRectTransform.position.y + containerHeight * (1 - _containerRectTransform.pivot.y);
            _containerBottom = _containerRectTransform.position.y - containerHeight * _containerRectTransform.pivot.y;

            await CenterPlayerItemInContainerImmediate();
        }

        protected override void OnHidden()
        {
            _closeButton.onClick.RemoveAllListeners();
            _scrollRect.onValueChanged.RemoveListener(OnScrollChanged);
        }
        
        private async UniTask CenterPlayerItemInContainerImmediate()
        {
            await UniTask.Yield();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentContainerTransform);
            Canvas.ForceUpdateCanvases();

            var content = _contentContainerTransform;
            var itemRect = Model.PlayerItemView.RectTransform;
            var containerRect = _containerRectTransform;
            var itemBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(content, itemRect);
            var containerBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(content, containerRect);
            var itemCenter = itemBounds.center;
            var containerCenter = containerBounds.center;
            Vector2 delta = itemCenter - containerCenter;
            var anchored = content.anchoredPosition;
            anchored -= delta;
            content.anchoredPosition = anchored;
            
            await UniTask.Yield();
        }

        private void OnCloseButtonClicked() => CloseButtonClicked?.Invoke();

        private void ScrollToCenter()
        {
            var content = _scrollRect.content;
            var viewport = _scrollRect.viewport;
            var playerItem = Model.PlayerItemView.RectTransform;

            // Центр вьюпорта и айтема в локальных координатах контента
            Vector2 viewportCenterLocalPos = content.InverseTransformPoint(viewport.TransformPoint(viewport.rect.center));
            Vector2 itemCenterLocalPos = content.InverseTransformPoint(playerItem.TransformPoint(playerItem.rect.center));

            // Смещение, необходимое для центрирования
            float offsetY = itemCenterLocalPos.y - viewportCenterLocalPos.y;

            // Применяем смещение
            Vector2 anchoredPosition = content.anchoredPosition;
            anchoredPosition.y += offsetY;
            content.anchoredPosition = anchoredPosition;
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
            
            if(_verticalLayoutGroupAnchor.gameObject.activeInHierarchy)
                return;
            
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