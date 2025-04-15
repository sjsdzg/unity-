using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;

namespace XFramework.Actions
{
    public class UpdateTagAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// 标签状态
        /// </summary>
        private TagState State;

        public UpdateTagAction(GameObject _gameObject, TagState _state)
        {
            gameObject = _gameObject;
            State = _state;
        }

        public override void Execute()
        {
            if (gameObject != null)
            {
                TagComponent m_Tag = gameObject.GetOrAddComponent<TagComponent>();
                m_Tag.State = State;
                Completed();
            }
        }
    }
}
