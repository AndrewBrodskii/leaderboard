using UnityEngine;

namespace Leaderboard.Item.PlayerItem
{
    public class LeaderboardPlayerItemView : LeaderboardItemView
    {
        [SerializeField] private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;
    }
}