using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class TemperatureEmpiricalData : EmpiricalData
    {
        //public EmpiricalDataType Type { get; set; }

        //public List<EmpiricalItem> Items { get; set; }

        public TemperatureEmpiricalData()
        {
            Type = EmpiricalDataType.Temperature;
            Items = new List<EmpiricalItem>()
            {
                new EmpiricalItem("25"),
                new EmpiricalItem("30"),
                new EmpiricalItem("35"),
                new EmpiricalItem("40"),
                new EmpiricalItem("45"),
                new EmpiricalItem("50"),
                new EmpiricalItem("55"),
                new EmpiricalItem("60"),
                new EmpiricalItem("65"),
            };
        }
    }
}
