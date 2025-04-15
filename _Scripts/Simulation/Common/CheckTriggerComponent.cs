using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Simulation;

namespace XFramework.Component
{
    public class CheckTriggerComponent : ComponentBase
    {
        /// <summary>
        /// 被检测的物体的tag
        /// </summary>
        public string _tag;

        /// <summary>
        /// 被检测的物体是否触发，在本物体内
        /// </summary>
        private bool isEnter;

        public bool IsEnter
        {
            get
            {
                return isEnter;
            }

            set
            {
                isEnter = value;
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other != null && other.tag == _tag)
            {
                IsEnter = true;
                Debug.Log("人物在里面");
            }
            else
            {
                Debug.Log("没人");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IsEnter = false;
            Debug.Log("人物出来了");
        }
    }
}
