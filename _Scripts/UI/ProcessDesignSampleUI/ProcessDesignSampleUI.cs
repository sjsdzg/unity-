using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.UI
{
    public class ProcessDesignSampleUI : MonoBehaviour
    {
        /// <summary>
        /// 工艺流程数据
        /// </summary>
        private ProcessLibraryData m_ProcessLibraryData;

        /// <summary>
        /// 流程列表面板
        /// </summary>
        private ProcessSamplePanel m_ProcessSamplePanel;

        /// <summary>
        /// 流程放置面板
        /// </summary>
        private ProcessDropPanel m_ProcessDropPanel;

        /// <summary>
        /// 流程参数设置面板
        /// </summary>
        private ProcessParamSettingPanel m_ProcessParamSettingPanel;

        /// <summary>
        /// 工艺报告面板
        /// </summary>
        private ProcessReportSamplePanel m_ProcessReportSamplePanel;

        /// <summary>
        /// PDF 面板
        /// </summary>
        private PDFPanel m_PDFPanel;

        /// <summary>
        /// 图标列表
        /// </summary>
        private ImageList m_ImageList;

        /// <summary>
        /// 帮助按钮
        /// </summary>
        private Button buttonHelp;

        /// <summary>
        /// 提交按钮
        /// </summary>
        private Button buttonSubmit;

        private void Awake()
        {
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            m_ImageList = transform.Find("Background/ImageList").GetComponent<ImageList>();
            m_ProcessSamplePanel = transform.Find("Background/ProcessSamplePanel").GetComponent<ProcessSamplePanel>();
            m_ProcessDropPanel = transform.Find("Background/ProcessDropPanel").GetComponent<ProcessDropPanel>();
            m_ProcessParamSettingPanel = transform.Find("Background/ProcessParamSettingPanel").GetComponent<ProcessParamSettingPanel>();
            m_ProcessReportSamplePanel = transform.Find("Background/ProcessReportSamplePanel").GetComponent<ProcessReportSamplePanel>();
            m_PDFPanel = transform.Find("Background/PDFPanel").GetComponent<PDFPanel>();
            buttonHelp = transform.Find("Background/TopBar/ButtonHelp").GetComponent<Button>();
            buttonSubmit = transform.Find("Background/TopBar/ButtonSubmit").GetComponent<Button>();
        }


        private void InitEvent()
        {
            m_ProcessDropPanel.OnItemSelected.AddListener(m_ProcessDropPanel_OnItemSelected);
           // m_ProcessDropPanel.OnItemEnqueued.AddListener(m_ProcessDropPanel_OnItemEnqueued);
           // m_ProcessDropPanel.OnItemDequeued.AddListener(m_ProcessDropPanel_OnItemDequeued);
            m_ProcessReportSamplePanel.OnBack.AddListener(m_ProcessReportSamplePanel_OnBack);
            buttonHelp.onClick.AddListener(buttonHelp_onClick);
            buttonSubmit.onClick.AddListener(buttonSubmit_onClick);
        }

        private void Start()
        {
            if (GlobalManager.DefaultMode == Simulation.ProductionMode.Examine)
            {
                buttonHelp.gameObject.SetActive(false);
                buttonSubmit.GetComponentInChildren<Text>().text = "提交";
                m_ProcessDropPanel.SetState("（考试模式）");
            }
            else
            {
                buttonSubmit.GetComponentInChildren<Text>().text = "评价";
                m_ProcessDropPanel.SetState("（学习模式）");
            }

            m_ProcessLibraryData = LoadProcessLibraryData();
            m_ProcessLibraryData.ConvertVariablesType();
            foreach (var itemData in m_ProcessLibraryData.ItemDataList)
            {
                itemData.Sprite = m_ImageList[itemData.Name];
            }

            m_ProcessSamplePanel.SetData(m_ProcessLibraryData);
            m_ProcessReportSamplePanel.Hide();
            m_PDFPanel.Hide();
        }

        private ProcessLibraryData LoadProcessLibraryData()
        {
            string path = "ProcessDesign/ProcessLibrary";
            TextAsset asset = Resources.Load<TextAsset>(path);
            if (asset == null)
                throw new NullReferenceException("TextAsset is null");

            string json = asset.text;


            return JsonConvert.DeserializeObject<ProcessLibraryData>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }


        private void m_ProcessDropPanel_OnItemSelected(ProcessDropItem arg0)
        {
            m_ProcessParamSettingPanel.SetData(arg0.Data);
        }


        private void m_ProcessDropPanel_OnItemEnqueued(ProcessDropItem arg0)
        {
            m_ProcessSamplePanel.HideItem(arg0.Data);
        }

        private void m_ProcessDropPanel_OnItemDequeued(ProcessDropItem arg0)
        {
            m_ProcessSamplePanel.ShowItem(arg0.Data);
            if (m_ProcessDropPanel.SelectedItem.Equals(arg0))
            {
                m_ProcessParamSettingPanel.SetData(null);
            }
        }

        /// <summary>
        /// 提交按钮点击时，触发
        /// </summary>
        private void buttonSubmit_onClick()
        {
            ProcessSampleReport report = new ProcessSampleReport();
            report.Init(m_ProcessLibraryData, m_ProcessDropPanel.Items);
            m_ProcessReportSamplePanel.Show(report);

            if (GlobalManager.DefaultMode == Simulation.ProductionMode.Examine)
                m_ProcessReportSamplePanel.SetState("设计结果", false, true);
            else
                m_ProcessReportSamplePanel.SetState("设计结果", true, true);
        }

        /// <summary>
        /// 帮助按钮点击时，触发
        /// </summary>
        private void buttonHelp_onClick()
        {
            //ProcessSampleReport report = new ProcessSampleReport();
            //report.Init(m_ProcessLibraryData, m_ProcessDropPanel.Items);
            //m_ProcessReportSamplePanel.Show(report);
            //m_ProcessReportSamplePanel.SetState("帮助", true, false);
            string path = AppSettings.Settings.AssetServerUrl + "帮助文档/流程设计帮助文档.pdf";
            m_PDFPanel.LoadDocumentFromWeb(path, "帮助文档");
        }

        /// <summary>
        /// 报告返回按钮点击时，触发
        /// </summary>
        private void m_ProcessReportSamplePanel_OnBack()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }

    }

}
