using System;
using Cysharp.Threading.Tasks;
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

        public async UniTask ShowAsync()
        {
            gameObject.SetActive(true);
            await OnShownAsync();
        }

        public void Hide()
        {
            OnHidden();
            gameObject.SetActive(false);
            OnDeactivated?.Invoke(name, this);
        }

        protected virtual async UniTask OnShownAsync(){}
        protected virtual void OnHidden(){}
    }
}