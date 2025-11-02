using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ResourcesModul
{
    public interface IResourcesManager
    {
        UniTask<T> LoadPrefabAsync<T>() where T : Object;
        void Release<T>();
        void ClearCache();//todo add to ICacheOwner interface
    }
}