
using UnityEngine;
using XFramework.Simulation;
using XFramework.Common;
using XFramework.Core;
using XFramework.Actions;
using XFramework;

public partial class ArchiteFixedIntroduceScene 
{
    private bool isCompleted_1_1;
    private bool isCompleted_1_1_1;
    private bool isCompleted_1_2;
    private bool isCompleted_1_2_1;
    private void Produce_1_1()
    {      
        Task.NewTask()
           .Append(new DelayedAction(0.5f))
           .Append(new SetCameraPositionAction(new Vector3(2.96206f, 20, 4.555766f), 8f))
           .Append(new GameObjectAction(firstMan,true))
           .Append(new UpdateTwinkleAction(hightRooms[0].gameObject, true))
           .OnCompleted(() =>
           {
               EventDispatcher.ExecuteEvent<int, int>(Events.Procedure.Current, 1, 1);
               CoroutineManager.Instance.Invoke(5, () =>
               {
                   MessageBoxEx.Show("前往二楼...", "提示", MessageBoxExEnum.SingleDialog, X =>
                   {
                       isCompleted_1_1 = true;
                       Task.NewTask()
                          .Append(new CheckBoolAction(isCompleted_1_1_1, true))
                          .Append(new GameObjectAction(firstMan, false))
                          .Append(new UpdateTwinkleAction(hightRooms[0].gameObject, false))
                          .Append(new GameObjectAction(secondFloow, true))
                          .Append(new GameObjectAction(secondMan, true))
                          .Append(new SetCameraPositionAction(new Vector3(4.110145f, 20, 6.50235f), 9f))
                          .Append(new UpdateTwinkleAction(secondMan.gameObject, true))
                          .Append(new DelayedAction(3))
                          .Append(new UpdateTwinkleAction(secondMan.gameObject, false))
                          .Append(new GameObjectAction(secondFloow, false))
                         .Append(new GameObjectAction(secondMan, false))
                          .OnCompleted(() =>
                          {
                              EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-2");
                          })
                          .Execute();
                       Task.NewTask()
                          .Append(new CheckBoolAction(isCompleted_1_1_1, false))
                          .Append(new GameObjectAction(firstMan, false))
                          .Append(new UpdateTwinkleAction(hightRooms[0].gameObject, false))
                          .Append(new GameObjectAction(secondFloow, true))
                          .Append(new GameObjectAction(secondMan, true))
                          .Append(new SetCameraPositionAction(new Vector3(4.110145f, 20, 6.50235f), 9f))
                          .Append(new UpdateTwinkleAction(secondMan.gameObject, true))
                          .Execute();

                   });
               });             
           })
           .Execute();
    }
    private void Produce_1_1_Completed()
    {
        if (isCompleted_1_1)
        {
            Task.NewTask()
             .Append(new UpdateTwinkleAction(secondMan.gameObject, false))
             .Append(new GameObjectAction(secondFloow, false))
             .Append(new GameObjectAction(secondMan, false))
             .OnCompleted(() =>
             {
                 EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-2");
             })
            .Execute();
        }
        else
        {
            isCompleted_1_1_1 = true;
        }
    }
    private void Produce_1_2()
    {
        Task.NewTask()           
            .Append(new SetCameraPositionAction(new Vector3(3.10943f, 20, 64.24834f), 8f))
            .Append(new UpdateTwinkleAction(hightRooms[1].gameObject, true))
            .Append(new DelayedAction(5))
            .OnCompleted(() =>
            {
                MessageBoxEx.Show("前往二楼...", "提示", MessageBoxExEnum.SingleDialog, X =>
                {
                    isCompleted_1_2 = true;


                    Task.NewTask()
                        .Append(new CheckBoolAction(isCompleted_1_2_1,false))
                        .Append(new UpdateTwinkleAction(hightRooms[1].gameObject, false))
                        .Append(new GameObjectAction(secondFloow, true))
                        .Append(new GameObjectAction(car01, true))
                        .Append(new SetCameraPositionAction(new Vector3(6.51593f, 20, 63.36759f), 9f))
                        .Append(new UpdateTwinkleAction(car01.gameObject, true))                  
                        .Execute();
                    Task.NewTask()
                       .Append(new CheckBoolAction(isCompleted_1_2_1, true))
                       .Append(new UpdateTwinkleAction(hightRooms[1].gameObject, false))
                       .Append(new GameObjectAction(secondFloow, true))
                       .Append(new GameObjectAction(car01, true))
                       .Append(new SetCameraPositionAction(new Vector3(6.51593f, 20, 63.36759f), 9f))
                       .Append(new UpdateTwinkleAction(car01.gameObject, true))
                       .Append(new DelayedAction(3))
                       .Append(new UpdateTwinkleAction(car01.gameObject, false))
                       .Append(new GameObjectAction(car01, false))
                       .OnCompleted(() =>
                       {
                           EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-3");
                       })
                       .Execute();
                });
            })
            .Execute();
    }
    private void Produce_1_2_Completed()
    {
        if (isCompleted_1_2)
        {
            Task.NewTask()
               .Append(new UpdateTwinkleAction(car01.gameObject, false))
               .Append(new GameObjectAction(car01, false))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-3");
               })
               .Execute();
        }
        else
        {
            isCompleted_1_2_1 = true;
        }
    }
    private void Produce_1_3()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(3.661548f, 20, 5.754135f), 9f))
            .Append(new UpdateTwinkleAction(hightRooms[2].gameObject, true))
            .Append(new GameObjectAction(escapeRoom, true))
            .Append(new UpdateTwinkleAction(escapeRoom.gameObject, true))          
            .Execute();
    }
    private void Produce_1_3_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightRooms[2].gameObject, false))
            .Append(new UpdateTwinkleAction(escapeRoom.gameObject, false))
            .Append(new GameObjectAction(escapeRoom, false))
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-4");
            })
            .Execute();
    }
    private void Produce_1_4()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(19.01444f, 20, 7.019793f), 9f))
            .Append(new UpdateTwinkleAction(hightRooms[3].gameObject, true))
            .Append(new DelayedAction(16))
            .Append(new UpdateTwinkleAction(hightRooms[3].gameObject, false))
            .Append(new SetCameraPositionAction(new Vector3(27.37646f, 20, 62.40115f), 10f))
            .Append(new UpdateTwinkleAction(hightRooms[4].gameObject, true))          
            .Execute();
    }
    private void Produce_1_4_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightRooms[4].gameObject, false))
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-5");
            })
            .Execute();
    }
    private void Produce_1_5()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(32.21206f, 20, 6.923837f), 9f))
            .Append(new UpdateTwinkleAction(hightRooms[5].gameObject, true))
            .Append(new GameObjectAction(man01))
            .Append(new GameObjectAction(man02))
            .Append(new DelayedAction(5))
            .Append(new UpdateTwinkleAction(hightRooms[5].gameObject, false))           
            .Append(new DOLocalMoveAction(man01, new Vector3 (32.53f,0.1f, 1.75f), 0.5f,true))
            .Append(new DOLocalMoveAction(man02, new Vector3(32.53f, 0.1f, 7.73f), 0.5f, false))
            .Append(new DOLocalMoveAction(man01, new Vector3(39.28f, 0.1f, 1.75f), 0.5f))
            .Append(new DOLocalMoveAction(man02, new Vector3(35.82f, 0.1f, 7.73f), 0.5f, false))
            .Append(new DOLocalMoveAction(man01, new Vector3(39.28f, 0.1f, 6.57f), 0.5f))
            .Append(new DOLocalMoveAction(man02, new Vector3(35.82f, 0.1f,4.06f), 0.5f, false))
            .Append(new DOLocalMoveAction(man01, new Vector3(38.54f, 0.1f, 6.57f), 0.5f))
            .Append(new DOLocalMoveAction(man02, new Vector3(38.5f, 0.1f, 4.06f), 0.5f, false))
            .Append(new DOLocalMoveAction(man01, new Vector3(38.54f, 0.1f, 9.19f), 0.5f))
            .Append(new DOLocalMoveAction(man02, new Vector3(38.54f, 0.1f, 9.19f), 0.5f, false))
            .Append(new DOLocalMoveAction(man01, new Vector3(34.14f, 0.1f, 9.19f), 0.5f))
            .Append(new DOLocalMoveAction(man02, new Vector3(36.23f, 0.1f, 9.19f), 0.5f, false))
            .Append(new DelayedAction(2))
            .OnCompleted(() =>
            {
                MessageBoxEx.Show("固定条件讲解结束", "提示", MessageBoxExEnum.SingleDialog, X =>
                {
                    SceneLoader.Instance.LoadSceneAsync(SceneType.ArchiteIntroduceScene);
                });
            })
            .Execute();
    }
}

