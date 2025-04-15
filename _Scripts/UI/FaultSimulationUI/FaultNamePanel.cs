using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using XFramework.Module;
using UnityEngine.UI;
using UnityEngine.Events;
using XFramework.Common;

namespace XFramework.UI
{
    public class FaultNamePanel : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<FaultNamePanel> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// 故障名称
        /// </summary>
        private Text textName;

        /// <summary>
        /// 选中按钮
        /// </summary>
        private Button selectButton;

        /// <summary>
        /// 正确故障信息
        /// </summary>
        public FaultInfo DefaultFaultInfo { get; set; }

        /// <summary>
        /// 故障信息
        /// </summary>
        public FaultInfo FaultInfo { get; set; }
        void Awake()
        {
            textName = transform.Find("Content/Name/Text").GetComponent<Text>();
            selectButton = transform.Find("TitleBar/SelectButton").GetComponent<Button>();
            selectButton.onClick.AddListener(selectButton_onClick);
        }

        public void SetValue(FaultInfo faultInfo)
        {
            FaultInfo = faultInfo;
            textName.text = FaultInfo.Name;
        }

        /// <summary>
        /// 选中按钮点击时，触发
        /// </summary>
        private void selectButton_onClick()
        {
            OnClicked.Invoke(this);
        }

        /// <summary>
        /// 故障名称是否匹配
        /// </summary>
        /// <returns></returns>
        public bool IsMatchName()
        {
            bool isMatch = false;
            if (FaultInfo != null)
            {
                if (FaultInfo.ID == DefaultFaultInfo.ID)
                {
                    isMatch = true;
                }
            }
            return isMatch;
        }
    }
}

