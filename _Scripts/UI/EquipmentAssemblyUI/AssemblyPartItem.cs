using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using XFramework.Module;
using UnityEngine.Events;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 设备部件Item
    /// </summary>
    public class AssemblyPartItem : Element, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// 组装/拆卸模式
        /// </summary>
        public AssemblyMode AssemblyMode { get; set; }

        /// <summary>
        /// 占空位
        /// </summary>
        private GameObject fakeObject = null;

        /// <summary>
        /// 拖拽的父物体
        /// </summary>
        private Transform dragableObjectParent = null;

        /// <summary>
        /// 触动事件类
        /// </summary>
        public class TouchEvent : UnityEvent<AssemblyPartItem> { }

        /// <summary>
        /// 图标
        /// </summary>
        [HideInInspector]
        public Image m_Image;

        /// <summary>
        /// 文本
        /// </summary>
        [HideInInspector]
        public Text m_Text;

        private TouchEvent m_OnTouch = new TouchEvent();
        /// <summary>
        /// 触动事件
        /// </summary>
        public TouchEvent OnTouch
        {
            get { return m_OnTouch; }
            set { m_OnTouch = value; }
        }

        /// <summary>
        /// 计时器
        /// </summary>
        private float m_Timer = 0;

        /// <summary>
        /// 延迟时间
        /// </summary>
        public float delay = 1f;

        /// <summary>
        /// 
        /// </summary>
        private Vector3 mousePosLast = Vector3.zero;

        /// <summary>
        /// 指针按下
        /// </summary>
        private bool isPointerDown = false;

        /// <summary>
        /// 是否触发
        /// </summary>
        private bool isTouch = false;

        /// <summary>
        /// ScrollRect
        /// </summary>
        public ScrollRect m_ScrollRect;

        protected override void OnAwake()
        {
            base.OnAwake();
            m_Image = transform.Find("Image").GetComponent<Image>();
            m_Text = transform.Find("Text").GetComponent<Text>();
        }

        void Update()
        {
            if (AssemblyMode == AssemblyMode.Disassembly)// 拆卸模式
                return;

            if (!isPointerDown)
                return;

            if (Input.GetMouseButton(0))
            {
                if (Vector3.Distance(Input.mousePosition, mousePosLast) > 0.1f)
                {
                    m_Timer = 0;
                    if (Input.mousePosition.x - mousePosLast.x > 5f)
                    {
                        OnTouchHandler();
                    }
                }
                else
                {
                    m_Timer += Time.deltaTime;
                    if (m_Timer > delay)
                    {
                        OnTouchHandler();
                        m_Timer = 0;
                    }
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            mousePosLast = Input.mousePosition;
            isPointerDown = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EquipmentPart data = Data as EquipmentPart;
            ToolTip.Instance.Show(true, 0.5f, data.Description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ToolTip.Instance.Show(false);
            isPointerDown = false;
        }

        /// <summary>
        /// 触发操作
        /// </summary>
        public void OnTouchHandler()
        {
            if (isTouch)
                return;

            fakeObject = new GameObject();
            LayoutElement layoutElement = fakeObject.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            layoutElement.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            layoutElement.flexibleWidth = 0;
            layoutElement.flexibleHeight = 0;
            fakeObject.transform.SetParent(this.transform.parent);
            fakeObject.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            dragableObjectParent = this.transform.parent;
            this.transform.SetParent(m_ScrollRect.transform.parent);
            this.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
            this.GetOrAddComponent<CanvasGroup>().alpha = 0;
            //触动时
            OnTouch.Invoke(this);
            m_ScrollRect.vertical = false;
            isTouch = true;
        }

        public void OnDropHandler(bool succeed = true)
        {
            if (succeed)
            {
                gameObject.SetActive(false);
                Destroy(fakeObject);
            }
            else
            {
                this.transform.SetParent(dragableObjectParent);
                this.transform.SetSiblingIndex(fakeObject.transform.GetSiblingIndex());
                this.GetComponent<CanvasGroup>().blocksRaycasts = true;
                this.GetComponent<CanvasGroup>().alpha = 1;
                Destroy(fakeObject);
            }

            ToolTip.Instance.Hide();
            m_ScrollRect.vertical = true;
            isTouch = false;
        }
    }
}
