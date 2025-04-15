using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 知识点基类
    /// </summary>
    public abstract class BaseKnowledgePoint : MonoBehaviour
    {
        /// <summary>
        /// 获取知识点类型
        /// </summary>
        /// <returns></returns>
        public abstract KnowledgePointType GetKnowledgePointType();

        /// <summary>
        /// 展示知识点
        /// </summary>
        public abstract void Display();

        /// <summary>
        /// 隐藏知识点
        /// </summary>
        public abstract void Close();

    }
}