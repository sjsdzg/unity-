using UnityEngine;
using System.Collections;

namespace XFramework.Core
{
    public abstract class DDOLSingleton<T> : MonoBehaviour where T : DDOLSingleton<T>
    {
        protected static T _instance = null;
        public static T Instance
        {
            get
            {
                if (null == _instance)
                {
                    GameObject go = GameObject.Find("DDOLGameObject");
                    if (null == go)
                    {
                        go = new GameObject("DDOLGameObject");
                        DontDestroyOnLoad(go);
                    }

                    _instance = go.GetComponent<T>();
                    if (_instance == null)
                    {
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
}