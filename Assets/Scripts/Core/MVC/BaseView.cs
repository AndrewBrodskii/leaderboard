using System;
using ObjectPool;
using UnityEngine;

namespace MVC
{
    public abstract class BaseView<TModel> : MonoBehaviour, IPoolable
        where TModel : class, IModel
    {
        public event Action<string, MonoBehaviour> OnDeactivated;
        protected TModel Model;

        public void Initialize(TModel model)
        {
            Model = model;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            OnShown();
        }

        public void Hide()
        {
            OnHidden();
            gameObject.SetActive(false);
            OnDeactivated?.Invoke(name, this);
        }

        protected virtual void OnShown(){}
        protected virtual void OnHidden(){}
    }
}