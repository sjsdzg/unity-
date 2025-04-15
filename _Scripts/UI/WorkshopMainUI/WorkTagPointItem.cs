using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using HighlightingSystem;

namespace XFramework.UI
{
    /// <summary>
    /// 生产车间 流程 标识点
    /// </summary>
	public class WorkTagPointItem : MonoBehaviour {

        /// <summary>
        /// 时间间隔
        /// </summary>
        public float Interval = 0.4f;

        /// <summary>
        /// 缩放
        /// </summary>
        public float scale = 1f;

        ///// <summary>
        ///// 工艺点类型
        ///// </summary>
        //public WorkPointType type = StagePointType.Arrow;

        /// <summary>
        /// 工艺按钮
        /// </summary>
        private Button button;

        private Highlighter h;

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
            transform.localScale = Vector3.zero;

            button = transform.Find("Button").GetComponent<Button>();
            //h = transform.FindChild("OutLine").GetComponent<Highlighter>();

            button.onClick.AddListener(button_onClick);

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
        /// 显示高亮
        /// </summary>
        public void ShowHighlight()
        {
            print("___:" + name + "  高亮");
            if(h==null)
            {
                Transform objOutLine = transform.Find("OutLine");
                h = objOutLine.GetComponent<Highlighter>();
            }
            else
            {

            }
            h.ConstantOn(Color.green);
        }
    }

}
