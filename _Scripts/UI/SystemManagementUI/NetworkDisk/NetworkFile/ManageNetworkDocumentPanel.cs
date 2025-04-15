using UnityEngine;
using System.Collections;
using XFramework.Module;
using XFramework.Common;
using System.Collections.Generic;
using XFramework.Core;
using UIWidgets;
using System;
using XFramework.Core;
using Crosstales.FB;

namespace XFramework.UI
{
    public class ManageNetworkDocumentPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_NAME = "Name";//名称列
        public const string COLUMN_CREATETIME = "CreateTime";//创建时间列

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManageNetworkDocumentPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 网络文件模块
        /// </summary>
        private NetworkFileModule networkFileModule;

        /// <summary>
        /// 表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 文件查询栏目
        /// </summary>
        private NetworkFileQueryBar queryBar;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 
        /// </summary>
        private ImageList m_ImageList;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            pageDataGrid = transform.Find("PageDataGrid").GetComponent<PageDataGrid>();
            batchActionBar = transform.Find("BatchActionBar").GetComponent<BatchActionBar>();
            queryBar = transform.Find("QueryBar").GetComponent<NetworkFileQueryBar>();
            m_ImageList = transform.Find("ImageList").GetComponent<ImageList>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            batchActionBar.ButtonCellClick.AddListener(batchActionBar_ButtonCellClick);
            queryBar.OnQuery.AddListener(userQueryBar_OnQuery);
        }

        /// <summary>
        /// 获取文件图标
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private Sprite GetSprite(NetworkFile file)
        {
            string key = file.Type + "_type";
            if (!(m_ImageList.Exists(key)))
            {
                if (file.IsDocument())
                {
                    key = "general_type";
                }
                else if (file.IsImage())
                {
                    key = "img_type";
                }
                else if (file.IsAudio())
                {
                    key = "music_type";
                }
                else if (file.IsVideo())
                {
                    key = "video_type";
                }
                else if (file.IsCompress())
                {
                    key = "rar_type";
                }
                else
                {
                    key = "other_type";
                }
            }
            return m_ImageList[key];
        }

        protected override void OnStart()
        {
            base.OnStart();
            networkFileModule = ModuleManager.Instance.GetModule<NetworkFileModule>();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageEmpiricalPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageNetworkDocumentPanel));
            queryBar.SetFileTypes(NetworkFile.GetDocTypes());
            //分页查询
            WebRequestOperation async = networkFileModule.PageNetworkFileByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions);
            async.OnCompleted(PageNetworkFile_OnCompleted);
        }

        /// <summary>
        /// 地址栏点击时，触发
        /// </summary>
        /// <param name="type"></param>
        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        /// <summary>
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            //分页查询
            WebRequestOperation async = networkFileModule.PageNetworkFileByCondition(currentPage, pageSize, queryBar.SqlConditions);
            async.OnCompleted(PageNetworkFile_OnCompleted);
        }

        /// <summary>
        /// 查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void userQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            WebRequestOperation async = networkFileModule.PageNetworkFileByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions);
            async.OnCompleted(PageNetworkFile_OnCompleted);
        }

        private void PageNetworkFile_OnCompleted(AsyncLoadOperation arg0)
        {
            if (!string.IsNullOrEmpty(arg0.Error))
            {
                Debug.Log(arg0.Error);
                return;
            }

            WebRequestOperation operation = arg0 as WebRequestOperation;
            string json = operation.GetText();
            Debug.Log(json);
            PageBeanJson<NetworkFile> pageBeanJson = PageBeanJson<NetworkFile>.Parser.ParseJson(json);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = pageBeanJson.CurrentPage;
            pageBean.PageSize = pageBeanJson.PageSize;
            pageBean.TotalPages = pageBeanJson.TotalPages;
            pageBean.TotalRecords = pageBeanJson.TotalRecords;

            if (pageBeanJson.dataList != null)
            {
                pageBean.DataList = BuildDataSource(pageBeanJson.dataList);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }


        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="userProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(List<NetworkFile> networkFiles)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < networkFiles.Count; i++)
            {
                NetworkFile networkFile = networkFiles[i];
                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = networkFile;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());
                //实验数据名称
                rowData.CellValueDict.Add(COLUMN_NAME, networkFile.Name);
                //开始时间
                rowData.CellValueDict.Add(COLUMN_CREATETIME, DateTimeUtil.ToString(networkFile.CreateTime));
                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            NetworkFile networkFile = row.Data.Tag as NetworkFile;
            switch (type)
            {
                case ButtonCellType.Download:
                    // 保存文件夹路径
                    string saveDir = FileBrowser.OpenSingleFolder("保存文件", "");
                    if (string.IsNullOrEmpty(saveDir))
                        return;

                    // 下载
                    DownloadNetworkFileOperation download = networkFileModule.DownloadNetworkFile(networkFile, saveDir);
                    // 发送监控内容
                    TaskMonitorItemData monitorItem = new TaskMonitorItemData();
                    monitorItem.Async = download;
                    monitorItem.FileIcon = GetSprite(networkFile);
                    EventDispatcher.ExecuteEvent<TaskMonitorItemData>(Events.TaskMonitor.Add, monitorItem);
                    // 下载完成
                    download.OnCompleted(y =>
                    {
                        if (!string.IsNullOrEmpty(download.Error))
                        {
                            Debug.LogError(download.Error);
                            return;
                        }
                        // 添加到文件列表
                    });
                    break;
                case ButtonCellType.Delete:
                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            WebRequestOperation async = networkFileModule.Remove(networkFile.Id);
                            async.OnCompleted(y =>
                            {
                                if (!string.IsNullOrEmpty(y.Error))
                                {
                                    Debug.LogError(y.Error);
                                    return;
                                }
                                string json = async.GetText();
                                Debug.Log(json);
                                ResultJson result = ResultJson.Parser.ParseJson(json);
                                if (result.Type == ResultJson.NORMAL)
                                {
                                    WebRequestOperation pageAsync = networkFileModule.PageNetworkFileByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions);
                                    pageAsync.OnCompleted(PageNetworkFile_OnCompleted);
                                }
                                else
                                {
                                    Debug.LogWarning(result.Detail);
                                }
                            });
                        }
                    });
                    break;
                default:
                    break;
            }
        }

        private void batchActionBar_ButtonCellClick(BatchActionBar arg0, ButtonCellType type)
        {
            List<DataGridViewRow> rows = pageDataGrid.GetRowsByChecked();
            switch (type)
            {
                case ButtonCellType.BatchDelete:
                    MessageBoxEx.Show("<color=red>您确定要删除这" + rows.Count + "个文件吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            foreach (var row in rows)
                            {
                                NetworkFile networkFile = row.Data.Tag as NetworkFile;
                                WebRequestOperation async = networkFileModule.Remove(networkFile.Id);
                                async.OnCompleted(y =>
                                {
                                    if (!string.IsNullOrEmpty(y.Error))
                                    {
                                        Debug.LogError(y.Error);
                                        return;
                                    }
                                    string json = async.GetText();
                                    Debug.Log(json);
                                    ResultJson result = ResultJson.Parser.ParseJson(json);
                                    if (result.Type == ResultJson.NORMAL)
                                    {
                                        WebRequestOperation pageAsync = networkFileModule.PageNetworkFileByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions);
                                        pageAsync.OnCompleted(PageNetworkFile_OnCompleted);
                                    }
                                    else
                                    {
                                        Debug.LogWarning(result.Detail);
                                    }
                                });
                            }
                        }
                    });
                    break;
                case ButtonCellType.Download:
                    // 保存文件夹路径
                    string saveDir = FileBrowser.OpenSingleFolder("保存文件", "");
                    if (string.IsNullOrEmpty(saveDir))
                        return;

                    foreach (var row in rows)
                    {
                        NetworkFile networkFile = row.Data.Tag as NetworkFile;
                        // 下载
                        DownloadNetworkFileOperation download = networkFileModule.DownloadNetworkFile(networkFile, saveDir);
                        // 发送监控内容
                        TaskMonitorItemData monitorItem = new TaskMonitorItemData();
                        monitorItem.Async = download;
                        monitorItem.FileIcon = GetSprite(networkFile);
                        EventDispatcher.ExecuteEvent<TaskMonitorItemData>(Events.TaskMonitor.Add, monitorItem);
                        // 下载完成
                        download.OnCompleted(y =>
                        {
                            if (!string.IsNullOrEmpty(download.Error))
                            {
                                Debug.LogError(download.Error);
                                return;
                            }
                            // 添加到文件列表
                        });
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
