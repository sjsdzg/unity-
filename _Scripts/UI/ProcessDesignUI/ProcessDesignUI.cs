using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Module;
using System;
using XFramework.Diagram;
using XFramework.UIWidgets;
using Newtonsoft.Json;
using XFramework.Core;
using XFramework.Common;

namespace XFramework.UI
{
    public class ProcessDesignUI : MonoBehaviour
    {
        private ProcessLibraryData m_ProcessLibraryData;

        private ProcessLibraryPanel m_ProcessLibraryPanel;

        private InspectorView m_InspectorView;


        /// <summary>
        /// 流程设计结果面板
        /// </summary>
        private ProcessDesignReportPanel m_ProcessDesignReportPanel;

        /// <summary>
        /// 流程框图截图
        /// </summary>
        private GraphViewerScreenshot m_GraphViewerScreenshot;

        /// <summary>
        /// 图片查看器
        /// </summary>
        private ImageViewer m_ImageViewer;

        /// <summary>
        /// 预览按钮
        /// </summary>
        private Button buttonPreview;

        /// <summary>
        /// 撤销按钮
        /// </summary>
        private Button buttonUndo;

        /// <summary>
        /// 重做按钮
        /// </summary>
        private Button buttonRedo;

        /// <summary>
        /// 清空按钮
        /// </summary>
        private Button buttonClear;

        /// <summary>
        /// 删除按钮
        /// </summary>
        private Button buttonDelete;

        private void Awake()
        {
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            m_ProcessLibraryData = LoadProcessLibraryPanelData();
            m_ProcessLibraryData.ConvertVariablesType();

            m_ProcessLibraryPanel = transform.Find("Background/ProcessLibraryPanel").GetComponent<ProcessLibraryPanel>();
            m_ProcessLibraryPanel.SetData(m_ProcessLibraryData);

            InspectorManager.Instance.RegisterEditor<NodeEditor>();
            m_InspectorView = transform.Find("Background/InspectorView").GetComponent<InspectorView>();

            m_ProcessDesignReportPanel = transform.Find("Background/ProcessDesignReportPanel").GetComponent<ProcessDesignReportPanel>();
            m_GraphViewerScreenshot = transform.Find("Background/GraphViewer").GetComponent<GraphViewerScreenshot>();
            m_ImageViewer = transform.Find("Background/ImageViewer").GetComponent<ImageViewer>();

            buttonPreview = transform.Find("Background/TopBar/ContentRight/ButtonPreview").GetComponent<Button>();
            buttonUndo = transform.Find("Background/TopBar/ContentMiddle/ButtonUndo").GetComponent<Button>();
            buttonRedo = transform.Find("Background/TopBar/ContentMiddle/ButtonRedo").GetComponent<Button>();
            buttonClear = transform.Find("Background/TopBar/ContentMiddle/ButtonClear").GetComponent<Button>();
            buttonDelete = transform.Find("Background/TopBar/ContentMiddle/ButtonDelete").GetComponent<Button>();
        }

        private void InitEvent()
        {
            m_ProcessLibraryPanel.OnItemSelected.AddListener(m_ProcessLibraryPanel_OnItemSelected);
            m_ProcessLibraryPanel.OnItemBeginDrag.AddListener(m_ProcessLibraryPanel_OnItemBeginDrag);
            m_ProcessDesignReportPanel.OnImageView.AddListener(m_ProcessDesignReportPanel_OnImageView);

            GraphMaster.Instance.OnUnitSelected += GraphMaster_OnUnitSelected;
            GraphMaster.Instance.CurrentGraph.OnUnitAdded += Graph_OnUnitAdded;
            GraphMaster.Instance.CurrentGraph.OnUnitRemoved += GraphMaster_OnUnitRemoved;
            UndoManager.Instance.OnStatusChanged += UndoManager_OnStatusChanged;

            buttonPreview.onClick.AddListener(buttonPreview_onClick);
            buttonUndo.onClick.AddListener(buttonUndo_onClick);
            buttonRedo.onClick.AddListener(buttonRedo_onClick);
            buttonClear.onClick.AddListener(buttonClear_onClick);
            buttonDelete.onClick.AddListener(buttonDelete_onClick);
        }

        private void Start()
        {
            GraphMaster.Instance.Enable = true;
            m_ProcessDesignReportPanel.Hide();
            m_ImageViewer.Hide();
            buttonUndo.interactable = false;
            buttonRedo.interactable = false;
            buttonDelete.interactable = false;
        }

