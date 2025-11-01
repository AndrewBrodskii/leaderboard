using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Factory;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectPoolManager : IObjectPool
    {
        private readonly Dictionary<string, Queue<MonoBehaviour>> _objectContainer;
        private readonly IFactory _factory;

        public ObjectPoolManager(IFactory factory)
        {
            _objectContainer = new Dictionary<string, Queue<MonoBehaviour>>();
            _factory = factory;
        }

        public async UniTask<T> GetAsync<T>(Transform parent) where T : MonoBehaviour, IPoolable
        {
            T pooledObj;
            if (_objectContainer.TryGetValue(typeof(T).Name, out var value) && value.Count > 0)
            {
                pooledObj = value.Dequeue() as T;
            }
            else
            {
                pooledObj = await _factory.CreateGameObjectAsync<T>(parent);
                pooledObj.OnDeactivated += Release;
            }

            if (!pooledObj) return null;

            pooledObj.transform.SetParent(parent, false);
            return pooledObj;
        }

        private void Release(string objectType, MonoBehaviour pooledObj)
        {
            if (!_objectContainer.ContainsKey(objectType))
            {
                _objectContainer.Add(objectType, new Queue<MonoBehaviour>());
            }

            _objectContainer[objectType].Enqueue(pooledObj);
        }
    }
}