using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    [Serializable]
    public class HUDPrafabInfoList
    {
        [SerializeField]
        public List<HUDPrafabInfo> HUDPrafabInfos = new List<HUDPrafabInfo>();

        /// <summary>
        /// 根据名称，获取Sprite。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public GameObject this[HUDType key]
        {
            get
            {
                if (HUDPrafabInfos == null)
                    return null;

                HUDPrafabInfo HUDPrafabInfo = HUDPrafabInfos.FirstOrDefault(x => x.hudType == key);

                if (HUDPrafabInfo != null)
                {
                    return HUDPrafabInfo.prefab;
                }
                else
                    return null;
            }
        }
    }
}
