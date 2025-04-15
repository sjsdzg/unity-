using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using System.Xml.Serialization;

namespace XFramework.Module
{
    [XmlType("IntroductionPointCollection")]
    public class IntroductionPointCollection : DataObject<IntroductionPointCollection>
    {
        [XmlArray("IntroductionPoints")]
        [XmlArrayItem("IntroductionPoint")]
        public List<IntroductionPoint> IntroductionPoints { get; set; }

    }
}
