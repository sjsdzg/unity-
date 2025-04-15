using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 工程设计面板
    /// </summary>
    public class DesignInfoPanel : MonoBehaviour
    {
        public class TreeNodeSelectedEvent : UnityEvent<TreeNode<TreeViewItem>> { }

        /// <summary>
        /// 工程设计资料
        /// </summary>
        public List<Folder> DesignData { get; private set; }

        /// <summary>
        /// 图标列表
        /// </summary>
        public ImageList ImageList;

        /// <summary>
        /// 设备列表树状视图
        /// </summary>
        private TreeView m_TreeView;

        /// <summary>
        /// 选中的TreeViewItem
        /// </summary>
        public TreeNode<TreeViewItem> SelectedNode { get; private set; }

        private TreeNodeSelectedEvent m_NodeSelected = new TreeNodeSelectedEvent();
        /// <summary>
        /// TreeNode选中事件
        /// </summary>
        public TreeNodeSelectedEvent NodeSelected
        {
            get { return m_NodeSelected; }
            set { m_NodeSelected = value; }
        }

        private TreeNode<TreeViewItem> lastSelectedNode = null;

        void Awake()
        {
            m_TreeView = transform.Find("TreeView").GetComponent<TreeView>();
            m_TreeView.NodeSelected.AddListener(m_TreeView_NodeSelected);
            m_TreeView.NodeToggle.AddListener(m_TreeView_NodeToggle);
        }

        private void m_TreeView_NodeToggle(TreeNode<TreeViewItem> node)
        {
            m_TreeView.SelectedNodes = new List<TreeNode<TreeViewItem>> { SelectedNode };
        }

        /// <summary>
        /// TreeView选中时触发
        /// </summary>
        /// <param name="node"></param>
        private void m_TreeView_NodeSelected(TreeNode<TreeViewItem> node)
        {
            SelectedNode = node;
            NodeSelected.Invoke(SelectedNode);
            m_TreeView.SelectedNodes = new List<TreeNode<TreeViewItem>> { SelectedNode };
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="infos"></param>
        public void InitData(List<Folder> data)
        {
            DesignData = data;
            m_TreeView.Nodes = List2Tree(DesignData);
        }

        /// <summary>
        /// 将设备信息转化Tree
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        private ObservableList<TreeNode<TreeViewItem>> List2Tree(List<Folder> data)
        {
            var nodes = new ObservableList<TreeNode<TreeViewItem>>();

            data.ForEach(x =>
            {
                Sprite sprite = ImageList["Folder"];
                var item = new TreeViewItem(x.Name, sprite);
                item.Tag = x;
                TreeNode<TreeViewItem> node = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                nodes.Add(node);
                CreateTree(x, node);
            });

            return nodes;
        }

        /// <summary>
        /// 组成组成树状结构-递归
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private void CreateTree(Folder folder, TreeNode<TreeViewItem> node)
        {
            foreach (var _folder in folder.FolderList)
            {
                Sprite sprite = ImageList["Folder"];
                var item = new TreeViewItem(_folder.Name, sprite);
                item.Tag = _folder;
                TreeNode<TreeViewItem> _folderNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                node.Nodes.Add(_folderNode);
                CreateTree(_folder, _folderNode);
            }

            foreach (var _file in folder.FileList)
            {
                Sprite sprite = null;
                switch (_file.Type)
                {
                    case FileType.PDF:
                        sprite = ImageList["Pdf"];
                        break;
                    case FileType.Image:
                        break;
                    case FileType.Video:
                        sprite = ImageList["Video"];
                        break;
                    default:
                        break;
                }
                
                var item = new TreeViewItem(_file.Name, sprite);
                item.Tag = _file;
                TreeNode<TreeViewItem> _fileNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                node.Nodes.Add(_fileNode);
            }
        }
    }
}
