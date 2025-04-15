using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 文本单元
    /// </summary>
    public class DataGridViewTextCell : DataGridViewCell
    {
        public override CellType GetCellType()
        {
            return CellType.Text;
        }

        /// <summary>
        /// 文本
        /// </summary>
        private Text text;

        protected override void OnAwake()
        {
            base.OnAwake();
            text = transform.GetComponentInChildren<Text>();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="data"></param>
        public override void SetValue(object data)
        {
            text.text = data.ToString();
            Value = data;
        }
    }
}
