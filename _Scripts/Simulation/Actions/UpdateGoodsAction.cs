using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.Module;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class UpdateGoodsAction : ActionBase
    {
        /// <summary>
        /// 物品名称（唯一）
        /// </summary>
        public string itemName;

        /// <summary>
        /// 更新模式
        /// </summary>
        public UpdateType updateType;

        public UpdateGoodsAction(string _itemName, UpdateType _updateType)
        {
            itemName = _itemName;
            updateType = _updateType;
        }

        public override void Execute()
        {
            switch (updateType)
            {
                case UpdateType.Add:
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, itemName);
                    Completed();
                    break;
                case UpdateType.Remove:
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, itemName);
                    Completed();
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 更新类型
    /// </summary>
    public enum UpdateType
    {
        Add,
        Remove,
    }
}
