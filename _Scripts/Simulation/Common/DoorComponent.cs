using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using HighlightingSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XFramework.Core;
using Simulation.Component;

namespace XFramework.Component
{
    /// <summary>
    /// 门组件
    /// </summary>
    public class DoorComponent : ToolTipComponent
    {
        public UnityEvent onOpened;
        /// <summary>
        /// 左
        /// </summary>
        //[SerializeField]
        public Transform m_Left;
        /// <summary>
        /// 右
        /// </summary>
        //[SerializeField]
        public Transform m_Right;

        //初始角度
        private Vector3 m_LeftRot;
        private Vector3 m_RightRot;

        /// <summary>
        /// 开度
        /// </summary>
        public Vector3 maxAngle = new Vector3(0,90,0);
        /// <summary>
        /// 过渡时间
        /// </summary>
        public float duration = 2f;

        /// <summary>
        /// 是否打开属性
        /// </summary>
        public bool IsOpened { get; set; }

        /// <summary>
        /// 碰撞器
        /// </summary>
        //[SerializeField]
        public  OnPointerComponent[] m_PointerComponents;

        /// <summary>
        /// 触发器
        /// </summary>
        [SerializeField]
        private OnCollisionComponent[] m_CollisionComponents;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Disable = false;

        void Awake()
        {
            h = transform.GetComponent<Highlighter>();

            if (m_Left != null)
                m_LeftRot = m_Left.eulerAngles;

            if (m_Right != null)
                m_RightRot = m_Right.eulerAngles;

            InitEvent();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            if (m_PointerComponents == null)
                return;

            for (int i = 0; i < m_PointerComponents.Length; i++)
            {
                OnPointerComponent component = m_PointerComponents[i];
                //鼠标点击
                component.OnMouseClick.AddListener(MouseClick);
                //鼠标进入
                component.OnMouseEnter.AddListener(MouseEnter);
                //鼠标退出
                component.OnMouseExit.AddListener(MouseExit);
            }
            if (m_CollisionComponents != null)
            {
                for (int i = 0; i < m_CollisionComponents.Length; i++)
                {
                    OnCollisionComponent colli = m_CollisionComponents[i];
                    colli.OnCollision.AddListener(isOn =>
                    {
                        Opening(isOn);
                        Debug.Log("触发了");
                    });
                }
            }
        }

        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="eventData"></param>
        private void MouseClick(PointerEventData eventData)
        {
            if (Disable)
                return;

            OnMouseClick(eventData);
        }

        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="eventData"></param>
        protected virtual void OnMouseClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Opening(!IsOpened);
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        private void MouseEnter()
        {
            if (Disable)
                return;

            OnPointerEnter(null);
        }

        /// <summary>
        /// 鼠标退出
        /// </summary>
        private void MouseExit()
        {
            if (Disable)
                return;

            OnPointerExit(null);
        }

        /// <summary>
        /// 打开中
        /// </summary>
        /// <param name="b"></param>
        public void Opening(bool b)
        {
            if (b)
            {
                if (m_Left != null)
                    m_Left.DORotate(m_LeftRot - maxAngle, duration).OnComplete(() => 
                    {
                        IsOpened = b;
                        onOpened.Invoke();
                    });

                if (m_Right != null)
                    m_Right.DORotate(m_RightRot + maxAngle, duration).OnComplete(() => 
                    {
                        IsOpened = b;
                        if (m_Left == null)
                        {
                            onOpened.Invoke();
                        }
                    });
            }
            else
            {
                if (m_Left != null)
                    m_Left.DORotate(m_LeftRot, duration).OnComplete(() => { IsOpened = b; });

                if (m_Right != null)
                    m_Right.DORotate(m_RightRot, duration).OnComplete(() => { IsOpened = b; });
            }
        }

    }
}
