using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Common
{
    /// <summary>
    /// GameOjectAction
    /// </summary>
    public class GameObjectAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 状态
        /// </summary>
        public bool activeState;

        public GameObjectAction(GameObject _gameObject, bool _activeState = true)
        {
            gameObject = _gameObject;
            activeState = _activeState;
        }
        public GameObjectAction(Transform _tran, bool _activeState = true)
        {
            gameObject = _tran.gameObject;
            activeState = _activeState;
        }
        /// <summary>
        /// 执行
        /// </summary>
        public override void Execute()
        {
            gameObject.SetActive(activeState);
            Completed();
        }
    }

    /// <summary>
    /// 显示方式
    /// </summary>
    public enum ShowStyle
    {
        /// <summary> 正常出现 </summary>
        Nomal,
        /// <summary> 中间由小变大 </summary>
        CenterScaleBigNomal,
        /// <summary> 由上往中间 </summary>
        UpToCenterSlide,
        /// <summary> 由下往中间 </summary>
        DownToCenterSlide,
        /// <summary> 由中间往上 </summary>
        CenterToUpSlide,
        /// <summary> 由中间往下 </summary>
        CenterToDownSlide,
        ///// <summary> 左往中 </summary>
        //LeftToSlide,
        ///// <summary> 右往中 </summary>
        //RightToSlide,
    }
}
