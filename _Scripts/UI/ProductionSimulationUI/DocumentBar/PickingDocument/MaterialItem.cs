using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 领料项
    /// </summary>
    public class MaterialItem : MonoBehaviour
    {
        /// <summary>
        /// 料名称
        /// </summary>
        private Text Name;

        /// <summary>
        /// 规格
        /// </summary>
        private Text Standard;

        /// <summary>
        /// 质量
        /// </summary>
        private Text Mass;

        void Awake()
        {
            Name = transform.Find("Name/Text").GetComponent<Text>();
            Standard = transform.Find("Standard/Text").GetComponent<Text>();
            Mass = transform.Find("Mass/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="data"></param>
        public void SetParams(MaterialItemData data)
        {
            Name.text = data.Name;
            Standard.text = data.Standard;
            Mass.text = data.Mass;
        }
    }

    /// <summary>
    /// 领料项数据
    /// </summary>
    public class MaterialItemData
    {
        /// <summary>
        /// 料名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Standard { get; set; }

        /// <summary>
        /// 质量
        /// </summary>
        public string Mass { get; set; }
    }
}
