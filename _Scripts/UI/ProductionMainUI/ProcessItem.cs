using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 流程组个体
    /// </summary>
	public class ProcessItem : MonoBehaviour {

        /// <summary>
        /// 片剂流程列表
        /// </summary>
        public List<Button> ProcessList;
        public bool IsShow { get; set; }

        public bool IsPlaying { get; set; }

        public class ProcessBarClickEvent : UnityEvent<string> { }

        private ProcessBarClickEvent m_ClickProcessBar = new ProcessBarClickEvent();

        public ProcessBarClickEvent OnClickProcessEvent
        {
            get { return m_ClickProcessBar; }
            set { m_ClickProcessBar = value; }
        }

        /// <summary>
        /// 列表x轴初始化点
        /// </summary>
        private float initX = -54.8f;

        private float offset = 120;
        private void Start()
        {
            ///初始化胶囊流程列表
            for (int i = 0; i < transform.childCount; i++)
            {
                Button btn = transform.GetChild(i).GetComponent<Button>();
                ProcessList.Add(btn);
                btn.onClick.AddListener(() => { OnClickEvent(btn.name); });
            }
            init();
        }
        private void init()
        {
            for (int i = 0; i < ProcessList.Count; i++)
            {
                Vector3 initPos = ProcessList[i].transform.localPosition;
                ProcessList[i].transform.DOLocalMoveX(initPos.x - offset, 0.1f);
            }
        }
        public void OnClickEvent(string name)
        {
            OnClickProcessEvent.Invoke(name);
        }
        /// <summary>
        /// 显示列表
        /// </summary>
        public void ShowList()
        {
            if (IsPlaying || IsShow)
                return;
            IsPlaying = true;
            for (int i = 0; i < ProcessList.Count; i++)
            {
                Vector3 initPos = ProcessList[i].transform.localPosition;
                ProcessList[i].transform.DOLocalMoveX(initPos.x + offset, 0.5f).SetDelay(i * 0.1f); ;
                if (i == ProcessList.Count - 1)
                {
                    ProcessList[i].transform.DOLocalMoveX(initPos.x + offset, 0.5f).SetDelay(i * 0.1f).OnComplete(() => {
                        IsPlaying = false;
                        IsShow = true;
                    }); ;
                }
                else
                {
                    ProcessList[i].transform.DOLocalMoveX(initPos.x + offset, 0.5f).SetDelay(i * 0.1f);
                }
            }

        }
        public void HideList(Action _action)
        {
            if (IsPlaying || !IsShow)
                return;
            IsPlaying = true;
            for (int i = 0; i < ProcessList.Count; i++)
            {
                Vector3 initPos = ProcessList[i].transform.localPosition;
                if (i == ProcessList.Count - 1)
                {
                    ProcessList[i].transform.DOLocalMoveX(initPos.x - offset, 0.5f).SetDelay(i * 0.1f).OnComplete(() => {
                        IsPlaying = false;
                        IsShow = false;
                        _action.Invoke();
                    }); ;
                }
                else
                {

                    ProcessList[i].transform.DOLocalMoveX(initPos.x - offset, 0.5f).SetDelay(i * 0.1f);
                }
            }
        }
    }
}