using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;
using UnityEngine.Events;
using XFramework.Core;
using System;

namespace XFramework.UI
{
    public class ArchitectReportPanel : MultiListViewCustom<ArchitectReport, ArchitectScoreItem, ArchitectScoreItemData>
    {
        private UnityEvent m_OnBack = new UnityEvent();
        /// <summary>
        /// 返回按钮事件
        /// </summary>
        public UnityEvent OnBack
        {
            get { return m_OnBack; }
            set { m_OnBack = value; }
        }

        private UniEvent<Texture2D> m_OnImageView = new UniEvent<Texture2D>();
        /// <summary>
        /// 查看大图
        /// </summary>
        public UniEvent<Texture2D> OnImageView
        {
            get { return m_OnImageView; }
            set { m_OnImageView = value; }
        }

        /// <summary>
        /// 表格节点
        /// </summary>
        [SerializeField]
        private RectTransform m_Table;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 总分文本
        /// </summary>
        private Text m_TextTotalScore;

        /// <summary>
        /// 用户 RawImage
        /// </summary>
        private RawImage m_RawImageUser;

        /// <summary>
        /// 预览
        /// </summary>
        private Button buttonPreview;

        protected override void Awake()
        {
            base.Awake();
            buttonClose = transform.Find("Background/Header/ButtonClose").GetComponent<Button>();
            buttonBack = transform.Find("Background/ButtonBack").GetComponent<Button>();
            m_TextTotalScore = m_Table.Find("TotalScore/Score/Text").GetComponent<Text>();
            m_RawImageUser = m_Table.Find("Preview/User/ButtonPreview/Mask/RawImage").GetComponent<RawImage>();
            buttonPreview = m_Table.Find("Preview/User/ButtonPreview").GetComponent<Button>();
            buttonClose.onClick.AddListener(buttonClose_onClick);
            buttonBack.onClick.AddListener(buttonBack_onClick);
            buttonPreview.onClick.AddListener(buttonPreview_onClick);
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="title"></param>
        /// <param name="closeActive"></param>
        /// <param name="backActive"></param>
        public void SetState(bool closeActive, bool backActive)
        {
            buttonClose.gameObject.SetActive(closeActive);
            buttonBack.gameObject.SetActive(backActive);
        }

        public override void SetData(ArchitectReport data)
        {
            base.SetData(data);
            DataSource.Clear();
            if (data.ItemDataList != null)
            {
                DataSource.AddRange(data.ItemDataList);
            }
        }


        public void Show(ArchitectReport data)
        {
            gameObject.SetActive(true);
            m_RawImageUser.texture = data.UserGraph;
            m_TextTotalScore.text = data.TotalScore.ToString();
            SetData(data);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void buttonClose_onClick()
        {
            Hide();
        }

        private void buttonBack_onClick()
        {
            OnBack.Invoke();
        }


        private void buttonPreview_onClick()
        {
            OnImageView.Invoke(m_RawImageUser.texture as Texture2D);
        }

    }
}

