using MVC;
using UnityEngine;

namespace Leaderboard.Item
{
    public class LeaderboardItemModel : IModel
    {
        public int Place { get; private set; }
        public string Score { get; private set; }//сделать инт
        public Sprite Avatar { get; private set; }

        public readonly bool IsPlayer;
        public readonly string Nickname;

        public LeaderboardItemModel(bool isPlayer, string nickname, string score, Sprite avatar)
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