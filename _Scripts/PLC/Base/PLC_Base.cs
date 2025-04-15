using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.UI;

namespace XFramework.PLC
{
    public abstract class PLC_Base : MonoBehaviour
    {
        #region Cached gameObject & transfrom

        private Transform cachedTransform;
        /// <summary>0
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

        private GameObject cachedGameObject;
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
        #endregion

        #region PLC_Type & EnumObjectState
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
        /// Gets the type of the PLC.
        /// </summary>
        /// <returns>The user interface type.</returns>
        public abstract PLC_Type GetPLC_Type();
        #endregion

        void Awake()
        {
            this.State = EnumObjectState.Initial;

            OnAwake();
        }

        public virtual void OnAwake()
        {
            this.State = EnumObjectState.Loading;
            //播放动画
            //this.OnPlayOpenPLC_Anim();
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
            GameObject.Destroy(CachedGameObject);
            OnRelease();
        }

        protected virtual void OnRelease()
        {
            this.OnPlayClosePLC_Anim();
        }

        public void SetPLC_WhenOpening(params object[] PLC_Params)
        {
            SetPLC(PLC_Params);
            StartCoroutine(AsyncOnLoadData());
        }

        protected virtual void SetPLC(params object[] PLC_Params)
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

        protected virtual void OnLoadData()
        {

        }

        /// <summary>
        /// 播放PLC打开动画
        /// </summary>
        protected virtual void OnPlayOpenPLC_Anim()
        {

        }

        /// <summary>
        /// 播放PLC关闭动画
        /// </summary>
        protected virtual void OnPlayClosePLC_Anim()
        {

        }

        public virtual void SetPLC_Param(params object[] PLC_Params)
        {

        }
    }
}
