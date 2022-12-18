using Fusion;
using UnityEngine;

namespace Util
{
    public abstract class NBSingelton<T> : NetworkBehaviour where T : NBSingelton<T>
    {
        protected static T _instance;

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
            
            Init();
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