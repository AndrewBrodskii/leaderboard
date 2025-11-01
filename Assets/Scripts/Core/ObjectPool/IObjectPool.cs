using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ObjectPool
{
    public interface IObjectPool
    {
        UniTask<T> GetAsync<T>(Transform parent) where T : MonoBehaviour, IPoolable;
    }
}