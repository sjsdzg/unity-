using UnityEngine;
using System.Collections;
using UIWidgets;
using System.Collections.Generic;
using XFramework.Module;
using XFramework.UI;
using UnityEngine.Events;

namespace XFramework.UI
{
    public class HelpListPanel : MonoBehaviour
    {
        public class TreeNodeSelectedEvent : UnityEvent<TreeNode<TreeViewItem>> { }

        /// <summary>
        /// 工程设计资料
        /// </summary>
        public Folder m_Folder { get; private set; }

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
        public void InitData(Folder data)
        {
            m_Folder = data;
            m_TreeView.Nodes = List2Tree(data);
        }

        /// <summary>
        /// 将设备信息转化Tree
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        private ObservableList<TreeNode<TreeViewItem>> List2Tree(Folder folder)
        {
            var nodes = new ObservableList<TreeNode<TreeViewItem>>();

            foreach (var _folder in folder.FolderList)
            {
                Sprite sprite = ImageList["Folder"];
                var item = new TreeViewItem(_folder.Name, sprite);
                item.Tag = _folder;
                TreeNode<TreeViewItem> _folderNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                nodes.Add(_folderNode);
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
                nodes.Add(_fileNode);
            }

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

