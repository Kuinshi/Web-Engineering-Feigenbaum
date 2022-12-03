﻿using UnityEngine;

namespace Util
{
    public abstract class MBSingelton<T> : MonoBehaviour where T : MBSingelton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                    Debug.LogError($"Instance of {typeof(T)} is missing!");

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning($"Trying to create second Instance of {typeof(T)}. Destroying the new one!");
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
                Debug.Log($"Destroyed Instance of {typeof(T)}.");
            }
        }

        protected virtual void Init(){}
    }
}