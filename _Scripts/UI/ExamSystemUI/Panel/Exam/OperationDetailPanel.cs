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
    public class OperationDetailPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_DESC = "Desc";//操作详情
        public const string COLUMN_VALUE = "Value";//操作分值
        public const string COLUMN_SCORE = "Score";//操作得分

        /// <summary>
        /// 
        /// </summary>
        private RectTransform background;

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.OperationDetailPanel;
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
            PanelManager.Instance.ClosePanel(EnumPanelType.OperationDetailPanel);
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
                List<OperationPointJson> operationPointJsons = PanelParams[0] as List<OperationPointJson>;

                if (operationPointJsons != null)
                {
                    dataGridView.DataSource = BuildDataSource(operationPointJsons);
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
        private ObservableList<DataGridViewRowData> BuildDataSource(List<OperationPointJson> OperationPointJsons)
        {
            if (OperationPointJsons == null)
                return null;

            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < OperationPointJsons.Count; i++)
            {
                OperationPointJson pointJson = OperationPointJsons[i];
                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = pointJson;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, pointJson.Id.ToString());
                //操作关键点
                rowData.CellValueDict.Add(COLUMN_DESC, pointJson.Desc);
                //试卷名称
                rowData.CellValueDict.Add(COLUMN_VALUE, pointJson.Value);

                rowData.CellValueDict.Add(COLUMN_SCORE, pointJson.Score);//Switch

                dataSource.Add(rowData);
            }

            return dataSource;
        }

    }
}
