using System;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Converters;

namespace Leaderboard.Data
{
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlayerType
    {
        Default = 0,
        Diamond = 1,
        Gold = 2,
        Silver = 3,
        Bronze = 4,
    }
}