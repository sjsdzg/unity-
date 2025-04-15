using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Simulation
{
    /// <summary>
    /// 设置物体透明度
    /// </summary>
    public class UpdateTransparencyAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// 是否透明
        /// </summary>
        private bool isTransparent;

        /// <summary>
        /// 目标透明度
        /// </summary>
        private float transparency;

        ///// <summary>
        ///// 持续时间
        ///// </summary>
        //private float duration;

        ///// <summary>
        ///// 当前透明度
        ///// </summary>
        //private float nowTransparency;

        ///// <summary>
        ///// 是否立刻返回完成
        ///// </summary>
        //private bool immediate;

        public UpdateTransparencyAction(GameObject _gameObject,bool _isTransparent, float _transparency = 0.3f)
        {
            gameObject = _gameObject;
            isTransparent = _isTransparent;
            transparency = _transparency;
            //duration = _duration;
            //immediate = _immediate;
        }

        public override void Execute()
        {
            if (gameObject != null)
            {
                if (isTransparent)
                {
                    TransparentHelper.SetObjectAlpha(gameObject, transparency);
                    Completed();
                    //if (duration == 0)
                    //{
                    //    TransparentHelper.SetObjectAlpha(gameObject, transparency);
                    //    Completed();
                    //}
                    //else
                    //{
                    //    //CoroutineManager.Instance.StartCoroutine(UpdateTransparency(transparency, duration));
                    //    if (immediate)
                    //    {
                    //        Completed();
                    //    }
                    //}
                }
                else
                {
                    TransparentHelper.RestoreBack(gameObject);
                    Completed();
                }
            }
        }

        //IEnumerator UpdateTransparency(float _trans,float _time)
        //{
        //    float addTransparency = 0.1f / duration;
        //    while (transparency > nowTransparency)
        //    {
        //        yield return new WaitForSeconds(0.1f);
        //        nowTransparency += addTransparency;
        //        TransparentHelper.SetObjectAlpha(gameObject, nowTransparency);
        //    }
        //    if (!immediate)
        //    {
        //        Completed();
        //    }
        //}
    }
}
