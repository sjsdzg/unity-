using UnityEngine;
using System.Collections;
using XFramework.Common;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class ProcessParamGroup : MultiListViewCustom<ProcessParamGroupData, ProcessParamItem, ProcessParamItemData>
    {

        /// <summary>
        /// 名称
        /// </summary>
        [SerializeField]
        private Text m_TextName;

        public override void SetData(ProcessParamGroupData data)
        {
            base.SetData(data);
            m_TextName.text = data.Name;
            DataSource.Clear();
            if (data.ItemDataList != null)
            {
                DataSource.AddRange(data.ItemDataList);
            }
        }

    }
}
