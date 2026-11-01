using UnityEngine;
namespace Bang.Lib.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static bool IsExist => _instance;

        public static T Instance => GetInstance();

        protected virtual void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this as T;
        }

        public static T GetInstance()
        {
            if (_instance) return _instance;

            _instance = FindFirstObjectByType<T>();

            if (_instance) return _instance;

            return _instance = new GameObject(typeof(T).Name).AddComponent<T>();
        }
    }

    public class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(transform.root);
        }
    }

}
