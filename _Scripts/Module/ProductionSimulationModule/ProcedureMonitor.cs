using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace XFramework.Module
{
    /// <summary>
    /// 流程监视器
    /// </summary>
    public class ProcedureMonitor
    {
        private Dictionary<int, SequencePoint> SequencePoints = new Dictionary<int, SequencePoint>();

        /// <summary>
        /// 序列监视点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SequencePoint this[int id]
        {
            get
            {
                if (!SequencePoints.ContainsKey(id))
                    return null;

                return SequencePoints[id];
            }
        }

        /// <summary>
        /// 动作监视点
        /// </summary>
        /// <param name="seqId"></param>
        /// <param name="actId"></param>
        /// <returns></returns>
        public ActionPoint this[int seqId, int actId]
        {
            get
            {
                if (!SequencePoints.ContainsKey(seqId))
                    return null;

                SequencePoint seqPoint = SequencePoints[seqId];

                if (seqPoint == null || !seqPoint.ActionPoints.ContainsKey(actId))
                    return null;

                return seqPoint.ActionPoints[actId];
            }
        }

        private int monitorCount;
        /// <summary>
        /// 队列监视点数量
        /// </summary>
        public int MonitorCount
        {
            get
            {
                monitorCount = SequencePoints.Count;
                return monitorCount;
            }
            set { monitorCount = value; }
        }

        /// <summary>
        /// 添加队列监视点
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(int id, SequencePoint point)
        {
            if (!SequencePoints.ContainsKey(id))
            {
                point.CompletedEvent += SequencePoint_CompletedEvent;
                SequencePoints.Add(id, point);
            }
        }

        /// <summary>
        /// 队列监视点完成触发事件
        /// </summary>
        /// <param name="value"></param>
        private void SequencePoint_CompletedEvent(object sender, bool value)
        {
            if (value)
            {
                SequencePoint point = sender as SequencePoint;
                //Debug.LogFormat("完成流程：{0}", point.Id);
            }
        }

        /// <summary>
        /// 移除队列监视点
        /// </summary>
        /// <param name="key">Key.</param>
        public void Remove(int id)
        {
            if (null != SequencePoints && SequencePoints.ContainsKey(id))
            {
                SequencePoints.Remove(id);
            }
        }

        /// <summary>
        /// 某序列监视点完成
        /// </summary>
        /// <param name="seqId"></param>
        /// <param name="_params"></param>
        public void Completed(int seqId, params object[] _params)
        {
            SequencePoint point = this[seqId];
            if (point != null)
            {
                point.Completed = true;
            }
        }

        /// <summary>
        /// 某序列下的某动作监视点完成
        /// </summary>
        /// <param name="seqId"></param>
        /// <param name="actId"></param>
        /// <param name="_params"></param>
        public void Completed(int seqId, int actId, params object[] _params)
        {
            ActionPoint point = this[seqId, actId];
            if (point != null)
            {
                point.Completed = true;
            }
        }
        public void PrintListInfo()
        {

            for (int i = 1; i <=MonitorCount; i++)
            {

                for (int j = 1; j <=this[i].ActionPoints.Count; j++)
                {
                    MonoBehaviour.print("Sequence:" + (i) + "  " +this[i].Completed  + " Action:" +j+"  "+this[i,j].Completed);
                }
            }

        }

        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <returns></returns>
        public ProcedureMonitor Clone()
        {
            ProcedureMonitor monitor = new ProcedureMonitor();
            foreach (SequencePoint seq in SequencePoints.Values)
            {
                //队列监视点
                SequencePoint sequencePoint = new SequencePoint();
                sequencePoint.Id = seq.Id;

                foreach (ActionPoint _action in seq.ActionPoints.Values)
                {
                    ActionPoint actionPoint = new ActionPoint();
                    actionPoint.Id = _action.Id;
                    sequencePoint.Add(_action.Id, actionPoint);
                }
                monitor.Add(sequencePoint.Id, sequencePoint);
            }
            return monitor;
        }
    }
}
