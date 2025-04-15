using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Simulation;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺点组件
    /// </summary>
    public class StagePointComponent : MonoBehaviour
    {
        /// <summary>
        /// 时间间隔
        /// </summary>
        public float Interval = 0.4f;

        /// <summary>
        /// 缩放
        /// </summary>
        public float scale = 1f;

        /// <summary>
        /// 工艺点类型
        /// </summary>
        public StagePointType type = StagePointType.Arrow;

        /// <summary>
        /// 工艺按钮
        /// </summary>
        private Button button;

        /// <summary>
        /// 
        /// </summary>
        private Text text;

        /// <summary>
        /// 上锁
        /// </summary>
        private Transform m_Lock;

        public class OnClickedEvent : UnityEvent<string> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }


        private Tween m_Tween;

        void Awake()
        {
            //transform.localScale = Vector3.zero;

            if (type == StagePointType.Stage)
            {
                m_Lock = transform.Find("Lock").GetComponent<Transform>();
                button = transform.Find("Button").GetComponent<Button>();
                text = transform.Find("Button/Text").GetComponent<Text>();
                button.onClick.AddListener(button_onClick);
                //更改
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                switch (GlobalManager.DefaultMode)
                {
                    case ProductionMode.Study:
                    case ProductionMode.Examine:
                        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 124);
                        m_Lock.gameObject.SetActive(false);
                        string context = text.text;
                        context = context.Substring(context.IndexOf("\n") + 1);
                        text.text = context;
                        break;
                    case ProductionMode.Banditos:
                        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 210);
                        m_Lock.gameObject.SetActive(true);
                        break;
                    default:
                        break;
                }
            }

            //m_Tween = transform.DOScale(0, 0.1f);
            //m_Tween.SetAutoKill(false);
            //m_Tween.Pause();
        }

        /// <summary>
        /// 按钮点击时，触发
        /// </summary>
        private void button_onClick()
        {
            string name = button.GetComponentInChildren<Text>().text;
            OnClicked.Invoke(name);
        }

        public void Appear()
        {
            transform.DOScale(scale, 0.5f);
        }

        public void Disappear()
        {
            transform.DOKill();

            transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// 设置锁状态
        /// </summary>
        /// <param name="state"></param>
        public void SetLockState(bool state)
        {
            m_Lock.gameObject.SetActive(state);
        }
    }

    /// <summary>
    /// 工艺点类型
    /// </summary>
    public enum StagePointType
    {
        /// <summary>
        /// 箭头
        /// </summary>
        Arrow,
        /// <summary>
        /// 工艺
        /// </summary>
        Stage,
    }
}
