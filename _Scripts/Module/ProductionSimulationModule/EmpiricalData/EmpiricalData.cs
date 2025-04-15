using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;

namespace XFramework.Module
{
    public class EmpiricalData : DataObject<EmpiricalData>
    {
        public EmpiricalDataType Type { get; set; }

        public List<EmpiricalItem> Items { get; set; }

        /// <summary>
        /// 根据变量赋值
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="productRate"></param>
        /// <param name="productAmount"></param>
        public virtual void SetResultByVariable(string variable, string productRate, string productAmount)
        {
            EmpiricalItem item = Items.Find(x => x.Variable == variable);
            if (item != null)
            {
                item.ProductRate = productRate;
                item.ProductAmount = productAmount;
            }
            else
            {
                //TODO
            }
        }

        /// <summary>
        /// 根据变量，获取实验数据项
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public virtual EmpiricalItem GetResultByVariable(string variable)
        {
            EmpiricalItem item = Items.Find(x => x.Variable == variable);
            return item;
        }
    }

    public enum EmpiricalDataType
    {
        /// <summary>
        /// 温度
        /// </summary>
        Temperature,
        /// <summary>
        /// 时间
        /// </summary>
        Time,
        /// <summary>
        /// 溶剂
        /// </summary>
        Solvent
    }
}
