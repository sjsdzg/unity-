using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 物品项
    /// </summary>
    [XmlType("Goods")]
    public class Goods : Item
    {
        public Goods()
        {
            base.Type = ItemType.Goods;
        }

        /// <summary>
        /// 物品项类型
        /// </summary>
        [XmlAttribute("goodsType")]
        public GoodsType GoodsType { get; set; }
    }

    [XmlType("GoodsCollection")]
    public class GoodsCollection : DataObject<GoodsCollection>
    {
        /// <summary>
        /// 物品列表
        /// </summary>
        [XmlElement]
        public List<Goods> Goods { get; set; }

        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        public static GoodsCollection Load(string path, Encoding encoding)
        {
            GoodsCollection collection = null;
            try
            {
                collection = XMLHelper.DeserializeFromFile<GoodsCollection>(path, encoding);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return collection;
        }
    }
}
