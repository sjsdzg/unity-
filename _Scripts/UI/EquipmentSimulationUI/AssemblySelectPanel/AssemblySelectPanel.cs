using UnityEngine;
using System.Collections;
using XFramework.UI;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using XFramework.Core;

namespace XFramework.UI
{
    public class AssemblySelectPanel : MonoBehaviour
    {
        private UniEvent<AssemblySelectPanel> m_OnEnter = new UniEvent<AssemblySelectPanel>();
        /// <summary>
        /// 进入拆装事件
        /// </summary>
        public UniEvent<AssemblySelectPanel> OnEnter
        {
            get { return m_OnEnter; }
            set { m_OnEnter = value; }
        }

        /// <summary>
        /// 组装/拆卸模式
        /// </summary>
        public AssemblyMode AssemblyMode { get; set; }

        /// <summary>
        /// 练习模式
        /// </summary>
        public PracticeMode PracticeMode { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 
        /// </summary>
        private Button buttonEnter;

        /// <summary>
        /// 
        /// </summary>
        private AssemblyModeGroup m_AssemblyModeGroup;

        /// <summary>
        /// 
        /// </summary>
        private PracticeModeGroup m_PracticeModeGroup;

        private void Awake()
        {
            m_Title = transform.Find("TitleBar/Text").GetComponent<Text>();
            buttonEnter = transform.Find("Bottom/ButtonEnter").GetComponent<Button>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();
            m_AssemblyModeGroup = transform.Find("Panel/AssemblyModeGroup").GetComponent<AssemblyModeGroup>();
            m_PracticeModeGroup = transform.Find("Panel/PracticeModeGroup").GetComponent<PracticeModeGroup>();
            // event
            m_AssemblyModeGroup.OnSelected.AddListener(m_AssemblyModeGroup_OnSelected);
            m_PracticeModeGroup.OnSelected.AddListener(m_PracticeModeGroup_OnSelected);
            buttonClose.onClick.AddListener(buttonClose_onClick);
            buttonEnter.onClick.AddListener(buttonEnter_onClick);
        }

        /// <summary>
        /// 拆装模式组选择时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_AssemblyModeGroup_OnSelected(AssemblyMode arg0)
        {
            AssemblyMode = arg0;
        }

        /// <summary>
        /// 练习模式选择时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_PracticeModeGroup_OnSelected(PracticeMode arg0)
        {
            PracticeMode = arg0;
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="stage"></param>
        public void Show(string name)
        {
            gameObject.SetActive(true);
            m_Title.text = name;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 关闭按钮点击时，触发
        /// </summary>
        private void buttonClose_onClick()
        {
            Hide();
        }

        private void buttonEnter_onClick()
        {
            Hide();
            OnEnter.Invoke(this);
        }
    }
}

