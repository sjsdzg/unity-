using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.UI
{
    /// <summary>
    /// 表格行数据
    /// </summary>
    [Serializable]
    public class DataGridViewRowData
    {
        /// <summary>
        /// 绑定对象
        /// </summary>
        public object Tag { get; set; }

        private Dictionary<string, object> m_CellValueDict = new Dictionary<string, object>();
        /// <summary>
        /// 单元格数据（字典类型）
        /// </summary>
        public Dictionary<string, object> CellValueDict
        {
            get { return m_CellValueDict; }
            set { m_CellValueDict = value; }
        }

    }
}
