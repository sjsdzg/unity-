using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 溶剂实验数据
    /// </summary>
    public class SolventEmpiricalData : EmpiricalData
    {
        public SolventEmpiricalData()
        {
            Type = EmpiricalDataType.Solvent;
            Items = new List<EmpiricalItem>()
            {
                new EmpiricalItem("600"),
                new EmpiricalItem("700"),
                new EmpiricalItem("800"),
                new EmpiricalItem("900"),
                new EmpiricalItem("1000"),
                new EmpiricalItem("1100"),
                new EmpiricalItem("1200"),

            };
        }


    }
}
