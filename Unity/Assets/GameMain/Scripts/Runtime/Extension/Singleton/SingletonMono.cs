using UnityEngine;

namespace GameMain
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T sInstance;

        public static T Instance
        {
            get
            {
                if (sInstance == null)
                {
                    var gameObject = new GameObject(typeof(T).Name);
                    sInstance = gameObject.AddComponent<T>();
                    DontDestroyOnLoad(sInstance);
                }

                return sInstance;
            }
        }

        protected virtual void Awake()
        {
            if (sInstance == null)
            {
                sInstance = this as T;
            }
        }

        public virtual void OnInit()
        {
            
        }

        public virtual void OnClear()
        {
            if (sInstance != null)
            {
                Destroy(sInstance.gameObject);
                sInstance = null;
            }
        }
    }
}