        private ProcessLibraryData LoadProcessLibraryPanelData()
        {
            string path = "ProcessDesign/ProcessLibrary";
            TextAsset asset = Resources.Load<TextAsset>(path);
            if (asset == null)
                throw new NullReferenceException("TextAsset is null");

            string json = asset.text;
            return JsonConvert.DeserializeObject<ProcessLibraryData>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }

        private void m_ProcessLibraryPanel_OnItemBeginDrag(ProcessLibraryItem arg0)
        {
            ProcessLibraryItemData data = arg0.Data;

            GraphMaster.Instance.ActiveTool = GraphMaster.Instance.GetTool<NodeCreateTool>();
            NodeCreateToolArgs t = new NodeCreateToolArgs(data.Name, data.SizeDelta);
            t.Variables = data.Variables;
            t.IsDrag = true;

            GraphMaster.Instance.ActiveTool.Init(t);
        }

        private void m_ProcessLibraryPanel_OnItemSelected(ProcessLibraryItem arg0)
        {
            ProcessLibraryItemData data = arg0.Data;

            GraphMaster.Instance.ActiveTool = GraphMaster.Instance.GetTool<NodeCreateTool>();
            NodeCreateToolArgs t = new NodeCreateToolArgs(data.Name, data.SizeDelta);
            t.Variables = data.Variables;
            t.IsDrag = false;

            GraphMaster.Instance.ActiveTool.Init(t);
        }


        private void GraphMaster_OnUnitSelected(Unit unit)
        {
            if (unit is Node)
                m_InspectorView.Inspect((Node)unit, unit.Name);
            else
                m_InspectorView.Inspect(null);

            if (unit != null)
                buttonDelete.interactable = true;
            else
                buttonDelete.interactable = false;
        }

        private void Graph_OnUnitAdded(Unit unit)
        {
            if (unit is Node)
            {
                var itemData = m_ProcessLibraryPanel.DataSource.Find(x => x.Name.Equals(unit.Name));
                m_ProcessLibraryPanel.DataSource.Remove(itemData);
            }
        }

        private void GraphMaster_OnUnitRemoved(Unit unit)
        {
            if (unit is Node)
            {
                var itemData = m_ProcessLibraryData.ItemDataList.Find(x => x.Name.Equals(unit.Name));
                m_ProcessLibraryPanel.DataSource.Add(itemData);
            }
        }

        private void UndoManager_OnStatusChanged()
        {
            buttonUndo.interactable = UndoManager.Instance.HasUndoRecords;
            buttonRedo.interactable = UndoManager.Instance.HasRedoRecords;
        }

        private void buttonPreview_onClick()
        {
            ProcessDesignReport report = new ProcessDesignReport();
            report.Init(m_ProcessLibraryData, GraphMaster.Instance.CurrentGraph);
            StartCoroutine(ShowDesignReport(report));
        }


        /// <summary>
        /// 显示设计报告
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowDesignReport(ProcessDesignReport report)
        {
            yield return m_GraphViewerScreenshot.Capture();
            report.UserGraph = m_GraphViewerScreenshot.Texture;
            m_ProcessDesignReportPanel.Show(report);
        }

        /// <summary>
        /// 显示大图
        /// </summary>
        /// <param name="arg0"></param>
        private void m_ProcessDesignReportPanel_OnImageView(Texture2D arg0)
        {
            m_ImageViewer.Show(arg0);
        }


        private void buttonUndo_onClick()
        {
            UndoManager.Instance.Undo();
        }

        private void buttonRedo_onClick()
        {
            UndoManager.Instance.Redo();
        }

        private void buttonClear_onClick()
        {
            MessageBoxEx.Show("清空操作会清除当前全部设计", "确认清空？", MessageBoxExEnum.CommonDialog, result =>
            {
                bool b = (bool)result.Content;
                if (b)
                {
                    GraphMaster.ClearGraph();
                }
            });
        }

        private void buttonDelete_onClick()
        {
            var selectedUnit = GraphMaster.Instance.currentSelectedUnit;
            if (selectedUnit == null)
                return;

            VisualManager.Instance.DestoryWidget(selectedUnit);
            GraphMaster.RemoveUnit(selectedUnit);
        }


    }
}                                                  