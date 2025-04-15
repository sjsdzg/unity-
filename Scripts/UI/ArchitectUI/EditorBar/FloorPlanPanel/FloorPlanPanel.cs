using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;
using System;
using XFramework.Core;

namespace XFramework.UI
{
    public class FloorPlanPanel : MultiListViewCustom<FloorPlanPanelData, FloorPlanGroup, FloorPlanGroupData>
    {
        private UniEvent<FloorPlanItem> m_OnFloorPlanItemSelected = new UniEvent<FloorPlanItem>();
        /// <summary>
        /// cad item 点击时，触发
        /// </summary>
        public UniEvent<FloorPlanItem> OnFloorPlanItemSelected
        {
            get { return m_OnFloorPlanItemSelected; }
            set { m_OnFloorPlanItemSelected = value; }
        }

        /// <summary>
        /// 标题头
        /// </summary>
        private Text m_TextHead;

        protected override void Awake()
        {
            base.Awake();
            m_TextHead = transform.Find("Header/Text").GetComponent<Text>();
        }

        public override void SetData(FloorPlanPanelData data)
        {
            base.SetData(data);
            m_TextHead.text = data.Name;
            DataSource.Clear();
            if (data.FloorPlanGroupDataList != null)
            {
                DataSource.AddRange(data.FloorPlanGroupDataList);
            }
        }

        protected override void SetData(FloorPlanGroup item, FloorPlanGroupData data)
        {
            base.SetData(item, data);
            item.OnItemSelected.AddListener(x => FloorPlanGroup_OnItemSelected(item, x));
        }

        private void FloorPlanGroup_OnItemSelected(FloorPlanGroup floorPlanGroup, FloorPlanItem floorPlanItem)
        {
            foreach (var group in m_Items)
            {
                if (group.Equals(floorPlanGroup))
                    continue;

                group.DeselectAll();
            }
            OnFloorPlanItemSelected.Invoke(floorPlanItem);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
