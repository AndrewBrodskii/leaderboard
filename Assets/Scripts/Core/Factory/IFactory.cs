using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Factory
{
    public interface IFactory
    {
        UniTask<T> CreateGameObjectAsync<T>() where T : MonoBehaviour;
        UniTask<T> CreateGameObjectAsync<T>(Transform parent) where T : MonoBehaviour;
    }
}