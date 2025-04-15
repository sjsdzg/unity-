using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Module;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using XFramework.Common;

namespace XFramework.UI
{
    public class ProductionItemElement : Selectable, IEventSystemHandler, IPointerClickHandler
    {
        public class OnClickEvent : UnityEvent<ProductionItemElement> { }

        /// <summary>
        /// 物品图标路径
        /// </summary>
        public const string GoodsPath = "Icons/Goods/";
        /// <summary>
        /// 清洁图标路径
        /// </summary>
        public const string CleanPath = "Icons/Clean/";
        /// <summary>
        /// 文件图标路径
        /// </summary>
        public const string FilePath = "Icons/File/";

        /// <summary>
        /// 文本
        /// </summary>
        public Text text;

        /// <summary>
        /// 图标
        /// </summary>
        public Image icon;

        /// <summary>
        /// 工具数据
        /// </summary>
        public Item Item { get; private set; }

        private OnClickEvent m_OnClick = new OnClickEvent();
        /// <summary>
        /// 工具点击事件
        /// </summary>
        public OnClickEvent OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            icon = transform.Find("Image").GetComponent<Image>();
            text = transform.Find("Text").GetComponent<Text>();
        }

        /// <summary>
        /// Press
        /// </summary>
        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;
            //点击事件，触发。
            ToolTip.Instance.Show(false);
            OnClick.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            //点击
            Press();
        }

        /// <summary>
        /// 设置
        /// </summary>
        public void SetValue(Item item)
        {
            Item = item;

            string path = string.Empty;
            switch (item.Type)
            {
                case ItemType.Document:
                    path = FilePath;
                    break;
                case ItemType.Goods:
                    path = GoodsPath;
                    break;
                case ItemType.Clean:
                    path = CleanPath;
                    break;
                default:
                    break;
            }

            Sprite sprite = Resources.Load<Sprite>(path + item.Sprite);
            if (sprite == null)
            {
                icon.gameObject.SetActive(false);
            }
            else
            {
                icon.sprite = sprite;
            }
            text.text = item.Description;
        }

    }
}
