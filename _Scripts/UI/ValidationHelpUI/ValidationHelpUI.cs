using UnityEngine;
using System.Collections;
using UIWidgets;
using XFramework.Module;
using UnityEngine.UI;
using XFramework.Core;
using System.Collections.Generic;
using System;

namespace XFramework.UI
{
    public class ValidationHelpUI : BaseUI
    {

        public override EnumUIType GetUIType()
        {
            return EnumUIType.ValidationHelpUI;
        }

        /// <summary>
        /// 工程设计根目录
        /// </summary>
        private readonly string rootDir = "ValidationSimulation/Help/";

        /// <summary>
        /// 设计信息面板
        /// </summary>
        private HelpListPanel helpListPanel;

        /// <summary>
        /// 工程设计文档
        /// </summary>
        private HelpDisplayPanel helpDisplayPanel;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 工程设计模块
        /// </summary>
        public ValidationHelpModule module;

        protected override void OnAwake()
        {
            base.OnAwake();
            ModuleManager.Instance.Register<ValidationHelpModule>();
            module = ModuleManager.Instance.GetModule<ValidationHelpModule>();

            InitGUI();
            InitEvent();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            helpListPanel = transform.Find("Background/HelpListPanel").GetComponent<HelpListPanel>();
            helpDisplayPanel = transform.Find("Background/HelpDisplayPanel").GetComponent<HelpDisplayPanel>();
            buttonClose = transform.Find("Background/Header/ButtonClose").GetComponent<Button>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            helpListPanel.NodeSelected.AddListener(designInfoPanel_NodeSelected);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<ValidationHelpModule>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Folder m_Folder = module.GetFolder();
            helpListPanel.InitData(m_Folder);
        }


        private void buttonClose_onClick()
        {
            UIManager.Instance.CloseUI(EnumUIType.ValidationHelpUI);
        }

        /// <summary>
        /// 设计信息面板点击时，触发
        /// </summary>
        /// <param name="node"></param>
        private void designInfoPanel_NodeSelected(TreeNode<TreeViewItem> node)
        {
            object tag = node.Item.Tag;

            if (tag is File)//文件节点
            {
                File file = tag as File;
                switch (file.Type)
                {
                    case FileType.PDF:
                        string path = "Assets/Documents/Validation/Help/" + file.Name + ".pdf";
                        AsyncLoadAssetOperation async = Assets.LoadAssetAsync<TextAsset>(path);
                        if (async != null)
                        {
                            async.OnCompleted(x =>
                            {
                                AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                                TextAsset asset = loader.GetAsset<TextAsset>();
                                helpDisplayPanel.LoadDocumentWithBuffer(asset.bytes);
                            });
                        }
                        break;
                    case FileType.Image:
                        break;
                    case FileType.Video:

                        break;
                    default:
                        break;
                }

            }
        }
    }
}

