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
    public class OperationLogDetailPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_TYPE = "Type";//日志类型
        public const string COLUMN_LOG = "Log";//日志内容


        /// <summary>
        /// 
        /// </summary>
        private RectTransform background;

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.OperationLogDetailPanel;
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
            PanelManager.Instance.ClosePanel(EnumPanelType.OperationLogDetailPanel);
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
                UserOperationLog operationLog = PanelParams[0] as UserOperationLog;

                if (operationLog != null)
                {
                    dataGridView.DataSource = BuildDataSource(operationLog);
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
        private ObservableList<DataGridViewRowData> BuildDataSource(UserOperationLog userOperationLog)
        {
            LogData logData = LogData.Parser.ParseJson(userOperationLog.Data);
            if (logData == null)
                return null;

            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < logData.Items.Count; i++)
            {
                LogItemData logItem = logData.Items[i];
                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = logItem;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, (i + 1).ToString());
                //日志类型
                string logComment = "";
                switch (logItem.type)
                {
                    case LogType.Error:
                        logComment = "<color=red>错误</color>";
                        break;
                    case LogType.Assert:
                        logComment = "<color=red>错误</color>";
                        break;
                    case LogType.Warning:
                        logComment = "<color=yellow>警告</color>";
                        break;
                    case LogType.Log:
                        logComment = "<color=green>正确</color>";
                        break;
                    case LogType.Exception:
                        logComment = "<color=red>错误</color>";
                        break;
                    default:
                        break;
                }
                rowData.CellValueDict.Add(COLUMN_TYPE, logComment);
                //日志内容
                rowData.CellValueDict.Add(COLUMN_LOG, logItem.Log);

                dataSource.Add(rowData);
            }

            return dataSource;
        }

    }
}
