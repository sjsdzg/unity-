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
    /// 实验详情
    /// </summary>
    public class EmpiricalDetailPanel : AbstractPanel
    {
        public const string COLUMN_Variable = "Variable";//变量
        private const string COLUMN_PRODUCT_RATE = "ProductRate";//生成效率
        public const string COLUMN_PRODUCT_AMOUNT = "ProductAmount";//生成量

        /// <summary>
        /// 背景
        /// </summary>
        private RectTransform background;

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.EmpiricalDetailPanel;
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
            m_Title = transform.Find("Background/Title/Text").GetComponent<Text>();
        }

        private void InitEvent()
        {
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonClose_onClick()
        {
            PanelManager.Instance.ClosePanel(EnumPanelType.EmpiricalDetailPanel);
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
                EmpiricalDataRecord empiricalDataRecord = PanelParams[0] as EmpiricalDataRecord;
                if (empiricalDataRecord != null)
                {
                    EmpiricalData empiricalData = EmpiricalData.Parser.ParseJson(empiricalDataRecord.Data);
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    switch (empiricalData.Type)
                    {
                        case EmpiricalDataType.Temperature:
                            m_Title.text = "温度对反应的影响";
                            keyValues.Add(COLUMN_Variable, "TEMP(℃)");
                            keyValues.Add(COLUMN_PRODUCT_RATE, "收益率X");
                            keyValues.Add(COLUMN_PRODUCT_AMOUNT, "M2(kg)");
                            break;
                        case EmpiricalDataType.Time:
                            m_Title.text = "时间对反应的影响";
                            keyValues.Add(COLUMN_Variable, "T(min)");
                            keyValues.Add(COLUMN_PRODUCT_RATE, "收益率X");
                            keyValues.Add(COLUMN_PRODUCT_AMOUNT, "M2(kg)");
                            break;
                        case EmpiricalDataType.Solvent:
                            m_Title.text = "溶剂对反应的影响";
                            keyValues.Add(COLUMN_Variable, "M1(kg)");
                            keyValues.Add(COLUMN_PRODUCT_RATE, "收益率X");
                            keyValues.Add(COLUMN_PRODUCT_AMOUNT, "M2(kg)");
                            break;
                        default:
                            break;
                    }

                    dataGridView.DataSource = BuildDataSource(empiricalData);
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
        private ObservableList<DataGridViewRowData> BuildDataSource(EmpiricalData empiricalData)
        {
            if (empiricalData == null)
                return null;

            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < empiricalData.Items.Count; i++)
            {
                EmpiricalItem empiricalItem = empiricalData.Items[i];
                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = empiricalItem;

                rowData.CellValueDict.Add(COLUMN_Variable, empiricalItem.Variable);
                rowData.CellValueDict.Add(COLUMN_PRODUCT_RATE, empiricalItem.ProductRate);
                rowData.CellValueDict.Add(COLUMN_PRODUCT_AMOUNT, empiricalItem.ProductAmount);

                dataSource.Add(rowData);
            }

            return dataSource;
        }

    }
}
