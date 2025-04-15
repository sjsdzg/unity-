using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 监视点
    /// </summary>
    public class MonitoringPoint
    {
        private int id;
        /// <summary>
        /// 序列Id
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private bool completed;
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool Completed
        {
            get { return completed; }
            set
            {
                completed = value;
                OnCompleted(completed);
            }
        }

        private object[] content;
        /// <summary>
        /// 内容
        /// </summary>
        public object[] Content
        {
            get { return content; }
            set { content = value; }
        }

        protected virtual void OnCompleted(bool value)
        {

        }
    }

    /// <summary>
    /// 序列监视点
    /// </summary>
    public class SequencePoint : MonitoringPoint
    {
        /// <summary>
        /// 序列检测点完成事件
        /// </summary>
        public event CompletedEventHandler CompletedEvent;

        /// <summary>
        /// 包含的动作检测点列表
        /// </summary>
        public Dictionary<int, ActionPoint> ActionPoints = new Dictionary<int, ActionPoint>();

        /// <summary>
        /// 添加动作检测点
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(int id, ActionPoint point)
        {
            if (!ActionPoints.ContainsKey(id))
            {
                point.CompletedEvent += ActionPoint_OnCompletedEvent;
                ActionPoints.Add(id, point);
            }
        }

        /// <summary>
        /// 移除动作检测点
        /// </summary>
        /// <param name="key">Key.</param>
        public void Remove(int id)
        {
            if (null != ActionPoints && ActionPoints.ContainsKey(id))
            {
                ActionPoints.Remove(id);
            }
        }

        protected override void OnCompleted(bool value)
        {
            base.OnCompleted(value);
            if (CompletedEvent != null)
            {
                CompletedEvent(this, value);
            }
        }

        /// <summary>
        /// 动作监视点完成事件
        /// </summary>
        /// <param name="value"></param>
        private void ActionPoint_OnCompletedEvent(object sender, bool value)
        {
            bool flag = true;
            foreach (var item in ActionPoints.Values)
            {
                if (!item.Completed)
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                Completed = true;
            }
        }
    }

    /// <summary>
    /// 完成事件委托
    /// </summary>
    /// <param name="value"></param>
    public delegate void CompletedEventHandler(object sender, bool value);

    /// <summary>
    /// 动作检测点
    /// </summary>
    public class ActionPoint : MonitoringPoint
    {
        /// <summary>
        /// 动作检测点完成事件
        /// </summary>
        public event CompletedEventHandler CompletedEvent;

        protected override void OnCompleted(bool value)
        {
            base.OnCompleted(value);
            if (CompletedEvent != null)
            {
                CompletedEvent(this, value);
            }
        }
    }

}
