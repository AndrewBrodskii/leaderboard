using MVC;
using UnityEngine;

namespace Leaderboard.Item
{
    public class LeaderboardItemModel : IModel
    {
        public int Place { get; private set; }
        public int Score { get; private set; }
        public Sprite Avatar { get; private set; }

        public readonly bool IsPlayer;
        public readonly string Nickname;

        public LeaderboardItemModel(bool isPlayer, string nickname, int score, Sprite avatar)
        {
            IsPlayer = isPlayer;
            Nickname = nickname;
            Score = score;
            Avatar = avatar;
        }

        public void SetData(int place)
        {
            Place = place;
        }
    }
}