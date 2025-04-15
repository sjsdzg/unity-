using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 左侧流程UI面板
    /// </summary>
	public class ProcessBarUI : MonoBehaviour {
        public class ProcessBarClickEvent : UnityEvent<string> { }

        private ProcessBarClickEvent m_ClickProcessBar = new ProcessBarClickEvent();

        public ProcessBarClickEvent OnClickProcessBar
        {
            get { return m_ClickProcessBar; }
            set { m_ClickProcessBar = value; }
        }
        /// <summary>
        /// 片剂
        /// </summary>
        private ProcessItem tablet;

        /// <summary>
        /// 胶囊
        /// </summary>
        private ProcessItem capsule;

        public bool IsPlaying { get; set; }
        private void Start()
        {

            tablet = transform.Find("片剂流程").GetComponent<ProcessItem>();
            tablet.OnClickProcessEvent.AddListener(OnClickEvent);
            capsule = transform.Find("胶囊流程").GetComponent<ProcessItem>();
            capsule.OnClickProcessEvent.AddListener(OnClickEvent);
        }

        private  void OnClickEvent(string name)
        {
            OnClickProcessBar.Invoke(name);
        }

        /// <summary>
        /// 显示左侧流程列表
        /// </summary>
        /// <param name="index"></param>
        public void ShowProcessList(int  index)
        {
            if (capsule.IsPlaying || tablet.IsPlaying)
                return;
            if(index==0)
            {
                if(capsule.IsShow)
                {
                    capsule.HideList(() => { tablet.ShowList(); });
                }
                else
                {
                    tablet.ShowList();
                }
            }
            else if(index==1)
            {
                if(tablet.IsShow)
                {
                    tablet.HideList(() => { capsule.ShowList(); });
                }
                else
                {
                    capsule.ShowList();
                }
            }
        }
       
    }
}