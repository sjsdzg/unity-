using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Core;
using XFramework.UI;

namespace XFramework.UI
{
    public abstract class AbstractPanel : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<EnumPanelType> { }

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

        #region PanelType & EnumObjectState
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

        public abstract EnumPanelType GetPanelType();
        #endregion

        /// <summary>
        /// 父
        /// </summary>
        public Transform Parent { get; set; }

        void Awake()
        {
            this.State = EnumObjectState.Initial;
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            this.State = EnumObjectState.Loading;
            //播放音乐
            this.OnPlayOpenPanelAudio();
        }

        void Start()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            GameObject.Destroy(CachedGameObject);
            OnRelease();
        }

        protected virtual void OnRelease()
        {
            this.OnPlayClosePanelAudio();
        }

        public void SetPanelWhenOpening(params object[] PanelParams)
        {
            SetPanel(PanelParams);
            StartCoroutine(AsyncOnLoadData());
        }

        protected virtual void SetPanel(params object[] PanelParams)
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
        /// 播放打开界面音乐
        /// </summary>
        protected virtual void OnPlayOpenPanelAudio()
        {

        }


        /// <summary>
        /// 播放关闭界面音乐
        /// </summary>
        protected virtual void OnPlayClosePanelAudio()
        {

        }

        public virtual void SetPanelparam(params object[] PanelParams)
        {

        }
    }
}
