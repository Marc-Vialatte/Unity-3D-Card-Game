using System.Collections;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T Instance => _instance;

    private void Awake()
    {
        if (_instance) return;

        _instance = (T)this;
    }
}