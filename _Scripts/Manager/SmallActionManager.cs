using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework
{
    public class SmallActionManager : Singleton<SmallActionManager>
    {
        private Dictionary<string, bool> smallActionDict = new Dictionary<string, bool>();

        public bool CheckSmallAction(string name,bool status)
        {
            if (!smallActionDict.ContainsKey(name))
            {
                smallActionDict.Add(name, false);
            }
           // Debug.Log("检查小步骤：" + name + "状态：" + smallActionDict[name]);
            return smallActionDict[name] == status;
        }
        public void UpdateSmallAction(string name, bool status)
        {
            if (smallActionDict.ContainsKey(name))
            {
                smallActionDict[name]= status;
               // Debug.Log("更新小步骤：" + name + "状态：" + smallActionDict[name]);
            }
            else
            {
                smallActionDict.Add(name, status);
            }
        }
        public Dictionary<string, bool> SmallActionDict
        {
            get
            {
                return smallActionDict;
            }
            set
            {
                smallActionDict = value;
            }
        }
    }
}