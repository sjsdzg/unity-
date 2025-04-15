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
    /// 设备视图面板
    /// </summary>
    public class DevicePanel : MonoBehaviour
    {
        public class TreeNodeSelectedEvent : UnityEvent<TreeNode<TreeViewItem>> { }

        /// <summary>
        /// 设备信息列表
        /// </summary>
        private List<DeviceInfo> m_DeviceInfos = new List<DeviceInfo>();

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

        private int selectedIndex;
        /// <summary>
        /// 当前索引
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                selectedIndex = m_TreeView.SelectedIndex;
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                m_TreeView.Select(selectedIndex);
            }
        }


        private UnityEvent DocumentPanel_Close = new UnityEvent();
        public UnityEvent _DocumentPanel
        {
            get
            {
                return DocumentPanel_Close;
            }
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
            print(node.Item.Name);

            _DocumentPanel.Invoke();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="infos"></param>
        public void InitData(List<DeviceInfo> infos)
        {
            m_DeviceInfos = infos;
            m_TreeView.Nodes = List2Tree(m_DeviceInfos);
            //m_TreeView.Select(0);
        }

        /// <summary>
        /// 将设备信息转化Tree
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        private ObservableList<TreeNode<TreeViewItem>> List2Tree(List<DeviceInfo> infos)
        {
            var nodes = new ObservableList<TreeNode<TreeViewItem>>();

            infos.ForEach(x =>
            {
                nodes.Add(Item2Node(x));
            });

            return nodes;
        }

        /// <summary>
        /// 设备信息转化成TreeNode
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private TreeNode<TreeViewItem> Item2Node(DeviceInfo info)
        {
            Sprite sprite = ImageList["Object"];
            var nodeItem = new TreeViewItem(info.Name, sprite);
            nodeItem.Tag = info;

            return new TreeNode<TreeViewItem>(nodeItem, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
        }
    }
}
