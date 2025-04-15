using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    public class ProcessLibraryItemData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 SizeDelta { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [JsonIgnore]
        public Sprite Sprite { get; set; }

        /// <summary>
        /// 变量列表
        /// </summary>
        public List<Variable> Variables { get; set; }
    }
}
