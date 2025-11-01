namespace MVC
{
    public abstract class BaseController<TView, TModel>
    where TView : BaseView<TModel>
    where TModel : class, IModel
    {
        protected readonly TView View;
        protected readonly TModel Model;

        public BaseController(TView view, TModel model)
        {
            View = view;
            Model = model;
            View.Initialize(Model);
        }
    }
}