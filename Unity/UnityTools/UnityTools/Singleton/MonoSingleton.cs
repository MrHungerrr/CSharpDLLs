using UnityEngine;

namespace Vkimow.Unity.Tools.Single
{
    public abstract class MonoSingleton<T>: MonoBehaviour where T: MonoBehaviour
    {

        private static T _instance;
        private static object _lock = new object();


        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = GameObject.FindObjectOfType<T>();

                        if (GameObject.FindObjectsOfType<T>().Length > 1)
                        {
                            Debug.LogError($"На сцене найдено несколько {typeof(T)}");
                        }

                        if (_instance == null)
                        {
                            var singleton = new GameObject($"[SINGLETON] {typeof(T)}");
                            _instance = singleton.AddComponent<T>();
                            DontDestroyOnLoad(singleton);
                            Debug.LogError($"На сцене отсутсвует [SINGLETON] {typeof(T)}");
                        }
                    }

                    return _instance;
                }
            }
        }
    }
}
