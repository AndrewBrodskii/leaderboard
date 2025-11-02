using Cysharp.Threading.Tasks;
using MVC;
using ResourcesModul;

namespace Leaderboard.Item
{
    public class LeaderboardItemController : BaseController<LeaderboardItemView, LeaderboardItemModel>
    {
        private readonly IResourcesManager _resourcesManager;
        public LeaderboardItemController(LeaderboardItemView view, LeaderboardItemModel model) : base(view, model)
        {
            _resourcesManager = DI.DiContainer.Instance.Get<IResourcesManager>();
        }

        public async UniTask ShowAsync()
        {
            await View.ShowAsync();
            var avatear = await _resourcesManager.DownloadTextureAsync(Model.AvatarUrl);
            await UniTask.WaitForSeconds(2);
            View.SetAvatarSprite(avatear);
        }

        public void Hide()
        {
            View.Hide();
        }
    }
}