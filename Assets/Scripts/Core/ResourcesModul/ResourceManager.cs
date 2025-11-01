using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ResourcesModul
{
    public class ResourceManager : IResourcesManager
    {
        private readonly Dictionary<string, Object> _cache = new();

        public async UniTask<Object> LoadPrefabAsync<T>() where T : Object
        {
            var key = typeof(T).Name;
            if(_cache.TryGetValue(key, out var obj))
                return obj;

            var operation = Addressables.LoadAssetAsync<Object>(key);
            await operation.Task;

            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                _cache.Add(key, operation.Result);
                return operation.Result;
            }
            
            Debug.LogError($"Failed to load asset: {typeof(T)}");
            return null;
        }
        
        public void ClearCache()
        {
            foreach (var resource in _cache)
            {
                Addressables.Release(resource.Value);
            }

            _cache.Clear();
        }

        public void Release<T>()
        {
            var key = typeof(T).Name;
            if (_cache.ContainsKey(key))
            {
                _cache.Remove(key);
                Addressables.Release(key);
            }
            
            Debug.LogError($"Failed to release resource: {key.GetType()}");
        }
    }
}