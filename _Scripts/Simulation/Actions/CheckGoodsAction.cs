using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using XFramework.Module;
using XFramework.UI;
using XFramework.Common;

namespace XFramework.Simulation
{
    /// <summary>
    /// 检查物品类型Action
    /// </summary>
    public class CheckGoodsAction : ActionBase
    {
        /// <summary>
        /// eventData
        /// </summary>
        public PointerEventData eventData;

        /// <summary>
        /// 物品类型
        /// </summary>
        public GoodsType equalType;

        public CheckGoodsAction(PointerEventData _eventData, GoodsType _equalType)
        {
            eventData = _eventData;
            equalType = _equalType;
        }

        public override void Execute()
        {
            ProductionItemElement component = eventData.pointerDrag.GetComponent<ProductionItemElement>();
            if (component != null)
            {
                //判断物品
                Goods item = component.Item as Goods;
                if (item.GoodsType == equalType)
                {
                    //Completed
                    Completed();
                }
                else
                {
                    Error(new System.Exception("物品类型错误"));
                }
            }
            else
            {
                Error(new System.Exception("the dragged GameObject is not Goods"));
            }
        }
    }
}

