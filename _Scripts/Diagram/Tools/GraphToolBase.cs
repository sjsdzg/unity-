using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Diagram
{
    public class GraphToolBaseArgs
    {
        public static readonly GraphToolBaseArgs Empty = new GraphToolBaseArgs();
    }

    public abstract class GraphToolBase
    {
        private Color color = Color.white;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init(GraphToolBaseArgs t)
        {

        }

        /// <summary>
        /// 更新
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// 提交
        /// </summary>
        public virtual void Submit()
        {

        }

        /// <summary>
        /// 取消
        /// </summary>
        public virtual void Cancel()
        {

        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Release()
        {

        }
    }
}
