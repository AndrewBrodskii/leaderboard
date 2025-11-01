using System.Collections.Generic;
using System.Linq;
using Leaderboard.Item;
using Leaderboard.Item.PlayerItem;
using MVC;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardModel : IModel
    {
        private const int MockDataSize = 50;
        private const string PlayerNickname = "BroDi";
        private const string DefaultNickname = "Player{0}";

        public List<LeaderboardItemModel> MockDatas { get; private set; } = new();
        public LeaderboardPlayerItemView PlayerItemView { get; private set; }

        private readonly LeaderboardSettings _settings;

        public LeaderboardModel(LeaderboardSettings settings)
        {
            _settings = settings;
            CreateMockData();
        }
        
        public void SetPlayerItemView(LeaderboardPlayerItemView leaderboardPlayerItemView)
            => PlayerItemView = leaderboardPlayerItemView;

        private void CreateMockData()
        {
            var scores = GetRandomScores();
            MockDatas.Add(new LeaderboardItemModel(true, PlayerNickname, $"{scores[0]}", null));
            for (var i = 1; i < MockDataSize; i++)
            {
                MockDatas.Add(new LeaderboardItemModel(false,string.Format(DefaultNickname, i), $"{scores[i]}", null));
            }

            MockDatas = MockDatas.OrderBy(x => x.Score).ToList();

            for (var i = 0; i < MockDatas.Count; i++)
            {
                MockDatas[i].SetData(i + 1);
            }
        }

        private List<int> GetRandomScores()
        {
            var scores = new List<int>();
            for (var i = 0; i < MockDataSize; i++)
            {
                scores.Add(Random.Range(0, 1000));
            }

            return scores;
        }

        private Color GetItemColor(int place) =>
            place switch
            {
                1 => _settings.FirstPlaceColor,
                2 => _settings.SecondPlaceColor,
                3 => _settings.ThirdPlaceColor,
                _ => place <= _settings.LeaderboardSize - _settings.BadPlacesCount
                    ? _settings.UsualPlaceColor
                    : _settings.BadPlaceColor
            };
    }
}