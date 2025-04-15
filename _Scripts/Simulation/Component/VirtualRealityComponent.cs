using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.Component
{
    public class VirtualRealityComponent : ComponentBase
    {
        private bool isChange = false;

        public bool IsChange
        {
            get
            { return isChange;}
            set
            {
                isChange = value;
            }
        }

        /// <summary>
        /// 当前透明度
        /// </summary>
        private float transparency;

        /// <summary>
        /// 透明度是否加
        /// </summary>
        private bool isAdd = true;

        /// <summary>
        /// 是否恢复到初始状态
        /// </summary>
        private bool isOver = false;

        private void Update()
        {
            if (isAdd)
            {
                if (transparency >= 1)
                {
                    isAdd = false;
                }
                else
                {
                    transparency += 0.02f;
                }
            }
            else
            {
                if (transparency <= 0)
                {
                    isAdd = true;
                }
                else
                {
                    transparency -= 0.02f;
                }
            }
            if (isChange)
            {
                TransparentHelper.SetObjectAlpha(gameObject, transparency);
            }
            else
            {
                if (!isOver)
                {
                    isOver = true;
                    TransparentHelper.RestoreBack(gameObject);
                }
            }
        }
    }
}
