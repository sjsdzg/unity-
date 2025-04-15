using UnityEngine;
using UnityEngine.Events;
using XFramework.Common;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.Actions
{
    public class InvokeFocusAction : ActionBase
    {
        private string focusComName;
        
        /// <summary>
        /// 是否立刻返回完成
        /// </summary>
        public bool immediate;

        public InvokeFocusAction(string _focusComName, bool _immediate = true)
        {
            focusComName = _focusComName;
            immediate = _immediate;
        }
        public override void Execute()
        {
            UsableComponent[] usableComponents = Object.FindObjectsOfType<UsableComponent>();
            foreach (var item in usableComponents)
            {
                if (item.IsUsing)
                {
                    item.IsUsing = false;
                }
            }
            FocusManager.Instance.InvokeFocusAction(focusComName, () =>
            {
                if (!immediate)
                {
                    Completed();
                }
            });
            if (immediate)
            {
                Completed();
            }
            //Camera.main.GetComponent<CameraSwitcher>().enabled = false;
            //EventDispatcher.ExecuteEvent(Events.Prompt.Hide);
            //Camera.main.GetComponent<MouseOrbit>().enabled = false;
        }
    }
}
