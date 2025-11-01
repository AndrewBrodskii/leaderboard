using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DI;
using Leaderboard.Item;
using Leaderboard.Item.PlayerItem;
using MVC;
using ObjectPool;

namespace Leaderboard
{
    public class LeaderboardController : BaseController<LeaderboardView, LeaderboardModel>
    {
        private readonly IObjectPool _objectPool;
        private readonly List<LeaderboardItemController> _leaderboardItemControllers = new();

        public LeaderboardController(LeaderboardView view, LeaderboardModel model) : base(view, model)
        {
            _objectPool = DiContainer.Instance.Get<IObjectPool>();
        }

        public async UniTaskVoid ShowLeaderboardAsync()
        {
            View.CloseButtonClicked += OnCloseButtonClicked;

            foreach (var leaderboardItemModel in Model.MockDatas)
            {
                LeaderboardItemView leaderboardItemView;
                if (leaderboardItemModel.IsPlayer)
                {
                    leaderboardItemView = await CreatePlayerItems(leaderboardItemModel);
                }
                else
                {
                    leaderboardItemView = await _objectPool.GetAsync<LeaderboardItemView>(View.ContentContainerTransform);
                }
                
                var leaderboardItemController = new LeaderboardItemController(leaderboardItemView, leaderboardItemModel);
                leaderboardItemController.Show();
                _leaderboardItemControllers.Add(leaderboardItemController);
            }
            
            View.Show();
        }

        private async UniTask<LeaderboardItemView> CreatePlayerItems(LeaderboardItemModel leaderboardItemModel)
        {
            var leaderboardItemView = await _objectPool.GetAsync<LeaderboardPlayerItemView>(View.ContentContainerTransform);
            leaderboardItemView.transform.SetSiblingIndex(leaderboardItemModel.Place - 1);
            
            var leaderboardItemViewTop = await _objectPool.GetAsync<LeaderboardPlayerItemView>(View.VerticalLayoutGroupTransform.transform);
            var leaderboardItemControllerTop = new LeaderboardItemController(leaderboardItemViewTop, leaderboardItemModel);
            Model.SetPlayerItemView(leaderboardItemView);
            leaderboardItemControllerTop.Show();
            _leaderboardItemControllers.Add(leaderboardItemControllerTop);
            
            return leaderboardItemView;
        }

        private void OnCloseButtonClicked()
        {
            foreach (var leaderboardItemController in _leaderboardItemControllers)
            {
                leaderboardItemController.Hide();
            }

            View.Hide();
        }
    }
}