using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using XFramework.Module;
using XFramework.UI;
using XFramework.Common;
using XFramework.Component;

namespace XFramework.Actions
{
    /// <summary>
    /// 检查标签类型Action
    /// </summary>
    public class CheckTagAction : ActionBase
    {
        /// <summary>
        /// gameObject
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 标签类型
        /// </summary>
        public TagState equalType;

        /// <summary>
        /// 是否匹配
        /// </summary>
        public bool isMatch;

        public CheckTagAction(GameObject _gameObject, TagState _equalType, bool _isMatch = true)
        {
            gameObject = _gameObject;
            equalType = _equalType;
            isMatch = _isMatch;
        }

        public override void Execute()
        {
            TagComponent component = gameObject.GetOrAddComponent<TagComponent>();
            if (component != null)
            {
                //判断标签类型
                if ((component.State == equalType) == isMatch)
                {
                    //Completed
                    Completed();
                }
                else
                {
                    Error(new System.Exception("检查标签类型时,结果不匹配"));
                }
            }
            else
            {
                Error(new System.Exception("TagComponent is null"));
            }
        }
    }
}

