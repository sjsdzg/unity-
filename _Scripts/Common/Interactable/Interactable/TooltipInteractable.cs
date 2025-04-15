using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using XFramework.Common;

public class TooltipInteractable : MonoBehaviour
{

    [TextArea]
    public string text = "";

    private void Start()
    {
        //指针进入
        this.TriggerAction(EventTriggerType.PointerEnter, eventData =>
        {
            Task.NewTask()
            .Append(new TooltipAction(true, text))
            .Execute();
        });
        //指针退出
        this.TriggerAction(EventTriggerType.PointerExit, eventData =>
        {
            Task.NewTask()
            .Append(new TooltipAction(false))
            .Execute();
        });
    }
}
