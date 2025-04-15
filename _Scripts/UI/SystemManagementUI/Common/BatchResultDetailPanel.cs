using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf.Collections;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;
using XFramework.Common;
using DG.Tweening;

namespace XFramework.UI
{
    /// <summary>
    /// 批量操作细节面板
    /// </summary>
    public class BatchResultDetailPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_TYPE = "Type";//日志类型
        public const string COLUMN_DETAIL = "Detail";//日志内容


        /// <summary>
        /// 
        /// </summary>
        private RectTransform background;

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.BatchResultDetailPanel;
        }


        /// <summary>
        /// 操作表格视图
        /// </summary>
        private DataGridView dataGridView;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            Parent.GetComponent<Image>().enabled = false;
        }

        private void InitGUI()
        {
            buttonClose = transform.Find("Background/Title/ButtonClose").GetComponent<Button>();
            dataGridView = transform.Find("Background/Panel/DataGridView").GetComponent<DataGridView>();
            background = transform.Find("Background").GetComponent<RectTransform>();
        }

        private void InitEvent()
        {
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonClose_onClick()
        {
            PanelManager.Instance.ClosePanel(EnumPanelType.BatchResultDetailPanel);
        }

        protected override void OnStart()
        {
            base.OnStart();
            background.DOScale(0, 0.3f).From();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                string json = PanelParams[0] as string;

                if (!string.IsNullOrEmpty(json))
                {
                    dataGridView.DataSource = BuildDataSource(json);
                }
            }
        }

        /// <summary>
        /// 接受获取题目的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetPaperResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            GetPaperResp resp = GetPaperResp.Parser.ParseFrom(packageInfo.Body);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="paperProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(string json)
        {
            BatchResultJson batchResult = BatchResultJson.Parser.ParseJson(json);
            if (batchResult == null)
                return null;

            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < batchResult.Items.Count; i++)
            {
                ResultJson batchItem = batchResult.Items[i];
                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = batchItem;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, (i + 1).ToString());
                //日志类型
                string comment = "";
                switch (batchItem.Type)
                {
                    case BatchResultJson.NORMAL:
                        comment = "<color=green>正确</color>";
                        break;
                    case BatchResultJson.WARNING:
                        comment = "<color=yellow>警告</color>";
                        break;
                    case BatchResultJson.ERROR:
                        comment = "<color=red>错误</color>";
                        break;
                    case BatchResultJson.EXCEPTION:
                        comment = "<color=red>错误</color>";
                        break;
                    default:
                        break;
                }
                rowData.CellValueDict.Add(COLUMN_TYPE, comment);
                //日志内容
                rowData.CellValueDict.Add(COLUMN_DETAIL, batchItem.Detail);

                dataSource.Add(rowData);
            }

            return dataSource;
        }

    }
}
