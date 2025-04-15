using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;

namespace XFramework.UI
{
    public class GroupLibraryItem : ListViewItemBase<GroupLibraryItemData>
    {
        /// <summary>
        /// 图片
        /// </summary>
        public Image m_Image;

        /// <summary>
        /// 文本
        /// </summary>
        public Text m_Text;

        public override void SetData(GroupLibraryItemData data)
        {
            base.SetData(data);
            if (data.Sprite != null)
            {
                m_Image.sprite = data.Sprite;
            }
            m_Text.text = data.GroupInfo.Name;
        }
    }
}