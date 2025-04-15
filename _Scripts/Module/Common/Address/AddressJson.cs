using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// �����������
    /// </summary>
    public class AddressJson : DataObject<AddressJson>
    {
        /// <summary>
        /// ����
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// ʡ�� 
        /// </summary>
        public string Subdivision { get; set; }

        /// <summary>
        /// ����
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