using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TextHeader : MonoBehaviour
    {
        /// <summary>
        /// 选中框单元
        /// </summary>
        private TextHeaderCell[] m_Cells;

        private void Awake()
        {
            m_Cells = transform.GetComponentsInChildren<TextHeaderCell>();
        }

        /// <summary>
        /// 设置表头文本
        /// </summary>
        /// <param name="data"></param>
        public void SetData(Dictionary<string, string> cellTextDict)
        {
            foreach (var key in cellTextDict.Keys)
            {
                TextHeaderCell cell = GetCell(key);

                if (cell == null)
                    return;

                cell.SetValue(cellTextDict[key]);
            }
        }

        /// <summary>
        /// 获取单元格
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TextHeaderCell GetCell(string _name)
        {
            TextHeaderCell cell = null;

            for (int i = 0; i < m_Cells.Length; i++)
            {
                if (m_Cells[i].name == _name)
                {
                    cell = m_Cells[i];
                    break;
                }
            }

            return cell;
        }
    }
}
