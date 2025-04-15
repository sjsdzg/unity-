using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 批量操作结果
    /// </summary>
    public class AddressJson : DataObject<AddressJson>
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省份 
        /// </summary>
        public string Subdivision { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!Country.Equals("0"))
            {
                sb.Append(Country + " ");
            }

            if (!Subdivision.Equals("0"))
            {
                sb.Append(Subdivision + " ");
            }

            sb.Append(City);

            return sb.ToString();
        }
    }
}