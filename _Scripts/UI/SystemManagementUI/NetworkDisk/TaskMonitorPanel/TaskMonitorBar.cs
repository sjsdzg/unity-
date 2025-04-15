using UnityEngine;
using System.Collections;
using XFramework.Common;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace XFramework.UI
{
    public class TaskMonitorBar : ListViewCustom<TaskMonitorItem, TaskMonitorItemData>
    {
        /// <summary>
        /// 任务详情
        /// </summary>
        private Text m_TaskText;

        /// <summary>
        /// 是否展开
        /// </summary>
        private Toggle m_ToggleUnfold;

        /// <summary>
        /// 关闭所有任务按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 自身RectTransform
        /// </summary>
        private RectTransform m_Self;

        protected override void Awake()
        {
            base.Awake();
            m_TaskText = transform.Find("TitleBar/TaskText").GetComponent<Text>();
            m_ToggleUnfold = transform.Find("TitleBar/ToggleUnfold").GetComponent<Toggle>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();
            m_Self = transform.GetComponent<RectTransform>();
            // Event
            m_ToggleUnfold.onValueChanged.AddListener(m_ToggleUnfold_onValueChanged);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        /// <summary>
        /// 是否展开View
        /// </summary>
        /// <param name="flag"></param>
        private void Unfold(bool flag = true)
        {
            if (flag) // 展开
            {
                m_Self.DOSizeDelta(new Vector2(m_Self.sizeDelta.x, 340f), 0.3f);
            }
            else //折叠
            {
                m_Self.DOSizeDelta(new Vector2(m_Self.sizeDelta.x, 48f), 0.3f);
            }
        }

        /// <summary>
        /// 展开Toggle点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_ToggleUnfold_onValueChanged(bool arg0)
        {
            RectTransform image = m_ToggleUnfold.transform.Find("Image").GetComponent<RectTransform>();
            if (arg0)
            {
                image.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                image.localScale = new Vector3(1, 1, 1);
            }
            Unfold(arg0);
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void buttonClose_onClick()
        {
            bool isNotDone = Items.Exists(x => x.IsDone = false);
            if (isNotDone)
            {
                MessageBoxEx.Show("您还有任务在进行中，确定要清除吗", "提示", MessageBoxExEnum.CommonDialog, x =>
                {
                    bool flag = (bool)x.Content;
                    if (flag)
                    {
                        Items.ForEach(y => y.OnAbort());
                        DataSource.Clear();
                        Hide();
                    }
                });
            }
            else
            {
                DataSource.Clear();
                Hide();
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 中止所有任务
        /// </summary>
        public void AbortAll()
        {
            Items.ForEach(x => x.OnAbort());
        }
    }
}

