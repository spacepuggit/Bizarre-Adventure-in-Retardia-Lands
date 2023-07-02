using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static ServiceLocator _instance;
    private Dictionary<Type, object> _services;

    public static ServiceLocator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject(nameof(ServiceLocator)).AddComponent<ServiceLocator>();
            }
            return _instance;
        }
    }

    private ServiceLocator()
    {
        _services = new Dictionary<Type, object>();
    }

    public void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public T Get<T>()
    {
        return (T)_services[typeof(T)];
    }
}
