using System;
using UnityEngine;

namespace ObjectPool
{
    public interface IPoolable
    {
        event Action<string, MonoBehaviour> OnDeactivated; 
    }
}