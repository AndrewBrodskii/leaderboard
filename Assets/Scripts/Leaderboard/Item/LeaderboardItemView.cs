using Cysharp.Threading.Tasks;
using MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard.Item
{
    public class LeaderboardItemView : BaseView<LeaderboardItemModel>
    {
        [SerializeField] private Image avatar;
        [SerializeField] private TMP_Text placeText;
        [SerializeField] private TMP_Text nicknameText;
        [SerializeField] private TMP_Text scoreText;

        protected override async UniTask OnShownAsync()
        {
            avatar.sprite = Model.Avatar;
            placeText.text = $"{Model.Place}";
            nicknameText.text = Model.Nickname;
            scoreText.text = $"{Model.Score}";
        }

        protected override void OnHidden()
        {
        }
    }
}