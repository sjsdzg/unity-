using UnityEngine;
using System.Collections;
using XFramework.Module;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 故障视图栏
    /// </summary>
    public class FaultInfoViewBar : MonoBehaviour
    {
        public class OnNameClickedEvent : UnityEvent<FaultNamePanel> { }

        private OnNameClickedEvent m_OnNameClicked = new OnNameClickedEvent();
        /// <summary>
        /// 故障名称点击事件
        /// </summary>
        public OnNameClickedEvent OnNameClicked
        {
            get { return m_OnNameClicked; }
            set { m_OnNameClicked = value; }
        }

        public class OnCauseClickedEvent : UnityEvent<FaultCausePanel> { }
        /// <summary>
        /// 故障名称或原因选择错误或未选择的次数
        /// </summary>
        //private int num = 0;

        private OnCauseClickedEvent m_OnCauseClicked = new OnCauseClickedEvent();
        /// <summary>
        /// 故障原因点击事件
        /// </summary>
        public OnCauseClickedEvent OnCauseClicked
        {
            get { return m_OnCauseClicked; }
            set { m_OnCauseClicked = value; }
        }

        /// <summary>
        /// 故障现象面板
        /// </summary>
        private FaultPhenomenaPanel m_FaultPhenomenaPanel;

        /// <summary>
        /// 故障名称面板
        /// </summary>
        private FaultNamePanel m_FaultNamePanel;

        /// <summary>
        /// 故障原因面板
        /// </summary>
        private FaultCausePanel m_FaultCausePanel;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 提交按钮
        /// </summary>
        private Button buttonSubmit;

        private FaultInfo defaultFaultInfo;
        /// <summary>
        /// 正确故障信息
        /// </summary>
        public FaultInfo DefaultFaultInfo
        {
            get { return defaultFaultInfo; }
            set
            {
                defaultFaultInfo = value;
                m_FaultPhenomenaPanel.DefaultFaultInfo = defaultFaultInfo;
                m_FaultNamePanel.DefaultFaultInfo = defaultFaultInfo;
                m_FaultCausePanel.DefaultFaultInfo = defaultFaultInfo;
            }
        }

        private void Awake()
        {
            m_FaultPhenomenaPanel = transform.Find("ScrollView/Viewport/Content/FaultPhenomenaPanel").GetComponent<FaultPhenomenaPanel>();
            m_FaultNamePanel = transform.Find("ScrollView/Viewport/Content/FaultNamePanel").GetComponent<FaultNamePanel>();
            m_FaultCausePanel = transform.Find("ScrollView/Viewport/Content/FaultCausePanel").GetComponent<FaultCausePanel>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();
            buttonSubmit = transform.Find("Bottom/ButtonSubmit").GetComponent<Button>();
            m_FaultNamePanel.OnClicked.AddListener(m_FaultNamePanel_OnClicked);
            m_FaultCausePanel.OnClicked.AddListener(m_FaultCausePanel_OnClicked);
            buttonClose.onClick.AddListener(buttonClose_onClick);
            buttonSubmit.onClick.AddListener(buttonSubmit_onClick);
        }


        /// <summary>
        /// 关闭按钮点击时，触发
        /// </summary>
        private void buttonClose_onClick()
        {
            Hide();
        }

        /// <summary>
        /// 故障名称面板点击时，触发
        /// </summary>
        /// <param name="panel"></param>
        private void m_FaultNamePanel_OnClicked(FaultNamePanel panel)
        {
            OnNameClicked.Invoke(panel);
        }

        /// <summary>
        /// 故障原因面板点击时，触发
        /// </summary>
        /// <param name="panel"></param>
        private void m_FaultCausePanel_OnClicked(FaultCausePanel panel)
        {
            OnCauseClicked.Invoke(panel);
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 添加故障现象
        /// </summary>
        /// <param name="item"></param>
        public void AddFaultPhenomena(string name)
        {
            FaultPhenomena item = DefaultFaultInfo.FaultPhenomenas.Find(x => x.ID == name);
            m_FaultPhenomenaPanel.AddItem(item);
        }

        /// <summary>
        /// 移除故障现象
        /// </summary>
        /// <param name="item"></param>
        public void RemoveFaultPhenomena(FaultPhenomena item)
        {
            m_FaultPhenomenaPanel.RemoveItem(item.ID);
        }

        /// <summary>
        /// 确认按钮点击时，触发
        /// </summary>
        private void buttonSubmit_onClick()
        {
            //EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "故障名称："+defaultFaultInfo.Name+"\n故障现象："+defaultFaultInfo.GetFaultPhenomena(defaultFaultInfo.ID), LogType.Log);
            if (!m_FaultPhenomenaPanel.IsMatchPhenomena())
            {
                ShowHUDText(Utils.NewGameObject().transform, "故障现象未查找完成。", Color.red);
            }
            else if (!m_FaultNamePanel.IsMatchName())
            {
                ShowHUDText(Utils.NewGameObject().transform, "故障名称未选择正确。", Color.red);
            }
            else if (!m_FaultCausePanel.IsMatchCauses())
            {
                ShowHUDText(Utils.NewGameObject().transform, "故障原因未选择正确。", Color.red);
            }
            else
            {
                EventDispatcher.ExecuteEvent(Events.Fault.NameCompleted);
                EventDispatcher.ExecuteEvent(Events.Fault.CauseCompleted);
            }
            Hide();
        }


        /// <summary>
        /// 显示提示文本
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="text"></param>
        protected void ShowHUDText(Transform trans, string text, Color color, float Offset = 0f)
        {
            HUDTextInfo info = new HUDTextInfo(trans, text);
            info.Color = color;
            info.Size = 24;
            info.Speed = UnityEngine.Random.Range(10, 20);
            info.VerticalAceleration = 1;
            info.VerticalPositionOffset = Offset;
            info.VerticalFactorScale = UnityEngine.Random.Range(1.2f, 3);
            info.Side = bl_Guidance.Right;
            info.FadeSpeed = 250;
            info.ExtraDelayTime = 2;
            info.FixedFontSize = true;
            info.AnimationType = bl_HUDText.TextAnimationType.HorizontalSmall;
            //Send the information
            bl_UHTUtils.GetHUDText.NewText(info);
        }
    }
}

