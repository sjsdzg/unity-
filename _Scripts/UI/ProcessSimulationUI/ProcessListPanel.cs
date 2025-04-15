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
    /// 工艺列表Panel
    /// </summary>
    public class ProcessListPanel : MonoBehaviour
    {
        public class TreeNodeSelectedEvent : UnityEvent<TreeNode<TreeViewItem>> { }

        /// <summary>
        /// 工艺列表
        /// </summary>
        private List<ProcessInfo> m_ProcessInfos = new List<ProcessInfo>();

        /// <summary>
        /// 图标列表
        /// </summary>
        public ImageList ImageList;

        /// <summary>
        /// 工艺列表树状视图
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

        void Awake()
        {
            m_TreeView = transform.Find("TreeView").GetComponent<TreeView>();
            m_TreeView.NodeSelected.AddListener(m_TreeView_NodeSelected);
        }

        /// <summary>
        /// TreeView选中时触发
        /// </summary>
        /// <param name="node"></param>
        private void m_TreeView_NodeSelected(TreeNode<TreeViewItem> node)
        {
            SelectedNode = node;
            NodeSelected.Invoke(SelectedNode);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="infos"></param>
        public void InitData(List<ProcessInfo> infos)
        {
            m_ProcessInfos = infos;
            m_TreeView.Nodes = List2Tree(m_ProcessInfos);
            m_TreeView.Select(0);
        }

        /// <summary>
        /// 将工艺信息转化Tree
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        private ObservableList<TreeNode<TreeViewItem>> List2Tree(List<ProcessInfo> infos)
        {
            var nodes = new ObservableList<TreeNode<TreeViewItem>>();

            infos.ForEach(x =>
            {
                nodes.Add(Item2Node(x));
            });

            return nodes;
        }

        /// <summary>
        /// 工艺信息转化成TreeNode
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private TreeNode<TreeViewItem> Item2Node(ProcessInfo info)
        {
            Sprite sprite = ImageList["Object"];
            var nodeItem = new TreeViewItem(info.Name, sprite);
            nodeItem.Tag = info;

            return new TreeNode<TreeViewItem>(nodeItem, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
        }
    }
}
