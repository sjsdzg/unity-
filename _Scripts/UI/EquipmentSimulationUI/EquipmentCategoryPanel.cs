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
    /// 设备库面板
    /// </summary>
    public class EquipmentCategoryPanel : MonoBehaviour
    {
        public class TreeNodeSelectedEvent : UnityEvent<TreeNode<TreeViewItem>> { }

        /// <summary>
        /// 工程设计资料
        /// </summary>
        public EquipmentCategory EquipmentCategory { get; private set; }

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
        public void InitData(EquipmentCategory equipmentCategory)
        {
            EquipmentCategory = equipmentCategory;
            //
            //Sprite sprite = ImageList["category"];
            //var item = new TreeViewItem(EquipmentCategory.Name, sprite);
            //item.Tag = EquipmentCategory;
            ////root node
            //TreeNode<TreeViewItem> root = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>());
            //CreateTree(EquipmentCategory, root);

            ObservableList<TreeNode<TreeViewItem>> nodes = new ObservableList<TreeNode<TreeViewItem>>();

            foreach (var category in EquipmentCategory.Categorys)
            {
                Sprite sprite = ImageList["category"];
                var item = new TreeViewItem(category.Name, sprite);
                item.Tag = category;
                TreeNode<TreeViewItem> _folderNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                nodes.Add(_folderNode);
                CreateTree(category, _folderNode);
            }

            foreach (var equipment in EquipmentCategory.Equipments)
            {
                Sprite sprite = ImageList["equipment"];
                var item = new TreeViewItem(equipment.Name, sprite);
                item.Tag = equipment;
                TreeNode<TreeViewItem> _fileNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                nodes.Add(_fileNode);
            }

            m_TreeView.Nodes = nodes;
        }

        /// <summary>
        /// 组成组成树状结构-递归
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private void CreateTree(EquipmentCategory equipmentCategory, TreeNode<TreeViewItem> node)
        {
            foreach (var category in equipmentCategory.Categorys)
            {
                Sprite sprite = ImageList["category"];
                var item = new TreeViewItem(category.Name, sprite);
                item.Tag = category;
                TreeNode<TreeViewItem> _folderNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                node.Nodes.Add(_folderNode);
                CreateTree(category, _folderNode);
            }

            foreach (var equipment in equipmentCategory.Equipments)
            {
                Sprite sprite = ImageList["equipment"];
                var item = new TreeViewItem(equipment.Name, sprite);
                item.Tag = equipment;
                TreeNode<TreeViewItem> _fileNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                node.Nodes.Add(_fileNode);
            }
        }
    }
}
