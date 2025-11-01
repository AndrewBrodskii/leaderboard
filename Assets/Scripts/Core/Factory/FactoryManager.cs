using Cysharp.Threading.Tasks;
using ResourcesModul;
using UnityEngine;

namespace Factory
{
    public class FactoryManager : IFactory
    {
        private readonly IResourcesManager _resourceManager;

        public FactoryManager(IResourcesManager resourcesManager)
        {
            _resourceManager = resourcesManager;
        }

        public async UniTask<T> CreateGameObjectAsync<T>() where T : MonoBehaviour
        {
            return await CreateGameObjectAsync<T>(parent: null);
        }

        public async UniTask<T> CreateGameObjectAsync<T>(Transform parent) where T : MonoBehaviour
        {
            var prefab = await _resourceManager.LoadPrefabAsync<T>() as GameObject;
            var go = Object.Instantiate(prefab, parent);
            go.name = typeof(T).Name;
            go.transform.SetParent(parent);

            return go.GetComponent<T>();
        }
    }
}