using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DI;
using Leaderboard.Item;
using Leaderboard.Item.PlayerItem;
using MVC;
using ObjectPool;
using ResourcesModul;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardController : BaseController<LeaderboardView, LeaderboardModel>
    {
        private readonly IObjectPool _objectPool;
        private readonly List<LeaderboardItemController> _leaderboardItemControllers = new();
        private readonly IResourcesManager _resourcesManager;
        private readonly List<UniTask> _uniTasks = new();

        public LeaderboardController(LeaderboardView view, LeaderboardModel model) : base(view, model)
        {
            _objectPool = DiContainer.Instance.Get<IObjectPool>();
        }

        public async UniTask ShowLeaderboardAsync()
        {
            View.CloseButtonClicked += OnCloseButtonClicked;
            
            LeaderboardItemModel leaderboardPlayerItemModel = null;
            foreach (var leaderboardItemModel in Model.LeaderboardItemModels)
            {
                if (leaderboardItemModel.IsPlayer)
                {
                    leaderboardPlayerItemModel = leaderboardItemModel;
                    continue;
                }
            
                await ShowItem(leaderboardItemModel);
            }

            if (leaderboardPlayerItemModel != null)
                await ShowPlayerItem(leaderboardPlayerItemModel);
            else
                Debug.LogWarning("Player data not found");
            
            await View.ShowAsync();
            await UniTask.WhenAll(_uniTasks);
        }
        

        private async UniTask ShowItem(LeaderboardItemModel leaderboardItemModel)
        {
            var leaderboardItemView = await _objectPool.GetAsync<LeaderboardItemView>(View.ContentContainerTransform);

            var leaderboardItemController = new LeaderboardItemController(leaderboardItemView, leaderboardItemModel);
            _uniTasks.Add(leaderboardItemController.ShowAsync());
            _leaderboardItemControllers.Add(leaderboardItemController);
        }

        private async UniTask ShowPlayerItem(LeaderboardItemModel leaderboardItemModel)
        {
            var leaderboardItemView = await CreatePlayerItems(leaderboardItemModel);

            var leaderboardItemController = new LeaderboardItemController(leaderboardItemView, leaderboardItemModel);
            _uniTasks.Add(leaderboardItemController.ShowAsync());
            _leaderboardItemControllers.Add(leaderboardItemController);
        }

        private async UniTask<LeaderboardPlayerItemView> CreatePlayerItems(LeaderboardItemModel leaderboardItemModel)
        {
            var leaderboardItemView = await _objectPool.GetAsync<LeaderboardPlayerItemView>(View.ContentContainerTransform);
            leaderboardItemView.transform.SetSiblingIndex(leaderboardItemModel.Place - 1);

            var leaderboardItemViewTop = await _objectPool.GetAsync<LeaderboardPlayerItemView>(View.PlayerContainer);
            var leaderboardItemControllerTop = new LeaderboardItemController(leaderboardItemViewTop, leaderboardItemModel);
            Model.SetPlayerItemView(leaderboardItemView);
            _uniTasks.Add(leaderboardItemControllerTop.ShowAsync());
            _leaderboardItemControllers.Add(leaderboardItemControllerTop);

            return leaderboardItemView;
        }

        private void OnCloseButtonClicked()
        {
            View.CloseButtonClicked -= OnCloseButtonClicked;
            foreach (var leaderboardItemController in _leaderboardItemControllers)
            {
                leaderboardItemController.Hide();
            }

            View.Hide();
        }
    }
}