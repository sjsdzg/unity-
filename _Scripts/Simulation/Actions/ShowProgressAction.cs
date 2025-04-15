using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    public class ShowProgressAction : ActionBase
    {
        /// <summary>
        /// 显示内容
        /// </summary>
        public string content;

        /// <summary>
        /// 持续时间
        /// </summary>
        private float duration;
        /// <summary>
        /// 是否转完100才完成Action
        /// </summary>
        private bool completed;

        public ShowProgressAction(string _content, float _duration, bool _isCompleted = false)
        {
            content = _content;
            duration = _duration;
            completed = _isCompleted;           
        }

        public override void Execute()
        {
            if (ProgressPanel.Instance != null)
            {
				ProgressPanel.Instance.OnCompleted.RemoveAllListeners ();
                if (completed)
                {
					ProgressPanel.Instance.OnCompleted.AddListener(ProgressPanel_OnCompleted);
                    ProgressPanel.Instance.Show(content, duration);
                }
                else
                {
                    ProgressPanel.Instance.Show(content, duration);
                    Completed();
                }

            }
            else
            {
                Error(new Exception("ProgressPanel is null"));
            }
        }
        private void ProgressPanel_OnCompleted()
        {
            ProgressPanel.Instance.OnCompleted.RemoveAllListeners();
            Completed();
        }
    }
}
