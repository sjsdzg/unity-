using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Module;
using XFramework.Common;
using UnityEngine.UI;
using DG.Tweening;

namespace XFramework.UI
{
    /// <summary>
    /// 子工艺列表面板
    /// </summary>
    public class SubprocessListPanel : MonoBehaviour
    {
        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认Item
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// Subprocess项组件列表
        /// </summary>
        public List<Subprocess> SubprocessList { get; private set; }

        /// <summary>
        /// Subprocess点击事件类
        /// </summary>
        public class ItemClickEvent : UnityEvent<Subprocess> { }

        private ItemClickEvent m_ItemOnClicked = new ItemClickEvent();
        /// <summary>
        /// Subprocess点击触发
        /// </summary>
        public ItemClickEvent ItemOnClicked
        {
            get { return m_ItemOnClicked; }
            set { m_ItemOnClicked = value; }
        }

        /// <summary>
        /// 选中的Subprocess
        /// </summary>
        public Subprocess SelectedItem { get; private set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        private bool isOn = false;

        /// <summary>
        /// 面板宽度
        /// </summary>
        private float m_Width;

        /// <summary>
        /// 原始位置
        /// </summary>
        private Vector2 rawAnchoredPos = Vector2.zero;

        /// <summary>
        /// 自身
        /// </summary>
        private RectTransform m_Rect;

        /// <summary>
        /// 收缩Toggle
        /// </summary>
        private Toggle ShrinkToggle;

        void Awake()
        {
            SubprocessList = new List<Subprocess>();

            m_Rect = transform.GetComponent<RectTransform>();
            ShrinkToggle = transform.Find("ShrinkToggle").GetComponent<Toggle>();
            ShrinkToggle.onValueChanged.AddListener(ShrinkToggle_onValueChanged);

            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        void Start()
        {
            m_Width = m_Rect.rect.width;
            isOn = true;
            this.Invoke(0.2f, GetAnchoredPos);
        }

        private void GetAnchoredPos()
        {
            rawAnchoredPos = m_Rect.anchoredPosition;
        }

        /// <summary>
        /// 收缩Toggle点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void ShrinkToggle_onValueChanged(bool b)
        {
            if (b)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        /// <summary>
        /// 增加Subprocess
        /// </summary>
        /// <param name="info"></param>
        private void AddSubprocess(SubprocessInfo info)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            Subprocess Item = obj.GetComponent<Subprocess>();

            if (Content != null && Item != null)
            {
                Item.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                Item.SetValue(info);
                SubprocessList.Add(Item);

                Item.OnClick.AddListener(Item_OnClick);
            }

            SubprocessList[0].Select();
        }

        /// <summary>
        /// 增加一组Subprocess
        /// </summary>
        /// <param name="infos"></param>
        public void AddRange(List<SubprocessInfo> infos)
        {
            foreach (SubprocessInfo info in infos)
            {
                AddSubprocess(info);
            }
        }

        /// <summary>
        /// 清空Subprocess
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < SubprocessList.Count; i++)
            {
                Subprocess item = SubprocessList[i];
                Destroy(item.gameObject);
            }
            SubprocessList.Clear();
        }

        /// <summary>
        /// Subprocess点击时，触发。
        /// </summary>
        /// <param name="item"></param>
        private void Item_OnClick(Subprocess item)
        {
            SelectedItem = item;
            m_ItemOnClicked.Invoke(item);
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        public void Show()
        {
            m_Rect.DOAnchorPos(new Vector3(rawAnchoredPos.x, rawAnchoredPos.y), 0.5f).OnComplete(() =>
            {
                isOn = true;
            });
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        public void Hide()
        {
            m_Rect.DOAnchorPos(new Vector3(rawAnchoredPos.x + m_Rect.rect.width, rawAnchoredPos.y), 0.5f).OnComplete(() =>
            {
                isOn = false;
            });
        }
    }
}
