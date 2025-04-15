using System;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.Component
{
    public class PipeComponent : ComponentBase
    {
        private bool transparent = false;
        /// <summary>
        /// 获取或设置管道是否透明
        /// </summary>
        public bool Transparent
        {
            get { return transparent; }
            set
            {
                if (transparent == value)
                    return;

                transparent = value;
                OnTransparent(transparent);
            }
        }

        /// <summary>
        /// 是否管道透明
        /// </summary>
        /// <param name="isTransparent"></param>
        public virtual void OnTransparent(bool isTransparent)
        {
            if (isTransparent)
            {
                TransparentHelper.SetObjectAlpha(gameObject, 0.3f);
            }
            else
            {
                TransparentHelper.RestoreBack(gameObject);
            }
        }
    }
}
