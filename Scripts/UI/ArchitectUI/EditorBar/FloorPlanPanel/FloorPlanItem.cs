using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;

namespace XFramework.UI
{
    public class FloorPlanItem : ListViewItemBase<FloorPlanItemData>
    {
        /// <summary>
        /// 图片
        /// </summary>
        [SerializeField]
        private Image m_Image;

        /// <summary>
        /// 文本
        /// </summary>
        [SerializeField]
        private Text m_Text;

        public override void SetData(FloorPlanItemData data)
        {
            base.SetData(data);
            m_Image.sprite = data.Sprite;
            m_Text.text = data.Text;
        }
    }
}
