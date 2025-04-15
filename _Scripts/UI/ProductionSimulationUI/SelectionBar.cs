using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
    public class SelectionBar : MonoBehaviour, IDisplay
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        private CanvasGroup m_CanvasGroup;

        /// <summary>
        /// Text Name
        /// </summary>
        private Text m_TextName;

        /// <summary>
        /// text
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 选择器
        /// </summary>
        private Common.Selector m_Selector = null;

        /// <summary>
        /// usable
        /// </summary>
        private UsableComponent m_Usable = null;

        /// <summary>
        /// 是否显示
        /// </summary>
        private bool visible = false;

        /// <summary>
        /// 过渡时间
        /// </summary>
        public float duration = 0.2f;



        protected float CurrentDistance
        {
            get
            {
                return (m_Selector != null) ? m_Selector.CurrentDistance : 0;
            }
        }

        void Start()
        {
            m_CanvasGroup = transform.GetComponent<CanvasGroup>();
            m_CanvasGroup.alpha = 0;
            m_TextName = transform.Find("KeyCode/Text").GetComponent<Text>();
            m_Text = transform.Find("Image/Text").GetComponent<Text>();
            //m_Selector = GameObject.FindObjectOfType<Selector>();
            //if (m_Selector != null)
            //{
            //    m_Selector.OnSelected.AddListener(m_Selector_OnSelected);
            //    m_Selector.OnDeselected.AddListener(m_Selector_OnDeselected);
            //}
        }

        //private void m_Selector_OnSelected(UsableComponent usable)
        //{
        //    m_Usable = usable;
        //    m_Text.text = usable.useMessage;
        //    Debug.LogFormat("selector select gameObject name : [{0}]", usable.gameObject.name);
        //}

        //private void m_Selector_OnDeselected(UsableComponent usable)
        //{
        //    m_Usable = null;
        //    m_Text.text = "";
        //}

        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void OnSelected(string name, string message)
        {
            m_TextName.text = name;
            m_Text.text = message;
            Show();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDeselected()
        {
            m_Usable = null;
            m_Text.text = "";
            Hide();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            if (!visible)
            {
                m_CanvasGroup.alpha = 0;
                m_CanvasGroup.DOFade(1, duration);
                visible = true;
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            if (visible)
            {
                m_CanvasGroup.alpha = 1;
                m_CanvasGroup.DOFade(0, duration);
                visible = false;
            }
        }

        /// <summary>
        /// 在Usable范围内
        /// </summary>
        /// <returns></returns>
        private bool IsUsableInRange()
        {
            return (m_Usable != null) && (CurrentDistance <= m_Usable.maxUseDistance);
        }

        private void Update()
        {
            //if (IsUsableInRange())
            //{
            //    Show();
            //}
            //else
            //{
            //    Hide();
            //}
        }
    }
}
