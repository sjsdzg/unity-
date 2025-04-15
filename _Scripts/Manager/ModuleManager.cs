using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;

namespace XFramework
{
	public class ModuleManager : Singleton<ModuleManager>
	{
        private Dictionary<string, BaseModule> dicModules = null;

        protected override void Init()
        {
            dicModules = new Dictionary<string, BaseModule>();

            RegisterALL();
        }

        /// <summary>
        /// 注册所有模块
        /// </summary>
        public void RegisterALL()
        {
            //Register<SimuMainModule>();
        }

        #region get module
        /// <summary>
        /// get module by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BaseModule GetModule(string key)
        {
            if (dicModules.ContainsKey(key))
                return dicModules[key];
            return null;
        }
        /// <summary>
        /// get module by module type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModule<T>() where T : BaseModule
        {
            Type t = typeof(T);
            if (dicModules.ContainsKey(t.ToString()))
                return dicModules[t.ToString()] as T;
            return null;
        }
        #endregion

        #region register module
        /// <summary>
        /// Register the module by module Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Register<T>()
        {
            Type t = typeof(T);
            BaseModule module = System.Activator.CreateInstance(t) as BaseModule;
            Register(t.ToString(), module);
        }
        /// <summary>
        /// register the module by key & module
        /// </summary>
        /// <param name="key"></param>
        /// <param name="module"></param>
        private void Register(string key, BaseModule module)
        {
            if (!dicModules.ContainsKey(key))
            {
                module.Load();
                dicModules.Add(key, module);
            }
        }
        #endregion

        #region Unregister Module
        /// <summary>
        /// Unregister the module by module type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Unregister<T>()
        {
            Type t = typeof(T);
            Unregister(t.ToString());
        }
        /// <summary>
        /// Unregister the module by key
        /// </summary>
        /// <param name="key"></param>
        private void Unregister(string key)
        {
            if (dicModules.ContainsKey(key))
            {
                BaseModule module = dicModules[key];
                module.Release();
                dicModules.Remove(key);
            }
        }
        /// <summary>
        /// Unregister all
        /// </summary>
        public void UnregisterAll()
        {
            List<string> _keyList = new List<string>(dicModules.Keys);
            for (int i = 0; i < _keyList.Count; i++)
            {
                Unregister(_keyList[i]);
            }
            dicModules.Clear();
        }
        #endregion

    }
}
