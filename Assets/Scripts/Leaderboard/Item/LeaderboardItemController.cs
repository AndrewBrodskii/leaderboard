using MVC;

namespace Leaderboard.Item
{
    public class LeaderboardItemController : BaseController<LeaderboardItemView, LeaderboardItemModel>
    {
        public LeaderboardItemController(LeaderboardItemView view, LeaderboardItemModel model) : base(view, model)
        {
        }

        public void Show()
        {
            View.Show();
        }

        public void Hide()
        {
            View.Hide();
        }
    }
}