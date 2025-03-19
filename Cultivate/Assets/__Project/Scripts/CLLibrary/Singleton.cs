
using UnityEngine;

namespace CLLibrary
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance => _instance;

        public void Awake()
        {
            CheckAwake();
        }

        private bool _hasAwoken;

        public void CheckAwake()
        {
            if (_hasAwoken)
                return;
            _hasAwoken = true;
            AwakeFunction();
        }

        protected virtual void AwakeFunction()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
        }

        private void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}
