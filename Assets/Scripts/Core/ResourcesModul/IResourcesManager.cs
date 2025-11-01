using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ResourcesModul
{
    public interface IResourcesManager
    {
        UniTask<Object> LoadPrefabAsync<T>() where T : Object;
        void Release<T>();
        void ClearCache();//todo add to ICacheOwner interface
    }
}