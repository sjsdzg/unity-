using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Module
{
    /// <summary>
    /// 关联关系
    /// </summary>
    public class Relation
    {
        /// <summary>
        /// 元件名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 属性
        /// </summary>
        public string Property { get; private set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="relat"></param>
        public Relation(string relat)
        {
            relat = relat.Substring(1,relat.IndexOf('}') - 1);
            Debug.Log(relat);
            string[] strs = relat.Split(',');
            //设置属性
            Name = strs[0].Split('=')[1];
            Property = strs[1].Split('=')[0];
            Value = strs[1].Split('=')[1];
        }
    }
}
