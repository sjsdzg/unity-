using LiquidVolumeFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Actions
{
    public class UpdateBoilingAction : ActionBase
    {
        /// <summary>
        /// 粒子数量
        /// </summary>
        private float amount;

        /// <summary>
        /// 物体
        /// </summary>
        private LiquidVolume gameObject;

        public UpdateBoilingAction(LiquidVolume _gameObject, float _amount)
        {
            gameObject = _gameObject;
            amount = _amount;
        }

        public override void Execute()
        {
            if (gameObject != null)
            {
                gameObject.sparklingAmount = amount;
                Completed();
            }
        }
    }
}
