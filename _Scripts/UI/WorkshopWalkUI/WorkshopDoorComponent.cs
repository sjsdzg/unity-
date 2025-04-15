using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using HighlightingSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XFramework.Component;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 车间门组件
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class WorkshopDoorComponent : MonoBehaviour
    {
        /// <summary>
        /// 左
        /// </summary>
        [SerializeField]
        private Transform m_Left;
        /// <summary>
        /// 右
        /// </summary>
        [SerializeField]
        private Transform m_Right;

        //初始角度
        private Vector3 m_LeftRot;
        private Vector3 m_RightRot;

        /// <summary>
        /// 提示内容
        /// </summary>
        public string catchToolTip = "门";

        /// <summary>
        /// 开度
        /// </summary>
        public float maxAngle = 90f;

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
        [SerializeField]
        private OnPointerComponent[] m_PointerComponents;

        /// 高亮组件
        /// </summary>
        private Highlighter h;

        /// <summary>
        /// 是否核实
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
                OnPointerComponent mouseDown = m_PointerComponents[i];
                mouseDown.OnMouseClick.AddListener(eventData =>
                {
                    if (eventData.button == PointerEventData.InputButton.Left)
                    {
                        if (!Disable)
                        {
                            Opening(!IsOpened);
                        }
                    }
                });

                mouseDown.OnMouseEnter.AddListener(() =>
                {
                    if (!Disable)
                    {
                        OnPointerEnter();
                    }
                });

                mouseDown.OnMouseExit.AddListener(() =>
                {
                    if (!Disable)
                    {
                        OnPointerExit();
                    }
                });
            }
        }

        /// <summary>
        /// 打开中
        /// </summary>
        /// <param name="b"></param>
        private void Opening(bool b)
        {
            if (b)
            {
                if (m_Left != null)
                    m_Left.DORotate(m_LeftRot + new Vector3(0, maxAngle, 0), duration).OnComplete(() => { IsOpened = b; });

                if (m_Right != null)
                    m_Right.DORotate(m_RightRot + new Vector3(0, -maxAngle, 0), duration).OnComplete(() => { IsOpened = b; });
            }
            else
            {
                if (m_Left != null)
                    m_Left.DORotate(m_LeftRot, duration).OnComplete(() => { IsOpened = b; });

                if (m_Right != null)
                    m_Right.DORotate(m_RightRot, duration).OnComplete(() => { IsOpened = b; });
            }
        }

        void OnPointerEnter()
        {
            h.ConstantOn();
        }

        void OnPointerExit()
        {
            h.ConstantOff();
        }
    }
}
