
using UnityEngine;

namespace CLLibrary
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance => _instance;

        public void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            DidAwake();
        }

        public virtual void DidAwake()
        {
        }

        private void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}
