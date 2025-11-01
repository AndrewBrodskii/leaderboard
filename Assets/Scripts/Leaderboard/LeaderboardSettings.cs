using UnityEngine;

namespace Leaderboard
{
    [CreateAssetMenu(fileName = "Leaderboard", menuName = "Leaderboard/Settings")]
    public class LeaderboardSettings : ScriptableObject
    {
        [SerializeField] private Color _firstPlaceColor;
        [SerializeField] private Color _secondPlaceColor;
        [SerializeField] private Color _thirdPlaceColor;
        [SerializeField] private Color _usualPlaceColor;
        [SerializeField] private Color _badPlaceColor;

        [SerializeField] private int _leaderboardSize;
        [SerializeField] private int _badPlacesCount;

        public Color FirstPlaceColor => _firstPlaceColor;
        public Color SecondPlaceColor => _secondPlaceColor;
        public Color ThirdPlaceColor => _thirdPlaceColor;
        public Color UsualPlaceColor => _usualPlaceColor;
        public Color BadPlaceColor => _badPlaceColor;

        public int LeaderboardSize => _leaderboardSize;
        public int BadPlacesCount => _badPlacesCount;
    }
}