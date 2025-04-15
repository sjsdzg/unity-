using HighlightingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 生产车间 轮廓组件
    /// </summary>
	public class WorkOutLineComponent : MonoBehaviour {

        private List<Highlighter> lineList = new List<Highlighter>();
		// Use this for initialization
		void Start () {
            for (int i = 0; i < transform.childCount; i++)
            {

                Highlighter h = transform.GetChild(i).GetComponent<Highlighter>();
                lineList.Add(h);
            }
		}
		
        /// <summary>
        /// 指定房间个体高亮显示
        /// </summary>
        /// <param name="Name"></param>
		public void ShowHighlight( string Name)
        {
            HideHighlight();
            try
            {
                Highlighter h = lineList.First((x) => { return x.name == Name; });
                h.ConstantOn(Color.green);
            }
            catch(Exception ex) {
                print("未找到 高亮物体：" + Name+"  "+ex.Message);
            }
            
        }

        /// <summary>
        /// 隐藏所有的高亮物体
        /// </summary>
        public void HideHighlight()
        {
            for (int i = 0; i < lineList.Count; i++)
            {
                lineList[i].ConstantOff();
            }
        }
    }
}