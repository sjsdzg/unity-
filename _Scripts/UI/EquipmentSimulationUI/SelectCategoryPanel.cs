using UnityEngine;
using System.Collections;
using DG.Tweening;
using XFramework.Core;
using UnityEngine.UI;
using System;

namespace XFramework.UI
{
    public class SelectCategoryPanel : MonoBehaviour
    {
        private CanvasGroup m_CanvasGroup;

        private UniEvent<int> m_OnSelected = new UniEvent<int>();
        /// <summary>
        /// 选择事件
        /// </summary>
        public UniEvent<int> OnSelected
        {
            get { return m_OnSelected; }
            set { m_OnSelected = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private Button[] m_Buttons;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; private set; }

        void Awake()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
            m_Buttons = GetComponentsInChildren<Button>();
            IsShow = true;

            for (int i = 0; i < m_Buttons.Length; i++)
            {
                int index = i + 1;
                Button button = m_Buttons[i];
                button.onClick.AddListener(() => button_onClick(index));
            }
        }


        /// <summary>
        /// 点击时，触发事件
        /// </summary>
        /// <param name="index"></param>
        private void button_onClick(int index)
        {
            OnSelected.Invoke(index);
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            m_CanvasGroup.DOFade(1, 0.5f);
            m_CanvasGroup.blocksRaycasts = true;
            IsShow = true;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            m_CanvasGroup.DOFade(0, 0.5f).OnComplete(() => 
            {
                m_CanvasGroup.blocksRaycasts = false;
                gameObject.SetActive(false);
            });
            IsShow = false;
        }
    }
}

