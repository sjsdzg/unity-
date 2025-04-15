using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Core
{
    public class Dispatcher : DDOLSingleton<Dispatcher>
    {
        private static readonly Queue<Action> actions = new Queue<Action>();

        //private static Dispatcher instance;
        ///// <summary>
        ///// unity main thread dispatcher.
        ///// </summary>
        //public static Dispatcher Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            throw new Exception("Could not find the Dispatcher GameObject, Please ensure you have added this script to an empty GameObject in your scene.");
        //        }
        //        return instance;
        //    }
        //}

        //private void Awake()
        //{
        //    if (instance == null)
        //    {
        //        instance = this;
        //        DontDestroyOnLoad(gameObject);
        //    }
        //}

        private void Update()
        {
            lock(actions)
            {
                while (actions.Count > 0)
                {
                    actions.Dequeue().Invoke();
                }
            }
        }

        public void Invoke(Action action)
        {
            lock(actions)
            {
                actions.Enqueue(action);
            }
        }

        public void Invoke(IEnumerator action)
        {
            lock(actions)
            {
                actions.Enqueue(() => StartCoroutine(action));
            }
        }
    }
}
