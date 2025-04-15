using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;
using System;
using XFramework.Core;

namespace XFramework.UI
{
    public class ProcessDesignReportPanel : MultiListViewCustom<ProcessDesignReport, ProcessParamGroup, ProcessParamGroupData>
    {
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
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 总分文本
        /// </summary>
        private Text m_TextTotalScore;

        /// <summary>
        /// 流程框图预览项
        /// </summary>
        private ProcessGraphPreview m_ProcessGraphPreview;

        protected override void Awake()
        {
            base.Awake();
            buttonClose = transform.Find("Header/ButtonClose").GetComponent<Button>();
            m_TextTotalScore = transform.Find("Scroll View/Viewport/Content/Table/TotalScore/Score/Text").GetComponent<Text>();
            m_ProcessGraphPreview = transform.Find("Scroll View/Viewport/Content/Table/ProcessGraphPreview").GetComponent<ProcessGraphPreview>();

            buttonClose.onClick.AddListener(buttonClose_onClick);
            m_ProcessGraphPreview.OnImageView.AddListener(m_ProcessGraphPreview_OnImageView);
        }

        public override void SetData(ProcessDesignReport data)
        {
            base.SetData(data);
            DataSource.Clear();
            if (data.GroupDataList != null)
            {
                DataSource.AddRange(data.GroupDataList);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="data"></param>
        public void Show(ProcessDesignReport data)
        {
            gameObject.SetActive(true);

            data.StandardGraph = m_ProcessGraphPreview.StandardGraph;
            SetData(data);
            m_ProcessGraphPreview.Init(data.UserGraph, data.GraphScore);
            m_TextTotalScore.text = data.TotalScore.ToString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void buttonClose_onClick()
        {
            Hide();
        }

        private void m_ProcessGraphPreview_OnImageView(Texture2D arg0)
        {
            OnImageView.Invoke(arg0);
        }

    }
}

