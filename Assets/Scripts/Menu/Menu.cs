using DI;
using Leaderboard;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Leaderboard.Data;
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
        private LeaderboardSettings _leaderboardSettings;

        private void Start()
        {
            _objectPool = DiContainer.Instance.Get<IObjectPool>();
            _resourcesManager = DiContainer.Instance.Get<IResourcesManager>();
            _leaderboardButton.onClick.AddListener(OnLeaderboardButtonClicked);
            InitializeAsync().Forget();
        }

        private void OnDestroy()
        {
            _leaderboardButton.onClick.RemoveListener(OnLeaderboardButtonClicked);
        }

        private async UniTask InitializeAsync()
        {
            _leaderboardSettings = await _resourcesManager.LoadPrefabAsync<LeaderboardSettings>();
        }

        private void OnLeaderboardButtonClicked()
        {
            ShowLeaderboard().Forget();
        }

        private async UniTask ShowLeaderboard()
        {
            var leaderboardView = await _objectPool.GetAsync<LeaderboardView>(_popupContainerTransform);
            var leaderboardData = await _resourcesManager.LoadJsonAsync<LeaderboardData>();
            var leaderboardController = new LeaderboardController(leaderboardView, new LeaderboardModel(_leaderboardSettings, leaderboardData));
            await leaderboardController.ShowLeaderboardAsync();
        }
    }
}