using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class PLC_RightBar_ChangeStepColor : MonoBehaviour
    {
        private Image img_Bg;

        void Awake()
        {
            img_Bg = transform.Find("Bg").GetComponent<Image>();
        }

        /// <summary>
        /// 改变当前运行阶段背景的颜色
        /// </summary>
        /// <param name="color"></param>
        public void ChangeStepClolor(string color)
        {
            if (color == "red")
            {
                img_Bg.color = Color.red;
            }

            if (color == "blue")
            {
                img_Bg.color = Color.blue;
            }
        }
    }
}
