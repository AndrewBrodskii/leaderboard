using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace ResourcesModul
{
    public class ResourcesManager : IResourcesManager
    {
        private readonly Dictionary<string, Object> _cache = new();

        public async UniTask<T> LoadPrefabAsync<T>() where T : Object
        {
            var key = typeof(T).Name;
            if(_cache.TryGetValue(key, out var obj))
                return obj as T;

            var operation = Addressables.LoadAssetAsync<Object>(key);
            await operation.Task;

            if (operation.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load asset: {typeof(T)}");
                return null;
            }

            if (operation.Result is not GameObject go)
            {
                _cache.Add(key, operation.Result as T);
                return operation.Result as T;
            }

            if (go.TryGetComponent(out T component))
            {
                _cache.Add(key, component);
                return component;
            }
            
            Debug.LogError($"Failed to load asset: {typeof(T)}");
            return null;
        }
        
        public async UniTask<T> LoadJsonAsync<T>() where T : class
        {
            var key = typeof(T).Name;
            var operation = Addressables.LoadAssetAsync<TextAsset>(key);
            await operation.Task;

            if (operation.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"❌ Failed to load JSON asset: {key}");
                return null;
            }

            var textAsset = operation.Result;
            if (textAsset == null)
            {
                Debug.LogError($"❌ JSON asset is null: {key}");
                return null;
            }

            try
            {
                var data = JsonUtility.FromJson<T>(textAsset.text);
                if (data == null)
                    Debug.LogError($"⚠️ Failed to parse JSON for type {typeof(T)}");

                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"⚠️ Exception parsing JSON: {e}");
                return null;
            }
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