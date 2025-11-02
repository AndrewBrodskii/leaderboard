using Leaderboard.Data;
using MVC;
using UnityEngine;

namespace Leaderboard.Item
{
    public class LeaderboardItemModel : IModel
    {
        public readonly int Place;
        public readonly int Score;
        public readonly string AvatarUrl;
        public readonly string Nickname;
        public readonly bool IsPlayer;
        public readonly Color PlayerTypeColor;

        public LeaderboardItemModel(bool isPlayer, string nickname, int score, int place, string avatarUrl, Color playerTypeColor)
        {
            IsPlayer = isPlayer;
            Nickname = nickname;
            Score = score;
            AvatarUrl = avatarUrl;
            Place = place;
            PlayerTypeColor = playerTypeColor;
        }
    }
}