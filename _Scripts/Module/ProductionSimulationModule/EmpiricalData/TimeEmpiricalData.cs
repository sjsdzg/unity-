using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class TimeEmpiricalData : EmpiricalData
    {
        public TimeEmpiricalData()
        {
            Type = EmpiricalDataType.Time;
            Items = new List<EmpiricalItem>()
            {
                new EmpiricalItem("40"),
                new EmpiricalItem("50"),
                new EmpiricalItem("60"),
                new EmpiricalItem("70"),
                new EmpiricalItem("80"),
                new EmpiricalItem("90"),
                new EmpiricalItem("100"),
                new EmpiricalItem("110"),
                new EmpiricalItem("120"),
                new EmpiricalItem("130"),
                new EmpiricalItem("140"),
                new EmpiricalItem("150"),
            };                     
        }                          
    }                              
}
