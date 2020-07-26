using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSinglton<T> : MonoBehaviour where T : MonoSinglton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("The Manager is not availble !");

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _instance = (T) this;
    }
}
