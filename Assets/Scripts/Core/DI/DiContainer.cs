using System;
using System.Collections.Generic;
using Factory;
using ObjectPool;
using ResourcesModul;
using UnityEngine;

namespace DI
{
    public class DiContainer : MonoBehaviour
    {
        public static DiContainer Instance { get; private set; }

        private readonly Dictionary<Type, object> _services = new();

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Register<IResourcesManager>(new ResourceManager());
            Register<IFactory>(new FactoryManager(Get<IResourcesManager>()));
            Register<IObjectPool>(new ObjectPoolManager(Get<IFactory>()));
        }

        public void Register<T>(T service) => _services[typeof(T)] = service;
        public T Get<T>() => (T)_services[typeof(T)];
    }
}