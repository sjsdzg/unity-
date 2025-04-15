using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Diagram
{
    /// <summary>
    /// 流程节点连接
    /// </summary>
    public class NodeConnection : Unit
    {
        private Node source;
        /// <summary>
        /// 源
        /// </summary>
        public Node Source
        {
            get { return source; }
            set { SetProperty(ref source, value, "source"); }
        }

        private Vector2 sourcePoint;
        /// <summary>
        /// 连接的源点
        /// </summary>
        public Vector2 SourcePoint
        {
            get { return sourcePoint; }
            set { SetProperty(ref sourcePoint, value, "sourcePoint"); }
        }

        private Node target;
        /// <summary>
        /// 目标
        /// </summary>
        public Node Target
        {
            get { return target; }
            set { SetProperty(ref target, value, "target"); }
        }

        private Vector2 targetPoint;
        /// <summary>
        /// 连接的目标点
        /// </summary>
        public Vector2 TargetPoint
        {
            get { return targetPoint; }
            set { SetProperty(ref targetPoint, value, "targetPoint"); }
        }

        private List<Vector2> points;
        /// <summary>
        /// 控制点列表 （中间点）
        /// </summary>
        public List<Vector2> Points
        {
            get { return points; }
            set { points = value; }
        }

        /// <summary>
        /// 获取连接源的位置
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSourcePosition()
        {
            if (source == null)
                return SourcePoint;

            return GraphUtils.GetPosition(source, sourcePoint);
        }

        /// <summary>
        /// 获取连接目标的位置
        /// </summary>
        /// <returns></returns>
        public Vector2 GetTargetPosition()
        {
            if (target == null)
                return targetPoint;

            return GraphUtils.GetPosition(target, targetPoint);
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            switch (propertyName)
            {
                case "source":
                    PropertyChangedUtils.RemoveEvent(oldValue, source_PropertyChanged);
                    PropertyChangedUtils.AddEvent(newValue, source_PropertyChanged);
                    OnVerticesChanged();
                    break;
                case "sourcePoint":
                    OnVerticesChanged();
                    break;
                case "target":
                    PropertyChangedUtils.RemoveEvent(oldValue, target_PropertyChanged);
                    PropertyChangedUtils.AddEvent(newValue, target_PropertyChanged);
                    OnVerticesChanged();
                    break;
                case "targetPoint":
                    OnVerticesChanged();
                    break;
                default:
                    break;
            }
        }

        private void source_PropertyChanged(object sender, PropertyChangedArgs e)
        {
            OnVerticesChanged();
        }

        private void target_PropertyChanged(object sender, PropertyChangedArgs e)
        {
            OnVerticesChanged();
        }


    }
}
