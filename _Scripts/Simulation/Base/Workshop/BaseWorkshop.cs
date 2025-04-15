using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework;
using XFramework.Core;

namespace XFramework.Simulation
{
    /// <summary>
    /// 车间基类
    /// </summary>
    public abstract class BaseWorkshop : MonoBehaviour
    {
        private Transform cachedTransform;
        /// <summary>
        /// Gets the cached transform.
        /// </summary>
        /// <value>The cached transform.</value>
        public Transform CachedTransform
        {
            get
            {
                if (!cachedTransform)
                {
                    cachedTransform = this.transform;
                }
                return cachedTransform;
            }
        }

        private UnityEngine.GameObject cachedGameObject;
        /// <summary>
        /// Gets the cached game object.
        /// </summary>
        /// <value>The cached game object.</value>
        public GameObject CachedGameObject
        {
            get
            {
                if (!cachedGameObject)
                {
                    cachedGameObject = this.gameObject;
                }
                return cachedGameObject;
            }
        }

        /// <summary>
        /// The state.
        /// </summary>
        protected EnumObjectState state = EnumObjectState.None;

        /// <summary>
        /// Occurs when state changed.
        /// </summary>
        public event StateChangedEventHandler StateChanged;

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public EnumObjectState State
        {
            protected set
            {
                if (value != state)
                {
                    EnumObjectState oldState = state;
                    state = value;
                    if (null != StateChanged)
                    {
                        StateChanged(this, state, oldState);
                    }
                }
            }
            get { return this.state; }
        }

        /// <summary>
        /// 获取房间类型
        /// </summary>
        /// <returns></returns>
        public abstract EnumWorkshopType GetWorkshopType();

        void Awake()
        {
            this.State = EnumObjectState.Initial;
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            this.State = EnumObjectState.Loading;
        }

        void Start()
        {
            OnStart();
        }

        protected virtual void OnStart()
        {

        }

        void Update()
        {

            if (EnumObjectState.Ready == this.state)
            {
                OnUpdate(Time.deltaTime);
            }
        }

        protected virtual void OnUpdate(float deltaTime)
        {

        }

        public void Release()
        {
            this.State = EnumObjectState.Closing;
            OnRelease();
            UnityEngine.GameObject.Destroy((UnityEngine.Object)CachedGameObject);
        }

        /// <summary>
        /// 释放结束之后的操作
        /// </summary>
        protected virtual void OnRelease()
        {
            
        }

        /// <summary>
        /// 当车间加载时，设置参数
        /// </summary>
        /// <param name="uiParams"></param>
        public void SetWorkshopWhenLoading(params object[] workshopParams)
        {
            SetWorkshop(workshopParams);
            StartCoroutine(AsyncOnLoadData());
        }

        protected virtual void SetWorkshop(params object[] workshopParams)
        {
            this.State = EnumObjectState.Loading;
        }

        private IEnumerator AsyncOnLoadData()
        {
            yield return new WaitForEndOfFrame();
            if (this.State == EnumObjectState.Loading)
            {
                this.OnLoadData();
                this.State = EnumObjectState.Ready;
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        protected virtual void OnLoadData()
        {

        }

        /// <summary>
        /// 设置工艺参数
        /// </summary>
        /// <param name="uiParams"></param>
        public virtual void SetWorkshopParams(params object[] uiParams)
        {

        }
    }
}
