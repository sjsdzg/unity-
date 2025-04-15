using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public static class MarkerUtils
    {
        private static MarkerMap markerMap = null;
        /// <summary>
        /// 获取MarkerMap
        /// </summary>
        public static MarkerMap GetMarkerMap
        {
            get
            {
                if (markerMap == null)
                {
                    markerMap = GameObject.FindObjectOfType<MarkerMap>();
                }
                return markerMap;
            }
        }
    }
}
