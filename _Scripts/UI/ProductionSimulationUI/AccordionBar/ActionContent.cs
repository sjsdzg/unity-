using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    public class ActionContent : MonoBehaviour
    {
        /// <summary>
        /// 动作组件列表
        /// </summary>
        private List<ActionItem> actionComponets;

        /// <summary>
        /// 默认右键菜单项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;

        void Awake()
        {
            actionComponets = new List<ActionItem>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 添加动作Item
        /// </summary>
        /// <param name="_action"></param>
        public void AddActionItem(Sequence seq, _Action _action)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            ActionItem _ActionItem = obj.GetComponent<ActionItem>();

            if (_ActionItem != null && Content != null)
            {
                Transform t = _ActionItem.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                _ActionItem.SetValue(this, seq, _action);
                actionComponets.Add(_ActionItem);
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public ActionItem GetActionItem(int actionId)
        {
            ActionItem actionItem = null;
            foreach (var item in actionComponets)
            {
                if (item.data.ID == actionId)
                {
                    actionItem = item;
                    break;
                }
            }
            return actionItem;
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void Clear()
        {
            for (int i = 0; i < actionComponets.Count; i++)
            {
                ActionItem item = actionComponets[i];
                Destroy(item.gameObject);
            }

            actionComponets.Clear();
        }
    }
}
