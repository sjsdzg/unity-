using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    [Serializable]
    public class HUDPrafabInfo
    {
        /// <summary>
        /// HUD类型
        /// </summary>
        [SerializeField]
        public HUDType hudType;
        /// <summary>
        /// 预制
        /// </summary>
        [SerializeField]
        public GameObject prefab;
    }
}
