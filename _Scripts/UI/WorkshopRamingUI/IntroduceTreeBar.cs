using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
namespace XFramework.UI
{
    /// <summary>
    /// 介绍信息树状结构面板
    /// </summary>
    public class IntroduceTreeBar : BaseRamingUI{
        public class TreeNodeSelectedEvent : UnityEvent<TreeNode<TreeViewItem>> { }

        /// <summary>
        /// 房间信息介绍内容
        /// </summary>
        public List<IntroductionPoint> IntroduceData { get; private set; }

        /// <summary>
        /// 图标列表
        /// </summary>
        public ImageList ImageList;

        /// <summary>
        /// 设备列表树状视图
        /// </summary>
        private TreeView m_TreeView;

        #region ///动画参数
        /// <summary>
        /// 初始化位置
        /// </summary>
        private float  initPosX = -831;

        /// <summary>
        /// 目标位置
        /// </summary>
        private float  TargetPosX = -541;

        /// <summary>
        /// 位置
        /// </summary>
        private RectTransform rectTransform;

        private Tweener treeTweener;
        #endregion

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

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
        //private UnityEvent m_ClosePanel = new UnityEvent();

        //public UnityEvent OnClosePanel
        //{
        //    get { return m_ClosePanel; }
        //    set { m_ClosePanel = value; }
        //}
        void Awake()
        {
            m_TreeView = transform.Find("TreeView").GetComponent<TreeView>();
            m_TreeView.NodeSelected.AddListener(m_TreeView_NodeSelected);
            m_TreeView.NodeToggle.AddListener(m_TreeView_NodeToggle);
            rectTransform = transform.GetComponent<RectTransform>();

            ShowMode = UIWindowShowMode.DoNothing;
        }

        private void Start()
        {
            InitTweenAnimation();
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
        public void InitData(List<IntroductionPoint> data)
        {
            IntroduceData = data;
            m_TreeView.Nodes = List2Tree(IntroduceData);
            //TreeTweenAnima.DOPlayForward();
            //PlayForword();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void ClearData()
        {
            IntroduceData = null;
            m_TreeView.Nodes.Clear();
            //PlayRewind(()=> {
           
            //});
        }
     
        /// <summary>
        /// 将房间信息转化Tree
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        private ObservableList<TreeNode<TreeViewItem>> List2Tree(List<IntroductionPoint> data)
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
        private void CreateTree(IntroductionPoint Contents, TreeNode<TreeViewItem> node)
        {
            foreach (var _Contents in Contents.IntroduceList)
            {
                Sprite sprite = ImageList["Folder"];

                var item = new TreeViewItem(_Contents.Name, sprite);
                item.Tag = _Contents;
                TreeNode<TreeViewItem> _folderNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                node.Nodes.Add(_folderNode);

                CreateContentNode(_Contents,_folderNode);
            }

            ///设备大节点
            foreach (var _machine in Contents.MachinesList)
            {
                //Sprite sprite = ImageList["Machines"];
                Sprite sprite = ImageList["Folder"];

                //Sprite sprite = null;
                var item = new TreeViewItem(_machine.Name, sprite);
                item.Tag = _machine;
                TreeNode<TreeViewItem> _fileNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                node.Nodes.Add(_fileNode);

                CreateMachineNode(_machine,_fileNode);
            }
        }
        /// <summary>
        /// 创建具体内容目录
        /// </summary>
        /// <param name="ConItem"></param>
        /// <param name="node"></param>
        private void CreateContentNode(IntroduceContents ConItem, TreeNode<TreeViewItem> node)
        {
            foreach (var _Content in ConItem.ContentItemList)
            {
                Sprite sprite = null;
                var item = new TreeViewItem(_Content.Name, sprite);
                item.Tag = _Content;
                TreeNode<TreeViewItem> _fileNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                node.Nodes.Add(_fileNode);
            }
        }
        /// <summary>
        /// 创建设备信息目录
        /// </summary>
        /// <param name="mItem"></param>
        /// <param name="node"></param>
        private void CreateMachineNode(Machine mItem, TreeNode<TreeViewItem> node)
        {
            foreach (var _machine in mItem.MachineList)
            {
                Sprite sprite = null;
                var item = new TreeViewItem(_machine.Name, sprite);
                item.Tag = _machine;
                TreeNode<TreeViewItem> _fileNode = new TreeNode<TreeViewItem>(item, new ObservableList<TreeNode<TreeViewItem>>(), false, true);
                node.Nodes.Add(_fileNode);
            }
        }


        /// <summary>
        /// 播放列表隐藏显示动画
        /// </summary>
        public void PlayListPanelAnimation()
        {
            if (IntroduceData == null)
                return;
            if(IsShow)
            {
                PlayRewind();
            }
            else
            {
                PlayForword();

            }
        }

        /// <summary>
        ///初始化位置
        /// </summary>
        private void InitTweenAnimation()
        {
            Vector3 temp = rectTransform.localPosition;
            rectTransform.localPosition = new Vector3(initPosX, temp.y, temp.z);
        }

        /// <summary>
        /// 显示列表
        /// </summary>
        private void PlayForword()
        {
            if (IsShow)
                return;
            IsShow = true;
            treeTweener = transform.DOLocalMoveX(TargetPosX,0.3f).SetEase(Ease.InBounce).OnComplete(()=> {
            });

        }
        /// <summary>
        /// 关闭列表
        /// </summary>
        /// <param name="callback"></param>
        private void PlayRewind(TweenCallback callback)
        {
            if (!IsShow)
                return;
            IsShow = false;
            treeTweener = transform.DOLocalMoveX(initPosX, 0.3f).SetEase(Ease.InCubic).OnComplete(callback);

        }

        /// <summary>
        /// 关闭列表 
        /// </summary>
        private void PlayRewind()
        {
            if (!IsShow)
                return;
            IsShow = false;
            treeTweener = transform.DOLocalMoveX(initPosX, 0.3f).SetEase(Ease.InCubic);
        }
   

        public override void Show(BaseUIArgs uiParams)
        {
            base.Show(uiParams);

            if (!IsShow)
            {
                PlayForword();
            }
            print("显示 树");
        }

        public override void Hide(BaseUIArgs uiParams)
        {
            base.Show(uiParams);

            if (IsShow)
            {
                PlayRewind();
            }
            print("关闭 树");
            ClearData();
        }

    }
}
