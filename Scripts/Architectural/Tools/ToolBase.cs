using UnityEngine;
using XFramework.Core;
using XFramework.Math;

namespace XFramework.Architectural
{
    /// <summary>
    /// 编辑模式
    /// </summary>
    public enum EditMode
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 创建
        /// </summary>
        Create,
        /// <summary>
        /// 修改
        /// </summary>
        Modify,
    }

    /// <summary>
    /// 工具参数
    /// </summary>
    public class ToolArgs
    {
        public static readonly ToolArgs Empty = new ToolArgs();

        public ToolArgs()
        {

        }
    }

    public abstract class ToolBase
    {
        protected static readonly MeshBuilder s_VertexHelper = new MeshBuilder();

        /// <summary>
        /// s_VertexHelperPool
        /// </summary>
        protected static readonly ObjectPool<MeshBuilder> s_VertexHelperPool = new ObjectPool<MeshBuilder>(null, x => x.Clear());

        /// <summary>
        /// s_MeshPool
        /// </summary>
        protected static readonly ObjectPool<Mesh> s_MeshPool = new ObjectPool<Mesh>(null, x => x.Clear());

        /// <summary>
        /// 置顶
        /// </summary>
        public bool TopMost { get; set; }

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
        public virtual void Init(ToolArgs t)
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