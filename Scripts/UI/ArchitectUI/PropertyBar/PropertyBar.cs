using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;

namespace XFramework.UI
{
    public class PropertyBar : MultiListViewCustom<PropertyBarData, PropertyGroup, PropertyGroupData>
    {
        private Text m_TextHead;

        private Text m_Text;

        protected override void Awake()
        {
            base.Awake();
            m_TextHead = transform.Find("Header/Text").GetComponent<Text>();
        }

        public override void SetData(PropertyBarData data)
        {
            base.SetData(data);
            m_TextHead.text = data.Name;
            DataSource.Clear();
            if (data.PropertyGroupDataList != null)
            {
                DataSource.AddRange(data.PropertyGroupDataList);
            }
        }
    }
}
