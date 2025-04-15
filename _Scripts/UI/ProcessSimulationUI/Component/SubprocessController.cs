using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 子工艺控制器
    /// </summary>
    public class SubprocessController : MonoBehaviour
    {
        /// <summary>
        /// 子工艺组件列表
        /// </summary>
        private Dictionary<string, SubprocessComponent> components = null;

        /// <summary>
        /// 当前子工艺信息
        /// </summary>
        public SubprocessInfo CurrentInfo { get; private set; }

        void Awake()
        {
            Init();
        }

        /// <summary>
        /// 初始化子工艺组件
        /// </summary>
        private void Init()
        {
            components = new Dictionary<string, SubprocessComponent>();

            foreach (Transform item in transform)
            {
                SubprocessComponent component = item.GetComponent<SubprocessComponent>();
                if (component != null)
                {
                    components.Add(component.name, component);
                }
            }
        }

        /// <summary>
        /// 展示子工艺
        /// </summary>
        /// <param name="info"></param>
        public void Display(SubprocessInfo info)
        {
            if (info == null)
                return;

            if (CurrentInfo != null)
                components[CurrentInfo.Name].Disappear();

            SubprocessComponent component = null;

            if (components.TryGetValue(info.Name, out component))
            {
                component.Disappear();
                component.Appear();
            }

            CurrentInfo = info;
        }

        /// <summary>
        /// 关闭子工艺
        /// </summary>
        public void Close()
        {
            foreach (var item in components.Values)
            {
                item.Disappear();
            }

            CurrentInfo = null;
        }
    }
}
