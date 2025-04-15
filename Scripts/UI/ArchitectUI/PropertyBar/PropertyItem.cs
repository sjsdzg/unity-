using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;

namespace XFramework.UI
{
    public class PropertyItem : ListViewItemBase<PropertyItemData>
    {
        private Text m_Text;

        protected override void Awake()
        {
            base.Awake();
            m_Text = transform.Find("Text").GetComponent<Text>();
        }

        public override void SetData(PropertyItemData data)
        {
            base.SetData(data);
            m_Text.text = data.Text;
        }
    }
}
