using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;

namespace XFramework.Actions
{
    /// <summary>
    /// 由虚到实，由实变虚，来回变化组件
    /// </summary>
    public class ChangeVirtualRealityAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 是否变化
        /// </summary>
        public bool isChange;

        public ChangeVirtualRealityAction(GameObject _gameObject,bool _isChange)
        {
            gameObject = _gameObject;
            isChange = _isChange;
        }

        public override void Execute()
        {
            VirtualRealityComponent m_VirtualReality = gameObject.GetOrAddComponent<VirtualRealityComponent>();
            if (m_VirtualReality != null)
            {
                m_VirtualReality.IsChange = isChange;
                Completed();
            }
        }
    }
}
