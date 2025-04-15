using UnityEngine;
using System.Collections;

namespace XFramework.Core
{
    public abstract class BaseUI : MonoBehaviour
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

        #region UIType & EnumObjectState
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
        /// Gets the type of the user interface.
        /// </summary>
        /// <returns>The user interface type.</returns>
        public abstract EnumUIType GetUIType();
        #endregion

        /// <summary>
		/// UI层级置顶
		/// </summary>
		protected virtual void SetDepthToTop()
        {

        }

        void Awake()
        {
            this.State = EnumObjectState.Initial;
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            this.State = EnumObjectState.Loading;
            //播放音乐
            this.OnPlayOpenUIAudio();
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
            this.OnPlayCloseUIAudio();
        }

        public void SetUIWhenOpening(params object[] uiParams)
        {
            SetUI(uiParams);
            StartCoroutine(AsyncOnLoadData());
        }

        protected virtual void SetUI(params object[] uiParams)
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
        protected virtual void OnPlayOpenUIAudio()
        {

        }


        /// <summary>
        /// 播放关闭界面音乐
        /// </summary>
        protected virtual void OnPlayCloseUIAudio()
        {

        }

        public virtual void SetUIparam(params object[] uiParams)
        {

        }
    }

}