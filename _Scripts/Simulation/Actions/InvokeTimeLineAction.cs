using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Simulation;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

namespace XFramework.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public class InvokeTimeLineAction : ActionBase
    {
        /// <summary>
        /// TimeLine物体
        /// </summary>
        private  GameObject obj;

        private bool isCompleted;
        /// <summary>
        /// 播放顺序
        /// </summary>
        private int playOrder;
        PlayableDirector playable;
        float  waitTime;

        public InvokeTimeLineAction(GameObject _obj,int _playOrder=1, bool _isCompleted =false ,float _waitTime = 0f)
        {
            obj = _obj;
            playOrder = _playOrder;
            isCompleted = _isCompleted;
            waitTime = _waitTime;
        }

        public override void Execute()
        {
            playable = obj.GetComponent<PlayableDirector>();
            if (playOrder==1)
            {
                playable.Play();                   
            }
            else
            {
                playable.Pause();
                CoroutineManager.Instance.StartCoroutine(RePlay());
            }
            if (waitTime > 0)
            {
                CoroutineManager.Instance.Invoke(waitTime, () =>
                {
                    playable.Pause();
                    Completed();
                });
                return;
            }
            if (isCompleted)
            {
                Completed();
            }
            else
            {
                float time = (float)playable.duration ;
                CoroutineManager.Instance.Invoke(time, () =>
                 {
                     Completed();
                 });
            }           
        }

        /// <summary>
        /// 倒放
        /// </summary>
        private IEnumerator RePlay()
        {
            yield return new WaitForSeconds(0.001f * Time.deltaTime);
            playable.time -= Time.deltaTime * 1f;
            playable.Evaluate();
            if (playable.time<0f)
            {
                playable.time = 0f;
                playable.Evaluate();
                Completed();
            }
            else
            {
                CoroutineManager.Instance.StartCoroutine(RePlay());
            }
        }
    }
}
