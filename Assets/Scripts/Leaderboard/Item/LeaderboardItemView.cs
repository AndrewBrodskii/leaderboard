using System.Threading;
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
        [SerializeField] private Image playerType;
        [SerializeField] private TMP_Text placeText;
        [SerializeField] private TMP_Text nicknameText;
        [SerializeField] private TMP_Text scoreText;
        
        private CancellationTokenSource _cts;
        private Sprite _defaultSprite;

        public void SetAvatarSprite(Sprite sprite)
        {
            _cts.Cancel();
            avatar.fillAmount = 1f;
            avatar.sprite = sprite;
        }

        protected override async UniTask OnShownAsync()
        {
            _defaultSprite = avatar.sprite;
            placeText.text = $"{Model.Place}";
            nicknameText.text = Model.Nickname;
            scoreText.text = $"{Model.Score}";
            playerType.color = Model.PlayerTypeColor;

            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            AnimateFillAsync(_cts.Token).Forget();
        }

        protected override void OnHidden()
        {
            avatar.sprite = _defaultSprite;
            _cts?.Cancel();
        }
        
        private async UniTask AnimateFillAsync(CancellationToken token)
        {
            await UniTask.WaitUntil(() => avatar.isActiveAndEnabled, cancellationToken: token);
            while (!token.IsCancellationRequested)
            {
                avatar.fillAmount += Time.deltaTime * 0.5f;
                if (avatar.fillAmount >= 1f)
                    avatar.fillAmount = 0f;
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
    }
}