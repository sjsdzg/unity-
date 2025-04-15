using UnityEngine;
using System.Collections;

namespace XFramework.UI
{
    public class Element : MonoBehaviour
    {
        public object Data { get; set; }

        void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }

        void Start()
        {
            OnStart();
        }

        protected virtual void OnStart()
        {

        }
    }
}

