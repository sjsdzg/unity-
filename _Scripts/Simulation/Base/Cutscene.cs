using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace XFramework.Simulation
{
    /// <summary>
    /// 过场动画基类
    /// </summary>
    public class Cutscene : MonoBehaviour
    {
        private UnityEvent m_OnCompleted = new UnityEvent();
        /// <summary>
        /// 完成事件
        /// </summary>
        public UnityEvent OnCompleted
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }
    }
}

