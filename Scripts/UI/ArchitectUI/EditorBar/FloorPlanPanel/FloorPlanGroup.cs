using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;

namespace XFramework.UI
{
    public class FloorPlanGroup : MultiListViewCustom<FloorPlanGroupData, FloorPlanItem, FloorPlanItemData>
    {
        private Text m_TextHead;

        protected override void Awake()
        {
            base.Awake();
            m_TextHead = transform.Find("Header/Text").GetComponent<Text>();
        }

        public override void SetData(FloorPlanGroupData data)
        {
            base.SetData(data);
            m_TextHead.text = data.Name;
            DataSource.Clear();
            if (data.FloorPlanItemDataList != null)
            {
                DataSource.AddRange(data.FloorPlanItemDataList);
            }
        }
    }
}
