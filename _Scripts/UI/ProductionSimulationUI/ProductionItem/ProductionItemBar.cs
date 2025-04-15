using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;
using XFramework.Module;
using UnityEngine.UI.Extensions;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 生产工具栏目
    /// </summary>
    public class ProductionItemBar : MonoBehaviour
    {
        public class OnClickEvent : UnityEvent<ProductionItemElement> { }

        /// <summary>
        /// 清洁工具Toggle
        /// </summary>
        private Toggle toggleClean;

        /// <summary>
        /// 物品Toggle
        /// </summary>
        private Toggle toggleGoods;

        /// <summary>
        /// 文件Toggle
        /// </summary>
        private Toggle toggleFile;

        /// <summary>
        /// Viewport CanvasGroup
        /// </summary>
        private CanvasGroup m_CanvasGroup;

        /// <summary>
        /// ProductionItemContent
        /// </summary>
        private ProductionItemContent m_ItemContent;

        private OnClickEvent m_ItemOnClicked = new OnClickEvent();
        /// <summary>
        /// Item点击时，触发
        /// </summary>
        public OnClickEvent ItemOnClicked
        {
            get { return m_ItemOnClicked; }
            set { m_ItemOnClicked = value; }
        }

        void Awake()
        {
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            m_CanvasGroup = transform.Find("ScrollView/Viewport").GetComponent<CanvasGroup>();
            m_ItemContent = transform.Find("ScrollView/Viewport/Content").GetComponent<ProductionItemContent>();
            toggleClean = transform.Find("ButtomBar/ToggleClean").GetComponent<Toggle>();
            toggleGoods = transform.Find("ButtomBar/ToggleGoods").GetComponent<Toggle>();
            toggleFile = transform.Find("ButtomBar/ToggleFile").GetComponent<Toggle>();
        }

        private void InitEvent()
        {

            toggleClean.onValueChanged.AddListener(x => {
                if (x)
                {
                    ChangeItemType(ItemType.Clean);
                }
            });

            toggleGoods.onValueChanged.AddListener(x => {
                if (x)
                {
                    ChangeItemType(ItemType.Goods);
                }
            });

            toggleFile.onValueChanged.AddListener(x => {
                if (x)
                {
                    ChangeItemType(ItemType.Document);
                }
            });
            ChangeItemType(ItemType.Goods);
            m_ItemContent.ItemOnClicked.AddListener(m_ItemContent_ItemOnClicked);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item item)
        {
            m_ItemContent.AddItem(item);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            m_ItemContent.RemoveItem(item.Name);
        }

        private void ChangeItemType(ItemType itemType)
        {
            m_CanvasGroup.DOFade(0, 0.1f).OnComplete(() => 
            {
                m_ItemContent.CurrentItemType = itemType;
                m_CanvasGroup.DOFade(1, 0.1f);
            });
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            if (!gameObject.activeSelf)
            {
                transform.DOScale(0, 0.3f).From();
            }
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Item点击时触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_ItemContent_ItemOnClicked(ProductionItemElement element)
        {
            ItemOnClicked.Invoke(element);
        }

    }
}

                                                  