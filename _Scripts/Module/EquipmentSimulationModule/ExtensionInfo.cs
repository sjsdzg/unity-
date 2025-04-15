using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using System.Xml.Serialization;

namespace XFramework.Module
{
    [XmlType("ExtensionInfo")]
    public class ExtensionInfo : DataObject<ExtensionInfo>
    {
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<ExtensionItemInfo> Items { get; set; }
    }
}
