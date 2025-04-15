using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;

namespace XFramework.Common
{
    public interface IAction
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        void Execute();

        /// <summary>
        /// 完成
        /// </summary>
        void Completed();

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="error"></param>
        //void Error(Exception error);
        /// <summary>
        /// 状态
        /// </summary>
        //ActionState ActionState { get; set; }
    }
}
