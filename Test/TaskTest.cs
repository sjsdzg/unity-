using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using XFramework.Common;

public class TaskTest : MonoBehaviour
{
    Task task;
    // Use this for initialization
    void Start()
    {
        ////指针进入
        //this.TriggerAction(EventTriggerType.PointerEnter, eventData =>
        //{
        //    task = Task.NewTask();
        //    task.Append(new TooltipAction(true, "你好"))
        //    .Execute();
        //});
        ////指针退出
        //this.TriggerAction(EventTriggerType.PointerExit, eventData =>
        //{
        //    Task.NewTask()
        //    .Append(new TooltipAction(false))
        //    .Execute();
        //});

        this.AddTask(EventTriggerType.PointerEnter)
            .Append(new TooltipAction(true, "你好"));

        this.AddTask()
            .Append(new TooltipAction(false));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //task.Execute();
        }
    }
}
