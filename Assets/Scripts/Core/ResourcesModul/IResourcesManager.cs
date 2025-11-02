using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ResourcesModul
{
    public interface IResourcesManager
    {
        UniTask<T> LoadPrefabAsync<T>() where T : Object;
        UniTask<T> LoadJsonAsync<T>() where T : class;
        UniTask<Sprite> DownloadTextureAsync(string url);
        void Release<T>();
        void ClearCache();//todo add to ICacheOwner interface
    }
}