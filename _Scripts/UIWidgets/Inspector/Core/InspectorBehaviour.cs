using UnityEngine;
using System.Collections;

namespace XFramework.UIWidgets
{
    public abstract class InspectorBehaviour : MonoBehaviour
    {
        public abstract string GetKey();

        protected virtual void Awake()
        {

        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void Start()
        {

        }

        public virtual void Unbind()
        {
            InspectorManager.Instance.Destory(this);
        }
    }
}

