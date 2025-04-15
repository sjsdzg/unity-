using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using XFramework.Simulation;
using UnityEngine.EventSystems;

namespace XFramework.Common
{
    /// <summary>
    /// This component indicates that the game object is usable. This component works in
    /// conjunction with the Selector component. If you leave overrideName blank but there
    /// is an OverrideActorName component on the same object, this component will use
    /// the name specified in OverrideActorName.
    /// </summary>
    public class UsableComponent : MonoBehaviour
    {
        public enum Mode
        {
            /// <summary>
            /// 通过射线检测使用
            /// </summary>
            RayCastHit,
            /// <summary>
            /// 通过点击使用
            /// </summary>
            Click
        }

        public Mode mode;

        public UnityAction FocusCompoleteAction;

        CameraSwitcher m_CameraSwitcher;

        private UnityEvent m_OnUsable = new UnityEvent();
        /// <summary>
        /// 使用事件
        /// </summary>
        public UnityEvent OnUsable
        {
            get { return m_OnUsable; }
            set { m_OnUsable = value; }
        }

        private UnityEvent m_OnExit = new UnityEvent();
        /// <summary>
        /// 使用事件
        /// </summary>
        public UnityEvent OnExit
        {
            get { return m_OnExit; }
            set { m_OnExit = value; }
        }

        private bool disable = false;
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Disable
        {
            get { return disable; }
            set { disable = value; }
        }

        public bool IsUsing { get; set; }

        /// <summary>
        /// (Optional) Overrides the name shown by the Selector.
        /// </summary>
        public string useName;

        /// <summary>
        /// Overrides the use message shown by the Selector.
        /// </summary>
        public string useMessage;

        /// <summary>
        /// The max distance at which the object can be used.
        /// </summary>
        public float maxUseDistance = 5f;

        /// <summary>
        /// 用于点击的碰撞器
        /// </summary>
        public Collider collider;

        private void Awake()
        {
            m_CameraSwitcher = Camera.main.transform.GetComponent<CameraSwitcher>();
            if (mode == Mode.Click && collider != null)
            {
                collider.gameObject.TriggerAction(EventTriggerType.PointerClick, OnClick);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)&& IsUsing)
            {
                IsUsing = false;
                OnExit.Invoke();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnUse()
        {
            IsUsing = true;
            //if (OnUsable.GetPersistentEventCount() == 0)
            //{
            //    OnUsable.AddListener(() =>
            //    {
            //        FocusComponent m_FocusComponent = GetComponentInChildren<FocusComponent>();
            //        m_CameraSwitcher = Camera.main.transform.GetComponent<CameraSwitcher>();
            //        m_CameraSwitcher.Switch(CameraStyle.Focus);
            //        m_FocusComponent.Focus(FocusCompoleteAction);
            //    });
            //}
            OnUsable.Invoke();
        }
        private void OnClick(BaseEventData arg0)
        {
            if (mode != Mode.Click)
            {
                return;
            }
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData.button == 0)//InputButton.Left
            {
                OnUse();
            }
        }
    }
}

