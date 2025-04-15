using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFramework.Core;

namespace XFramework.UI
{
    public class ProcessLibraryData : DataObject<ProcessLibraryData>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ItemDataList
        /// </summary>
        public List<ProcessLibraryItemData> ItemDataList { get; set; }

        public void ConvertVariablesType()
        {
            if (ItemDataList == null)
                return;

            foreach (var itemData in ItemDataList)
            {
                if (itemData == null || itemData.Variables == null)
                    continue;

                foreach (var variable in itemData.Variables)
                {
                    variable.ConvertType();
                }
            }
        }
    }
}
