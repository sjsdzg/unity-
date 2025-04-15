using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Component;
using XFramework.Module;
using XFramework.Simulation;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺系统组件
    /// </summary>
    public class ProcessSystemComponent : MonoBehaviour
    {
        /// <summary>
        /// 工艺信息
        /// </summary>
        public ProcessInfo ProcessInfo { get; private set; }

        /// <summary>
        /// 最佳视角组件
        /// </summary>
        private FocusComponent m_Focus;

        /// <summary>
        /// 子工艺控制器
        /// </summary>
        private SubprocessController subprocessControl;

        /// <summary>
        /// 工艺元件控制器
        /// </summary>
        private ProcessElementController processElmentControl;

        void Awake()
        {
            m_Focus = transform.Find("Focus").GetComponent<FocusComponent>();
            subprocessControl = transform.Find("子工艺列表").GetComponent<SubprocessController>();
            processElmentControl = transform.Find("工艺元件").GetComponent<ProcessElementController>();
        }

        /// <summary>
        /// 设置工艺信息
        /// </summary>
        /// <param name="info"></param>
        public virtual void SetProcessInfo(ProcessInfo info)
        {
            m_Focus.Focus();
            ProcessInfo = info;
        }

        /// <summary>
        /// 展示子工艺
        /// </summary>
        /// <param name="info"></param>
        public void DisplaySubprocess(SubprocessInfo info)
        {
            subprocessControl.Display(info);
            processElmentControl.SetBlurEffect(info.ProcessElements);
        }

        /// <summary>
        /// 设置工艺元件透明度
        /// </summary>
        /// <param name="f"></param>
        public void SetTransparent(float f)
        {
            processElmentControl.Transparent = f;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            m_Focus.Focus();
            subprocessControl.Close();
            processElmentControl.Restore();
        }
    }
}
