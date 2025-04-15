using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using XFramework.Core;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.UI
{
    public class SequenceToggle : MonoBehaviour
    {
        /// <summary>
        /// 图标
        /// </summary>
        private RectTransform m_Icon;

        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text;
        /// <summary>
        /// 序号
        /// </summary>
        private Text m_NumText;
        private Button m_NumButton;

        private bool isOpen;
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                Toggle(isOpen);
            }
        }

        void Awake()
        {
            m_Icon = transform.Find("Icon").GetComponent<RectTransform>();
            m_Text = transform.Find("Text").GetComponent<Text>();
            m_NumButton = transform.Find("Num").GetComponent<Button>();
            m_NumText = transform.Find("Num/Text").GetComponent<Text>();
            m_NumButton.onClick.AddListener(m_NumButton_onClick);
        }

        private void m_NumButton_onClick()
        {
           
        }

        private void Toggle(bool _isOpen)
        {
            if (_isOpen)
            {
                m_Icon.DOLocalRotate(new Vector3(0, 0, -90), 0.2f);
                if (GlobalManager.ArchiteIntroduceModule)
                    return;
                MessageBoxEx.Show("是否跳转到当前操作流程？", "提示", MessageBoxExEnum.CommonDialog, result =>
                {
                    bool b = (bool) result.Content;                    
                    if (b)
                    {
                        int index = int.Parse(m_NumText.text);
                        EventDispatcher.ExecuteEvent<int>("选择操作流程", index);
                    }

                });
            }
            else
            {
                m_Icon.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            }
        }

        public void SetValue(string number, string text)
        {
            m_NumText.text = number;
            m_Text.text = text;
        }
    }
}
