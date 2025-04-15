using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf.Collections;
using UIWidgets;
using UnityEngine;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Proto;
using XFramework.Simulation;
using XFramework.Network;
using Newtonsoft.Json;
using XFramework.Core;
using Crosstales.FB;

namespace XFramework.UI
{
    /// <summary>
    /// 在线用户管理Panel
    /// </summary>
    public class ManageNetworkFilePanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManageNetworkFilePanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 网络磁盘模块
        /// </summary>
        private NetworkDiskModule networkDiskModule;

        /// <summary>
        /// 网络文件模块
        /// </summary>
        private NetworkFileModule networkFileModule;

        /// <summary>
        /// 
        /// </summary>
        private NetworkFileListView m_NetworkFileListView;

        /// <summary>
        /// 
        /// </summary>
        private ImageList m_ImageList;

        /// <summary>
        /// 对应磁盘
        /// </summary>
        private NetworkDisk m_NetworkDisk;

        /// <summary>
        /// 工具栏
        /// </summary>
        private ToolBar m_ToolBar;

        /// <summary>
        /// 当前ParentId
        /// </summary>
        private string directoryId;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            m_NetworkFileListView = transform.Find("NetworkFileBar").GetComponent<NetworkFileListView>();
            m_ImageList = transform.Find("ImageList").GetComponent<ImageList>();
            m_NetworkFileListView.DoubleItemClick.AddListener(m_NetworkFileListView_DoubleItemClick);
            m_ToolBar = transform.Find("ToolBar").GetComponent<ToolBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            m_ToolBar.ButtonCellClick.AddListener(m_ToolBar_ButtonCellClick);
            m_NetworkFileListView.OnItemSubmit.AddListener(m_NetworkFileListView_OnItemSubmit);
            m_NetworkFileListView.OnAddressClicked.AddListener(m_NetworkFileListView_OnAddressClicked);
        }

        protected override void OnStart()
        {
            base.OnStart();
            networkDiskModule = ModuleManager.Instance.GetModule<NetworkDiskModule>();
            networkFileModule = ModuleManager.Instance.GetModule<NetworkFileModule>();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageNetworkFilePanel, PanelDefine.GetPanelComment(EnumPanelType.ManageNetworkFilePanel));
            //获取在线用户
            WebRequestOperation diskRequest = networkDiskModule.GetByName(App.Instance.SoftwareId);
            diskRequest.OnCompletedEvent.AddListener(diskRequest_OnCompleted);
        }

        private void diskRequest_OnCompleted(AsyncLoadOperation arg0)
        {
            if (!string.IsNullOrEmpty(arg0.Error))
            {
                Debug.LogError(arg0.Error);
                return;
            }
            WebRequestOperation request = arg0 as WebRequestOperation;
            string json = request.GetText();
            m_NetworkDisk = NetworkDisk.Parser.ParseJson(json);
            directoryId = m_NetworkDisk.HomeId;
            // 根目录
            NetworkFile root = new NetworkFile();
            root.Id = m_NetworkDisk.HomeId;
            root.Name = "资料";
            m_NetworkFileListView.AddAddressButton(root);
            AccessDirectory(m_NetworkDisk.HomeId);
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

        /// <summary>
        /// 地址栏点击时，触发
        /// </summary>
        /// <param name="type"></param>
        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }


        private void m_NetworkFileListView_DoubleItemClick(NetworkFileItemData data)
        {
            NetworkFile file = data.NetworkFile;

            if (file.Type.Equals("dir"))
            {
                m_NetworkFileListView.AddAddressButton(file);
                AccessDirectory(file.Id);
            }
        }

        private void m_NetworkFileListView_OnItemSubmit(NetworkFileItem arg0)
        {
            NetworkFile file = arg0.Data.NetworkFile;
            WebRequestOperation async = networkFileModule.Rename(file);
            async.OnCompleted(x =>
            {
                if (!string.IsNullOrEmpty(x.Error))
                {
                    Debug.LogError(x.Error);
                    return;
                }
                // 获取新建文件夹请求结果
                string json = async.GetText();
                ResultJson result = ResultJson.Parser.ParseJson(json);
                if (result.Type == ResultJson.NORMAL)
                {
                    Debug.Log(result.Detail);
                }
                else
                {
                    Debug.LogError(result.Detail);
                }
            });
        }

        /// <summary>
        /// ListView 地址栏点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_NetworkFileListView_OnAddressClicked(NetworkFile arg0)
        {
            AccessDirectory(arg0.Id);
        }

        /// <summary>
        /// 进入目录
        /// </summary>
        private void AccessDirectory(string dirId)
        {
            directoryId = dirId;
            WebRequestOperation request = networkFileModule.List(dirId);
            request.OnCompletedEvent.AddListener(AccessDirectory_OnCompleted);
        }

        private void AccessDirectory_OnCompleted(AsyncLoadOperation arg0)
        {
            if (!string.IsNullOrEmpty(arg0.Error))
            {
                Debug.LogError(arg0.Error);
                return;
            }
            WebRequestOperation request = arg0 as WebRequestOperation;
            string json = request.GetText();
            List<NetworkFile> networkFiles = NetworkFile.Parser.ParseJsonToList(json);
            m_NetworkFileListView.DataSource = new ObservableCollection<NetworkFileItemData>();
            foreach (var file in networkFiles)
            {
                NetworkFileItemData itemData = new NetworkFileItemData();
                itemData.NetworkFile = file;
                itemData.FileIcon = GetSprite(file);
                m_NetworkFileListView.DataSource.Add(itemData);
            }
        }

        private void m_ToolBar_ButtonCellClick(ToolBar arg0, ButtonCellType type)
        {
            switch (type)
            {
                case ButtonCellType.UploadFile:
                    UploadFile();
                    break;
                case ButtonCellType.UploadDirectory:
                    UploadDirectory();
                    break;
                case ButtonCellType.Download:
                    Download();
                    break;
                case ButtonCellType.Delete:
                    Delete();
                    break;
                case ButtonCellType.CreateDir:
                    Mkdir();
                    break;
                case ButtonCellType.Refresh:
                    Refresh();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        private void UploadFile()
        {
            var extensions = new ExtensionFilter[]
            {
                new ExtensionFilter("All Files", "*"),
            };

            string[] filePaths = FileBrowser.OpenFiles("上传文件", "", extensions);
            foreach (var filePath in filePaths)
            {
                Debug.Log(filePath);
                // 上传文件
                UploadNetworkFileOperation upload = networkFileModule.UploadNetworkFile(directoryId, filePath);
                // 发送监控内容
                TaskMonitorItemData monitorItem = new TaskMonitorItemData();
                monitorItem.Async = upload;
                monitorItem.FileIcon = GetSprite(upload.NetworkFile);
                EventDispatcher.ExecuteEvent<TaskMonitorItemData>(Events.TaskMonitor.Add, monitorItem);
                // 上传完成
                upload.OnCompleted(x =>
                {
                    if (!string.IsNullOrEmpty(upload.Error))
                    {
                        Debug.LogError(upload.Error);
                        return;
                    }
                    // 添加到文件列表
                    if (upload.NetworkFile.ParentId.Equals(directoryId))
                    {
                        NetworkFileItemData itemData = new NetworkFileItemData();
                        itemData.NetworkFile = upload.NetworkFile;
                        itemData.FileIcon = GetSprite(upload.NetworkFile);
                        m_NetworkFileListView.DataSource.Add(itemData);
                    }
                });
            }
        }

        /// <summary>
        /// 上传文件夹
        /// </summary>
        private void UploadDirectory()
        {
            string path = FileBrowser.OpenSingleFolder("上传文件夹", "");
            if (string.IsNullOrEmpty(path))
                return;

            // 上传文件夹
            UploadNetworkDirectoryOperation upload = networkFileModule.UploadNetworkDirectory(directoryId, path);
            // 发送监控内容
            TaskMonitorItemData monitorItem = new TaskMonitorItemData();
            monitorItem.Async = upload;
            monitorItem.FileIcon = m_ImageList["dir_type"];
            EventDispatcher.ExecuteEvent<TaskMonitorItemData>(Events.TaskMonitor.Add, monitorItem);
            // 上传完成
            upload.OnCompleted(x =>
            {
                if (!string.IsNullOrEmpty(upload.Error))
                {
                    Debug.LogError(upload.Error);
                    return;
                }
                // 添加到文件列表
                Refresh();
            });
        }

        /// <summary>
        /// 新建文件夹
        /// </summary>
        public void Mkdir()
        {
            NetworkFile file = new NetworkFile();
            file.Type = "dir";
            file.Name = "新建文件夹";
            file.ParentId = directoryId;
            WebRequestOperation mkdirAsync = networkFileModule.Mkdir(file);
            mkdirAsync.OnCompleted(x =>
            {
                if (!string.IsNullOrEmpty(x.Error))
                {
                    Debug.LogError(x.Error);
                    return;
                }
                // 获取新建文件夹请求结果
                string json = mkdirAsync.GetText();
                ResultJson result = ResultJson.Parser.ParseJson(json);
                if (result.Type == ResultJson.NORMAL)
                {
                    file.Id = result.Keyword;
                    NetworkFileItemData itemData = new NetworkFileItemData();
                    itemData.NetworkFile = file;
                    itemData.FileIcon = GetSprite(file);
                    itemData.IsEdit = true;
                    m_NetworkFileListView.DataSource.Add(itemData);
                }
                else
                {
                    Debug.LogError(result);
                }
            });
        }

        /// <summary>
        /// 刷新目录
        /// </summary>
        private void Refresh()
        {
            if (string.IsNullOrEmpty(directoryId))
            {
                Debug.LogError("当前目录Id为空。");
                return;
            }
            AccessDirectory(directoryId);
        }

        /// <summary>
        /// 删除文件夹或者文件
        /// </summary>
        private void Delete()
        {

            NetworkFileItem lockedFileItem = m_NetworkFileListView.SelectedItems.Find(x => x.Data.NetworkFile.Status == 1);
            if (lockedFileItem != null)
            {
                MessageBoxEx.Show("<color=red>您不能删除[" + lockedFileItem.Data.NetworkFile.Name + "]!</color>", "提示", MessageBoxExEnum.SingleDialog, null);
                return;
            }

            MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
            {
                int count = m_NetworkFileListView.SelectedIndicies.Count;
                bool flag = (bool)x.Content;
                if (flag)
                {
                    for (int i = 0; i < count; i++)
                    {
                        int index = m_NetworkFileListView.SelectedIndicies[i];
                        NetworkFileItem item = m_NetworkFileListView.Items[index];
                        WebRequestOperation async = networkFileModule.Remove(item.Data.NetworkFile.Id);
                        async.OnCompleted(y =>
                        {
                            if (!string.IsNullOrEmpty(y.Error))
                            {
                                Debug.LogError(y.Error);
                                return;
                            }
                            string json = async.GetText();
                            ResultJson result = ResultJson.Parser.ParseJson(json);
                            if (result.Type == ResultJson.NORMAL)
                            {

                            }
                            else
                            {
                                Debug.LogWarning(result.Detail);
                            }

                            var removeItem = m_NetworkFileListView.DataSource.Find(data => data.NetworkFile.Id == result.Keyword);
                            m_NetworkFileListView.DataSource.Remove(removeItem);
                        });
                    }
                }
            });
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        private void Download()
        {
            bool existDir = m_NetworkFileListView.SelectedItems.Exists(x => x.Data.NetworkFile.IsDirectory());
            if (existDir)
            {
                MessageBoxEx.Show("当前平台不能下载文件夹", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                // 保存文件夹路径
                string saveDir = FileBrowser.OpenSingleFolder("保存文件", "");
                if (string.IsNullOrEmpty(saveDir))
                    return;

                m_NetworkFileListView.SelectedItems.ForEach(x =>
                {
                    // 下载
                    DownloadNetworkFileOperation download = networkFileModule.DownloadNetworkFile(x.Data.NetworkFile, saveDir);
                    // 发送监控内容
                    TaskMonitorItemData monitorItem = new TaskMonitorItemData();
                    monitorItem.Async = download;
                    monitorItem.FileIcon = x.Data.FileIcon;
                    EventDispatcher.ExecuteEvent<TaskMonitorItemData>(Events.TaskMonitor.Add, monitorItem);
                    // 上传完成
                    download.OnCompleted(y =>
                    {
                        if (!string.IsNullOrEmpty(download.Error))
                        {
                            Debug.LogError(download.Error);
                            return;
                        }
                        // 添加到文件列表

                    });
                });
            }
        }
    }
}
