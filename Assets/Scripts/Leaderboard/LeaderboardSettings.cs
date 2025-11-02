using UnityEngine;
using UnityEngine.Serialization;

namespace Leaderboard
{
    [CreateAssetMenu(fileName = "Leaderboard", menuName = "Leaderboard/Settings")]
    public class LeaderboardSettings : ScriptableObject
    {
        [FormerlySerializedAs("_firstPlaceColor")] [SerializeField] private Color diamondColor;
        [FormerlySerializedAs("_secondPlaceColor")] [SerializeField] private Color goldColor;
        [FormerlySerializedAs("_thirdPlaceColor")] [SerializeField] private Color silverColor;
        [FormerlySerializedAs("_usualPlaceColor")] [SerializeField] private Color defaultColor;
        [FormerlySerializedAs("_badPlaceColor")] [SerializeField] private Color bronzeColor;

        [SerializeField] private int _leaderboardSize;
        [SerializeField] private int _badPlacesCount;

        public Color DiamondColor => diamondColor;
        public Color GoldColor => goldColor;
        public Color SilverColor => silverColor;
        public Color DefaultColor => defaultColor;
        public Color BronzeColor => bronzeColor;

        public int LeaderboardSize => _leaderboardSize;
        public int BadPlacesCount => _badPlacesCount;
    }
}