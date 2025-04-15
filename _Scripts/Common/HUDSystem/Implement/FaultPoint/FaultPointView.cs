using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace XFramework.Common
{
    /// <summary>
    /// 指示器视图
    /// </summary>
    public class FaultPointView : HUDView
    {
        /// <summary>
        /// Image
        /// </summary>
        public Image m_Icon;
        /// <summary>
        /// 文本
        /// </summary>
        public Text m_Text;

        /// <summary>
        /// 提示标志
        /// </summary>
        private bool tip = false;

        /// <summary>
        /// 指示器信息
        /// </summary>
        private FaultPointInfo FaultPointInfo;

        /// <summary>
        /// 发现按钮
        /// </summary>
        private Button buttonFind;

        private UnityEvent m_OnClicked = new UnityEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public UnityEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// HUDInfo属性
        /// </summary>
        public override HUDInfo HUDInfo
        {
            get  { return FaultPointInfo; }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            //m_Sequence = DOTween.Sequence();
            //m_Sequence.Append(m_Icon.rectTransform.DOScale(1.2f, 1).SetLoops(-1, LoopType.Yoyo));
            //m_Sequence.Pause();
            buttonFind = transform.Find("Content/ButtonFind").GetComponent<Button>();
            m_Icon.rectTransform.DOScale(1.2f, 1).SetLoops(-1, LoopType.Yoyo);
            buttonFind.onClick.AddListener(buttonFind_onClick);
        }

        private void buttonFind_onClick()
        {
            OnClicked.Invoke();
            buttonFind.gameObject.SetActive(false);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="hudInfo"></param>
        public override void Initialize(HUDInfo hudInfo)
        {
            base.Initialize(hudInfo);
            FaultPointInfo = hudInfo as FaultPointInfo;
        }

        /// <summary>
        /// 在屏幕内显示
        /// </summary>
        protected override void OnScreenHandler()
        {
            if (FaultPointInfo.onScreenArgs.visible)
            {
                gameObject.SetActive(true);
                //Icon
                m_Icon.sprite = FaultPointInfo.onScreenArgs.m_Sprite;
                m_Icon.color = FaultPointInfo.onScreenArgs.m_Color;
                //Text
                m_Text.transform.parent.parent.gameObject.SetActive(true);
                m_Text.text = FaultPointInfo.onScreenArgs.m_Text;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 在屏幕外显示
        /// </summary>
        protected override void OffScreenHandler()
        {
            if (FaultPointInfo.offScreenArgs.visible)
            {
                gameObject.SetActive(true);
                m_Icon.sprite = FaultPointInfo.offScreenArgs.m_Sprite;
                //设置颜色
                m_Icon.color = FaultPointInfo.offScreenArgs.m_Color;
                //隐藏文本和距离文本
                m_Text.transform.parent.parent.gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
