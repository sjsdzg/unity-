using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class ActionItem : MonoBehaviour
    {
        /// <summary>
        /// Normal
        /// </summary>
        [SerializeField]
        private Color Normal;

        /// <summary>
        /// Highlighted
        /// </summary>
        [SerializeField]
        private Color Highlighted;
        /// <summary>
        /// Disabled
        /// </summary>
        [SerializeField]
        private Color Disabled;

        /// <summary>
        /// 动作视图
        /// </summary>
        public ActionContent Parent { get; private set; }

        /// <summary>
        /// 动作数据
        /// </summary>
        public _Action data { get; private set; }

        /// <summary>
        /// 文本
        /// </summary>
        private Text text;

        private ActionState state = ActionState.Unfinished;
        /// <summary>
        /// 状态
        /// </summary>
        public ActionState State
        {
            get { return state; }
            set
            {
                state = value;
                switch (state)
                {
                    case ActionState.Unfinished:
                        transform.GetComponent<Image>().color = Normal;
                        text.color = Color.black;
                        break;
                    case ActionState.Doing:
                        transform.GetComponent<Image>().color = Highlighted;
                        text.color = Color.black;
                        break;
                    case ActionState.Finished:
                        transform.GetComponent<Image>().color = Normal;
                        text.color = Disabled;
                        break;
                    default:
                        break;
                }
            }
        }

        void Awake()
        {
            text = transform.GetComponentInChildren<Text>();
            State = ActionState.Unfinished;
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="parameter"></param>
        public void SetValue(ActionContent view, Sequence seq, _Action _data)
        {
            Parent = view;
            data = _data;
            text.text = seq.ID + "." + _data.ID + " " + _data.ShortDesc;
        }
    }

    public enum ActionState
    {
        /// <summary>
        /// 未完成
        /// </summary>
        Unfinished,
        /// <summary>
        /// 正在进行
        /// </summary>
        Doing,
        /// <summary>
        /// 完成
        /// </summary>
        Finished,
    }
}
