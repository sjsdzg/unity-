using UnityEngine;
using UnityEngine.Events;
using XFramework.Common;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.Actions
{
    public class CloseFocusAction : ActionBase
    {
        public override void Execute()
        {
            UsableComponent[] usableComponents = Object.FindObjectsOfType<UsableComponent>();
            foreach (var item in usableComponents)
            {
                if (item.IsUsing)
                {
                    Completed();
                    return;
                }
            }
            FocusManager.Instance.ExitFocus();
            Completed();
        }
    }
}
