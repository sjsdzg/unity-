using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;
using XFramework.Component;

namespace XFramework.Common
{
    /// <summary>
    /// 转动一定角度
    /// </summary>
    public class RotateAction : ActionBase
    {
        /// <summary>
        /// gameObject
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 旋转角度
        /// </summary>
        public Vector3 m_rot;

        /// <summary>
        /// 旋转的时间
        /// </summary>
        public float m_Time;

        /// <summary>
        /// 是否立刻返回完成
        /// </summary>
        public bool immediate;
        /// <summary>
        /// 旋转中
        /// </summary>
        private bool isRotating = false;
        public RotateAction(GameObject _gameObject,Vector3 rot ,float time,bool _immediate = false)
        {
            gameObject = _gameObject;
            m_rot = rot;
            m_Time = time;
            immediate = _immediate;
        }
        public override void Execute()
        {
            if (gameObject != null)
            {
                if (isRotating)
                {
                    return;
                }
                isRotating = true;
                if (immediate)
                {
                    gameObject.transform.DOLocalRotate(m_rot, m_Time, RotateMode.LocalAxisAdd).OnComplete(()=> {
                        isRotating = false;
                    });
                    Completed();
                }
                else
                {
                    gameObject.transform.DOLocalRotate(m_rot, m_Time, RotateMode.LocalAxisAdd).OnComplete(() => {
                        isRotating = false;
                        Completed();
                    });
                }
            }
            else
            {
                Error(new Exception("gameObject is null"));
            }
        }
    }
}
