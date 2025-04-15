using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺流程图
    /// </summary>
    public class ProcessDiagramBar : MonoBehaviour
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
        /// SubprocessItem项组件列表
        /// </summary>
        public List<SubprocessItem> SubprocessItems { get; private set; }

        /// <summary>
        /// SubprocessItem点击事件类
        /// </summary>
        public class ItemClickEvent : UnityEvent<SubprocessItem> { }

        private ItemClickEvent m_ItemOnClicked = new ItemClickEvent();
        /// <summary>
        /// 子工艺Item点击触发
        /// </summary>
        public ItemClickEvent ItemOnClicked
        {
            get { return m_ItemOnClicked; }
            set { m_ItemOnClicked = value; }
        }

        /// <summary>
        /// 选中的SubprocessItem
        /// </summary>
        public SubprocessItem SelectedItem { get; private set; }

        void Awake()
        {
            SubprocessItems = new List<SubprocessItem>();

            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 增加SubprocessItem
        /// </summary>
        /// <param name="info"></param>
        private void AddSubprocessItem(SubprocessInfo info)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            SubprocessItem Item = obj.GetComponent<SubprocessItem>();

            if (Content != null && Item != null)
            {
                Item.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                Item.SetValue(info);
                SubprocessItems.Add(Item);

                Item.OnClick.AddListener(Item_OnClick);
            }
        }

        /// <summary>
        /// 添加一组子工艺
        /// </summary>
        /// <param name="infos"></param>
        public void AddRange(List<SubprocessInfo> infos)
        {
            Item_OnNext(infos, 0);
        }

        private void Item_OnNext(List<SubprocessInfo> infos, int i)
        {
            if (i > infos.Count - 1)
                return;

            SubprocessInfo info = infos[i];

            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            SubprocessItem Item = obj.GetComponent<SubprocessItem>();

            if (Content != null && Item != null)
            {
                Item.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                if (i == infos.Count - 1)
                {
                    Item.SetValue(info, false);
                }
                else
                {
                    Item.SetValue(info);
                }
                
                SubprocessItems.Add(Item);

                Item.OnClick.AddListener(Item_OnClick);

                Item.OnNext.AddListener(() => {
                    int index = i + 1;
                    Item_OnNext(infos, index);
                });
            }
        }


        /// <summary>
        /// 移除SubprocessItem
        /// </summary>
        public void RemovePartItem(string name)
        {
            for (int i = 0; i < SubprocessItems.Count; i++)
            {
                SubprocessItem item = SubprocessItems[i];

                if (item.Data.Name.Equals(name))
                {
                    SubprocessItems.Remove(item);
                    Destroy(item.gameObject);
                    break;
                }
            }
        }

        /// <summary>
        /// 清空SubprocessItem
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < SubprocessItems.Count; i++)
            {
                SubprocessItem item = SubprocessItems[i];
                Destroy(item.gameObject);
            }
            SubprocessItems.Clear();
        }

        /// <summary>
        /// SubprocessItem点击时，触发。
        /// </summary>
        /// <param name="item"></param>
        private void Item_OnClick(SubprocessItem item)
        {
            SelectedItem = item;
            m_ItemOnClicked.Invoke(item);
        }
    }
}
