using System.Collections.Generic;
using Leaderboard.Data;
using Leaderboard.Item;
using Leaderboard.Item.PlayerItem;
using MVC;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardModel : IModel
    {
        private const string TestPlayerName = "Player 10";

        public List<LeaderboardItemModel> LeaderboardItemModels { get; } = new();
        public LeaderboardPlayerItemView PlayerItemView { get; private set; }

        private readonly LeaderboardSettings _settings;
        private readonly LeaderboardData _leaderboardData;

        public LeaderboardModel(LeaderboardSettings settings, LeaderboardData leaderboardData)
        {
            _settings = settings;
            _leaderboardData = leaderboardData;
            CreateMockData();
        }

        public void SetPlayerItemView(LeaderboardPlayerItemView leaderboardPlayerItemView)
            => PlayerItemView = leaderboardPlayerItemView;

        private void CreateMockData()
        {
            var place = 1;
            foreach (var itemData in _leaderboardData.leaderboard)
            {
                var isPlayer = itemData.name == TestPlayerName;
                
                LeaderboardItemModels.Add(
                    new LeaderboardItemModel(isPlayer, itemData.name, itemData.score, place, itemData.avatar, GetItemColor(itemData.type)));
                place++;
            }
        }

        private Color GetItemColor(PlayerType playerType) =>
            playerType switch
            {
                PlayerType.Diamond => _settings.DiamondColor,
                PlayerType.Gold => _settings.GoldColor,
                PlayerType.Silver => _settings.SilverColor,
                PlayerType.Bronze => _settings.BronzeColor,
                PlayerType.Default => _settings.DefaultColor
            };
    }
}