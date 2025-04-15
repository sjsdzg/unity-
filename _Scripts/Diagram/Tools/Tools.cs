using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFramework.Core;

namespace XFramework.Diagram
{
    public class UITools : Singleton<UITools>, IUpdate
    {
        /// <summary>
        /// 工具集
        /// </summary>
        private Dictionary<Type, GraphToolBase> m_Tools = new Dictionary<Type, GraphToolBase>();

        private GraphToolBase activeTool;
        /// <summary>
        /// 激活工具
        /// </summary>
        public GraphToolBase ActiveTool
        {
            get { return activeTool; }
            set
            {
                if (activeTool != null)
                    activeTool.Release();

                activeTool = value;
            }
        }

        /// <summary>
        /// 获取工具
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetTool<T>() where T : GraphToolBase, new()
        {
            GraphToolBase tool;
            if (m_Tools.TryGetValue(typeof(T), out tool))
                return (T)tool;

            tool = new T();
            m_Tools.Add(typeof(T), tool);
            return (T)tool;
        }

        public void Update()
        {
            if (ActiveTool != null)
            {
                ActiveTool.Update();
            }
        }
    }
}
