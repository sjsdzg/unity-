using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Architectural
{
    public interface IEntityObject
    {
        ///// <summary>
        ///// 专业分类
        ///// </summary>
        //string Special { get; set; }

        /// <summary>
        /// 元件类型
        /// </summary>
        EntityType Type { get; }
    }
}
