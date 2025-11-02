using System;

namespace Leaderboard.Data
{
    [Serializable]
    public class LeaderboardItemData
    {
        public string name;
        public int score;
        public string avatar;
        public string type;
    }
}