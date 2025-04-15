using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    public abstract class Singleton<T> where T : class, new()
    {
        
        /// <summary>
        /// The instance.
        /// </summary>
        protected static T _instance = null;

        private static object lockObject = new object();

        /// <summary>
        /// gets the instance
        /// </summary>
        public static T Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock(lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initializes
        /// </summary>
        protected Singleton()
        {
            if (_instance != null)
                throw new System.Exception(string.Format("单例已被实例化过:{0}", typeof(T)));

            Init();
        }

        /// <summary>
        /// Init this Singleton
        /// </summary>
        protected virtual void Init()
        {

        }

        public virtual void Release()
        {
            _instance = null;
        }
    }
}
