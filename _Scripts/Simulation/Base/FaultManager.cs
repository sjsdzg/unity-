using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Simulation
{
    /// <summary>
    /// 故障管理器
    /// </summary>
    public class FaultManager : Singleton<PipeFittingManager>
    {
        /// <summary>
        /// 故障字典
        /// </summary>
        private Dictionary<string, Transform> m_FaultTransforms = new Dictionary<string, Transform>();

        private string currentFaultID;
        /// <summary>
        /// 当前故障ID
        /// </summary>
        public string CurrentFaultID
        {
            get { return currentFaultID; }
            set
            {
                currentFaultID = value;
                foreach (var item in m_FaultTransforms.Keys)
                {
                    if (item == currentFaultID)
                    {
                        m_FaultTransforms[currentFaultID].gameObject.SetActive(true);
                    }
                    else
                    {
                        m_FaultTransforms[item].gameObject.SetActive(false);
                    }
                }
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="faultParent"></param>
        public void Init(Transform faultParent)
        {
            foreach (Transform child in faultParent)
            {
                m_FaultTransforms.Add(child.name, child);
            }
        }
    }
}
