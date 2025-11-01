using DI;
using Leaderboard;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using ObjectPool;
using ResourcesModul;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Transform _popupContainerTransform;
        
        private IObjectPool _objectPool;
        private IResourcesManager _resourcesManager;

        private void Start()
        {
            _objectPool = DiContainer.Instance.Get<IObjectPool>();
            _resourcesManager = DiContainer.Instance.Get<IResourcesManager>();
            _leaderboardButton.onClick.AddListener(OnLeaderboardButtonClicked);
        }

        private void OnDestroy()
        {
            _leaderboardButton.onClick.RemoveListener(OnLeaderboardButtonClicked);
        }

        private void OnLeaderboardButtonClicked()
        {
            ShowLeaderboard().Forget();
        }

        private async UniTaskVoid ShowLeaderboard()
        {
            var settings = await _resourcesManager.LoadPrefabAsync<LeaderboardSettings>() as LeaderboardSettings;
            var leaderboardView = await _objectPool.GetAsync<LeaderboardView>(_popupContainerTransform);
            var leaderboardController = new LeaderboardController(leaderboardView, new LeaderboardModel(settings));
            leaderboardController.ShowLeaderboardAsync().Forget();
        }
    }
}