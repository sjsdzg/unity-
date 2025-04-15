using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺元件组件
    /// </summary>
    public class ProcessElementComponent : MonoBehaviour
    {
        private float transparent = 0.2f;
        /// <summary>
        /// 透明度
        /// </summary>
        public float Transparent
        {
            get { return transparent; }
            set
            {
                transparent = value;

                if (transparent < 0.8f)
                {
                    TransparentHelper.SetObjectAlpha(gameObject, transparent);
                }
                else
                {
                    TransparentHelper.RestoreBack(gameObject);
                }
            }
        }

    }
}
