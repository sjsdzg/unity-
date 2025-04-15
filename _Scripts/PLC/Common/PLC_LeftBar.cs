using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class PLC_LeftBar : MonoBehaviour
    {                
        /// <summary>
        /// 执行状态内容
        /// </summary>
        [HideInInspector]
        public static Text textRecord;

        /// <summary>
        /// 执行状态垂直的滑块随内容增加而到最新内容地方
        /// </summary>
        [HideInInspector]
        public static Scrollbar scrollBar;

        public static StringBuilder leftBar_WorkDateContent = new StringBuilder();
        void Awake()
        {
            OnAwake();
        }

        public virtual void OnAwake()
        {
            //执行状态记录       
            textRecord = transform.Find("Equip/OperateStatus/Scroll View/Panel/Viewport/Text_Record").GetComponent<Text>();
            //执行状态垂直的滑块随内容增加而到最新内容地方        
            scrollBar = transform.Find("Equip/OperateStatus/Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        }

        /// <summary>
        /// leftBar中的操作记录
        /// </summary>
        /// <param name="content"></param>
        public void LeftBar_ShowWorkDate(string content)
        {
            leftBar_WorkDateContent.Append(DateTime.Now.ToString("hh:mm:ss") + "    " + content + "\n");

            textRecord.text = leftBar_WorkDateContent.ToString();
        }
    }
}
