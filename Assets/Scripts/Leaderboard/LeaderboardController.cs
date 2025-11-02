using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DI;
using Leaderboard.Data;
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
        
        private LeaderboardData _leaderboardData;

        public LeaderboardController(LeaderboardView view, LeaderboardModel model) : base(view, model)
        {
            _objectPool = DiContainer.Instance.Get<IObjectPool>();
            _resourcesManager = DiContainer.Instance.Get<IResourcesManager>();
        }

        public async UniTask ShowLeaderboardAsync()
        {
            View.CloseButtonClicked += OnCloseButtonClicked;

            _leaderboardData = await _resourcesManager.LoadJsonAsync<LeaderboardData>();
            LeaderboardItemModel leaderboardPlayerItemModel = null;
            foreach (var leaderboardItemModel in Model.MockDatas)
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
        }

        private async UniTask ShowItem(LeaderboardItemModel leaderboardItemModel)
        {
            var leaderboardItemView = await _objectPool.GetAsync<LeaderboardItemView>(View.ContentContainerTransform);

            var leaderboardItemController = new LeaderboardItemController(leaderboardItemView, leaderboardItemModel);
            leaderboardItemController.Show();
            _leaderboardItemControllers.Add(leaderboardItemController);

            await UniTask.CompletedTask;
        }

        private async UniTask ShowPlayerItem(LeaderboardItemModel leaderboardItemModel)
        {
            var leaderboardItemView = await CreatePlayerItems(leaderboardItemModel);

            var leaderboardItemController = new LeaderboardItemController(leaderboardItemView, leaderboardItemModel);
            leaderboardItemController.Show();
            _leaderboardItemControllers.Add(leaderboardItemController);

            await UniTask.CompletedTask;
        }

        private async UniTask<LeaderboardPlayerItemView> CreatePlayerItems(LeaderboardItemModel leaderboardItemModel)
        {
            var leaderboardItemView = await _objectPool.GetAsync<LeaderboardPlayerItemView>(View.ContentContainerTransform);
            leaderboardItemView.transform.SetSiblingIndex(leaderboardItemModel.Place - 1);

            var leaderboardItemViewTop = await _objectPool.GetAsync<LeaderboardPlayerItemView>(View.PlayerContainer);
            var leaderboardItemControllerTop = new LeaderboardItemController(leaderboardItemViewTop, leaderboardItemModel);
            Model.SetPlayerItemView(leaderboardItemView);
            leaderboardItemControllerTop.Show();
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