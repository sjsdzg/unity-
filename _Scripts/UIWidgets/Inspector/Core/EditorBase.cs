using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.UIWidgets
{
    public abstract class EditorBase : InspectorBehaviour
    {
        public abstract void Bind(object target);
    }

    public class EditorBase<T> : EditorBase 
    {
        /// <summary>
        /// 目标
        /// </summary>
        public T Target { get; protected set; }

        private List<InspectorBehaviour> m_Components = new List<InspectorBehaviour>();
        /// <summary>
        /// Components
        /// </summary>
        public List<InspectorBehaviour> Components 
        { 
            get
            {
                return m_Components;
            } 
        }

        public override string GetKey()
        {
            return typeof(T).ToString();
        }

        public override void Bind(object target)
        {
            Target = (T)target;
            BuildEditor();
        }

        /// <summary>
        /// 构建编辑器
        /// </summary>
        public virtual void BuildEditor()
        {

        }

        public override void Unbind()
        {
            base.Unbind();
            foreach (var component in Components)
            {
                component.Unbind();
            }
            Components.Clear();
        }
    }
}
