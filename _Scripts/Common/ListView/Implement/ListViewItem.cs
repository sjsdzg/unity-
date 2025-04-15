using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.Common
{
    /// <summary>
    /// Sample ListViewItem
    /// </summary>
    public class ListViewItem : ListViewItemBase<ListViewItemData>
    {
        /// <summary>
        /// 图片
        /// </summary>
        public Image m_Image;

        /// <summary>
        /// 文本
        /// </summary>
        public Text m_Text;

        public override void SetData(ListViewItemData data)
        {
            base.SetData(data);
            if (m_Image != null)
                m_Image.sprite = data.Sprite;
            
            m_Text.text = data.Text;
        }
    }
}

