using UnityEngine;
using XFramework.Simulation;
using XFramework.Common;
using XFramework.Core;
using XFramework.Actions;

public partial class ArchiteIntroduceScene 
{
    void InitOperate()
    {    
    }
    void Produce_1_1()
    {
        Task.NewTask()
            .Append(new DelayedAction(0.5f))
            .Append(new SetCameraPositionAction(new Vector3(41.89007f, 20, 54.09739f),9f))
            .Append(new GameObjectAction(hightArrows[0].gameObject, true))
            .Append(new UpdateTwinkleAction(hightArrows[0].gameObject, true))          
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<int, int>(Events.Procedure.Current, 1,1);
            })
            .Execute();
    }
    void Produce_1_1_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightArrows[0].gameObject, false))
            .Append(new GameObjectAction(hightArrows[0].gameObject, false))
            .Execute();
    }
    void Produce_1_2()
    {
        Task.NewTask()           
           .Append(new SetCameraPositionAction(new Vector3(33.67292f, 20, 38.48324f)))
           .Append(new DelayedAction(2))
           .Append(new GameObjectAction(flowRooms[0].gameObject, false))
           .Append(new UpdateTwinkleAction(hightRooms[0].gameObject, true))
           .Append(new DelayedAction(3))
           .Append(new UpdateTwinkleAction(hightRooms[0].gameObject, false))
           .Append(new GameObjectAction(flowRooms[1].gameObject, false))
           .Append(new UpdateTwinkleAction(hightRooms[1].gameObject, true))
           .Append(new DelayedAction(3))
           .Append(new UpdateTwinkleAction(hightRooms[1].gameObject, false))
           .Append(new GameObjectAction(flowRooms[2].gameObject, false))
           .Append(new UpdateTwinkleAction(hightRooms[2].gameObject, true))
           .Append(new DelayedAction(3))
           .Append(new UpdateTwinkleAction(hightRooms[2].gameObject, false))
           .Append(new GameObjectAction(flowRooms[3].gameObject, false))
           .Append(new UpdateTwinkleAction(hightRooms[3].gameObject, true))
           .Append(new DelayedAction(3))
           .Append(new UpdateTwinkleAction(hightRooms[3].gameObject, false))
           .OnCompleted(() =>
           {
               EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-3");
           })
           .Execute();
    }
    void Produce_1_3()
    {
        Task.NewTask()         
            .Append(new SetCameraPositionAction(new Vector3(46.73524f, 20, 51.31035f),11f))
            .Append(new GameObjectAction(flowRooms[11].gameObject, false))
            .Append(new UpdateTwinkleAction(hightRooms[4].gameObject, true))
            .Execute();
    }
    void Produce_1_3_Completed()
    {
        Task.NewTask()
        .Append(new UpdateTwinkleAction(hightRooms[4].gameObject, false))
        .OnCompleted(() =>
        {
            EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-4");
        })
        .Execute();
    }
    void Produce_1_4()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(27.72065f, 20, 20.96896f), 11f))
            .Append(new GameObjectAction(flowRooms[4].gameObject, false))
            .Append(new UpdateTwinkleAction(hightRooms[5].gameObject, true))
            .Append(new DelayedAction(6))
            .Append(new UpdateTwinkleAction(hightRooms[5].gameObject, false))
            .Append(new GameObjectAction(car01.gameObject))
            .Append(new InvokeTimeLineAction(car01.gameObject))
            .OnCompleted(() =>
             {
                 
             })
            .Execute();
    }
    void Produce_1_4_Completed()
    {
        Task.NewTask()
        .Append(new GameObjectAction(car01.gameObject, false))
        .Append(new GameObjectAction(car02.gameObject, false))
        .OnCompleted(() =>
        {
            EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-5");
        })
        .Execute();
    }
    void Produce_1_5()
    {
        Task.NewTask()
            .Append(new GameObjectAction(man01.gameObject))
            .Append(new GameObjectAction(man02.gameObject))
            .Append(new GameObjectAction(flowRooms[12].gameObject, false))
            .Append(new InvokeTimeLineAction(man01.gameObject))
            .OnCompleted(() =>
            {
                MessageBoxEx.Show("等待清洗...", "提示", MessageBoxExEnum.SingleDialog);
            })
            .Execute();
    }
    void Produce_1_5_Completed()
    {
        Task.NewTask()
            .Append(new GameObjectAction(man01.gameObject,false))
            .Append(new GameObjectAction(man02.gameObject,false))
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-6");
            })
            .Execute();
    }
    void Produce_1_6()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(44.86402f, 20, 8.416421f), 6f))
            .Append(new GameObjectAction(flowRooms[5].gameObject, false))
            .Append(new UpdateTwinkleAction(hightRooms[6].gameObject, true))
            .OnCompleted(() =>
            {
            })
            .Execute();
    }
    void Produce_1_6_Completed()
    {
        Task.NewTask()
           .Append(new UpdateTwinkleAction(hightRooms[6].gameObject, false))
           .OnCompleted(() =>
           {
               EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "1-7");
           })
           .Execute();
    }
    void Produce_1_7()
    {
        Task.NewTask()          
            .Append(new GameObjectAction(car03.gameObject))
            .Append(new GameObjectAction(flowRooms[13].gameObject, false))
            .Append(new InvokeTimeLineAction(car03.gameObject))
            .OnCompleted(() =>
            {
                MessageBoxEx.Show("洁净区物料存放完毕", "提示", MessageBoxExEnum.SingleDialog,X=> {

                    Task.NewTask()
                        .Append(new GameObjectAction(hopper01.gameObject))
                        .Append(new GameObjectAction(hopper01_1.gameObject))
                        .Append(new InvokeTimeLineAction(hopper01.gameObject))
                        .OnCompleted(() =>
                        {
                            MessageBoxEx.Show("领取生产所需物料", "提示", MessageBoxExEnum.SingleDialog, Y => {
                                Task.NewTask()
                                    .Append(new GameObjectAction(hopper01.gameObject,false))
                                    .Append(new GameObjectAction(hopper01_1.gameObject,false))
                                    .Append(new GameObjectAction(hopper02.gameObject, true))
                                    .Append(new GameObjectAction(hopper02_2.gameObject, true))
                                    .Append(new InvokeTimeLineAction(hopper02.gameObject))
                                    .Append(new DelayedAction(2))
                                    .Append(new GameObjectAction(hopper02.gameObject, false))
                                    .Append(new GameObjectAction(hopper02_2.gameObject, false))
                                    .Append(new GameObjectAction(car03.gameObject,false))
                                    .Append(new GameObjectAction(car04.gameObject, false))
                                    .OnCompleted(() =>
                                    {
                                        EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "2-1");
                                    })
                                    .Execute();
                            });
                            })
                        .Execute();
                });
            })
            .Execute();
    }
    /// <summary>
    /// 物流系统
    /// </summary>
    void Produce_2_1()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(44.86402f, 20, 8.416421f), 6f))
            .Append(new UpdateTwinkleAction(hightRooms[6].gameObject, true))      
            .Execute();
    }
    void Produce_2_1_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightRooms[6].gameObject, false))
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "2-2");
            })
            .Execute();
    }
    void Produce_2_2()
    {
        Task.NewTask()           
            .Append(new SetCameraPositionAction(new Vector3(44.86402f, 20, 8.416421f), 6f))
            .Append(new UpdateTwinkleAction(hightRooms[7].gameObject, true))
            .Execute();
    }
    void Produce_2_2_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightRooms[7].gameObject, false))
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "2-3");
            })
            .Execute();
    }
    void Produce_2_3()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(44.86402f, 20, 8.416421f), 6f))
            .Append(new UpdateTwinkleAction(hightRooms[8].gameObject, true))
            .Execute();
    }
    void Produce_2_3_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightRooms[8].gameObject, false))
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "3-1");
            })
            .Execute();
    }
    void Produce_3_1()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(48.14163f, 20, 5.706283f),9f))
            .Append(new GameObjectAction(hightArrows[1].gameObject, true))
            .Append(new UpdateTwinkleAction(hightArrows[1].gameObject, true))
            .Execute();
    }
    void Produce_3_1_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightArrows[1].gameObject, false))
            .Append(new GameObjectAction(hightArrows[1].gameObject, false))
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "3-2");
            })
            .Execute();
    }
    void Produce_3_2()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(25.39483f, 20, 47.85699f),9f))
            .Append(new GameObjectAction(flowRooms[6].gameObject, false))
            .Append(new UpdateTwinkleAction(hightRooms[9].gameObject, true))
            .Append(new DelayedAction(3))
            .Append(new UpdateTwinkleAction(hightRooms[9].gameObject, false))
            .Append(new SetCameraPositionAction(new Vector3(28.84311f, 20, 11.93409f),9f))
            .Append(new GameObjectAction(flowRooms[7].gameObject, false))
            .Append(new UpdateTwinkleAction(hightRooms[10].gameObject, true))
            .Append(new DelayedAction(3))
            .Append(new UpdateTwinkleAction(hightRooms[10].gameObject, false))
            .Append(new SetCameraPositionAction(new Vector3(43.99525f, 20, 9.925976f),9f))
            .Append(new GameObjectAction(flowRooms[8].gameObject, false))
            .Append(new UpdateTwinkleAction(hightRooms[11].gameObject, true))
            .Execute();
    }
    void Produce_3_2_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightRooms[11].gameObject, false))
            .Execute();
        EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "3-3");
    }
    void Produce_3_3()
    {
        Task.NewTask()
           .Append(new GameObjectAction(flowRooms[14].gameObject, false))
           .Execute();
        m_ArchiteManController.Show();
    }
    void Produce_3_4()
    {
        m_ArchiteManController.Hide();
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(38.04709f, 20, 12.30922f), 11f))
            .Append(new GameObjectAction(dirtyMan.gameObject, true))
            .Append(new GameObjectAction(flowRooms[15].gameObject, false))
            .Append(new DOLocalMoveAction(dirtyMan,new Vector3 (46.57f,0.1f,9.13f),1f))
            .Append(new DOLocalMoveAction(dirtyMan, new Vector3(46.57f, 0.1f, 5.98f), 0.5f))
            .Append(new DOLocalMoveAction(dirtyMan, new Vector3(33.19f, 0.1f, 5.98f), 1.5f))
            .Append(new DOLocalMoveAction(dirtyMan, new Vector3(33.19f, 0.1f, 8.58f), 0.5f))
            .Append(new ShowSlideRunAction(m_Archite_SlideRun,"清洗中...","清洗完毕",true))
           // .Append(new GameObjectAction(dirtyMan.gameObject,false))
            .Append(new DelayedAction(1))
            .Append(new GameObjectAction(cleanMan01.gameObject, true))
            .Append(new ShowSlideRunAction(m_Archite_SlideRun, "灭菌中...", "灭菌完毕", true))
            .Append(new DOLocalMoveAction(cleanMan01,new Vector3 (39.66f,0.1f, 8.58f),0.2f))
            .Append(new DOLocalMoveAction(cleanMan01, new Vector3(39.66f, 0.1f, 8.58f), 0.2f))
            .Append(new DOLocalMoveAction(cleanMan01, new Vector3(39.66f, 0.1f, 12.98f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan01, new Vector3(31.67f, 0.1f, 12.98f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan01, new Vector3(31.67f, 0.1f, 10.1f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan01, new Vector3(27.05f, 0.1f, 10.1f), 0.5f))
            .Append(new SetCameraPositionAction(new Vector3(38.04709f, 20, 12.30922f), 11f))
            .Append(new GameObjectAction(cleanMan02.gameObject, true))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(39.66f, 0.1f, 8.58f), 0.2f))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(39.66f, 0.1f, 8.58f), 0.2f))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(39.66f, 0.1f, 12.98f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(41.66f, 0.1f, 12.98f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(41.66f, 0.1f, 11.27f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(44.97f, 0.1f, 11.27f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(44.97f, 0.1f, 5.84f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(46.5f, 0.1f, 5.84f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(46.5f, 0.1f, 10.21f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan02, new Vector3(50.06f, 0.1f, 10.21f), 0.5f))
            .Append(new GameObjectAction(cleanMan03.gameObject, true))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(39.66f, 0.1f, 8.58f), 0.2f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(39.66f, 0.1f, 8.58f), 0.2f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(39.66f, 0.1f, 12.98f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(31.67f, 0.1f, 12.98f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(31.67f, 0.1f, 10.1f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(27.05f, 0.1f, 10.1f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(17.81f, 0.1f, 10.1f), 1f,true))
            .Append(new DOLocalMoveAction(Camera.main.transform, new Vector3(22.10506f, 20f, 15.08f), 1f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(17.81f, 0.1f, 55.52f), 5f,true))
            .Append(new DOLocalMoveAction(Camera.main.transform, new Vector3(22.10506f, 20f, 53.88f), 5f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(19.65f, 0.1f, 55.52f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(19.65f, 0.1f, 53.97f), 0.5f))
            .Append(new DOLocalMoveAction(cleanMan03, new Vector3(23.85f, 0.1f, 53.97f), 0.5f))
            .Append(new DelayedAction(2))
            .Append(new GameObjectAction(cleanMan01.gameObject, true))
            .Append(new GameObjectAction(cleanMan02.gameObject, true))
            .Append(new GameObjectAction(cleanMan03.gameObject, true))
            .OnCompleted(() =>
            {
                EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "4-1");
            })
            .Execute();
    }
    void Produce_3_4_Completed()
    {
       
    }
    void Produce_4_1()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(28.84311f, 20, 11.93409f), 9f))
            .Append(new UpdateTwinkleAction(hightRooms[10].gameObject, true))
            .Execute();
    }
    void Produce_4_1_Completed()
    {
        Task.NewTask()
           .Append(new UpdateTwinkleAction(hightRooms[10].gameObject, false))
           .Execute();
        EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "4-2");
    }
    void Produce_4_2()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(28.84311f, 20, 11.93409f), 9f))
            .Append(new UpdateTwinkleAction(hightRooms[12].gameObject, true))
            .Execute();
    }
    void Produce_4_2_Completed()
    {
        Task.NewTask()
          .Append(new UpdateTwinkleAction(hightRooms[12].gameObject, false))
          .Execute();
        EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "4-3");
    }
    void Produce_4_3()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(28.84311f, 20, 11.93409f), 9f))
            .Append(new UpdateTwinkleAction(hightRooms[13].gameObject, true))
            .Execute();
    }
    void Produce_4_3_Completed()
    {
        Task.NewTask()
           .Append(new UpdateTwinkleAction(hightRooms[13].gameObject, false))
           .Execute();
        EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "4-4");
    }
    void Produce_4_4()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(28.84311f, 20, 11.93409f), 9f))
            .Append(new UpdateTwinkleAction(hightRooms[14].gameObject, true))
            .Execute();
    }
    void Produce_4_4_Completed()
    {
        Task.NewTask()
           .Append(new UpdateTwinkleAction(hightRooms[14].gameObject, false))
           .Execute();
        EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "5-1");
    }
    void Produce_5_1()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(28.84311f, 20, 11.93409f), 9f))
            .Append(new GameObjectAction(flowRooms[9].gameObject,false))
            .Append(new UpdateTwinkleAction(hightRooms[15].gameObject, true))
            .Execute();
    }
    void Produce_5_1_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightRooms[15].gameObject, false))
            .Execute();
        EventDispatcher.ExecuteEvent<string>(Events.ArchiteIntroduce.SceneToUI, "5-2");
    }
    void Produce_5_2()
    {
        Task.NewTask()
            .Append(new SetCameraPositionAction(new Vector3(28.84311f, 20, 11.93409f), 9f))
            .Append(new GameObjectAction(flowRooms[10].gameObject, false))
            .Append(new UpdateTwinkleAction(hightRooms[16].gameObject, true))
            .Execute();
    }
    void Produce_5_2_Completed()
    {
        Task.NewTask()
            .Append(new UpdateTwinkleAction(hightRooms[16].gameObject, false))
            .Append(new DelayedAction(2))
            .OnCompleted(() =>
            {
                MessageBoxEx.Show("范例讲解结束", "提示", MessageBoxExEnum.SingleDialog, X =>
                {
                    SceneLoader.Instance.LoadSceneAsync(SceneType.ArchitectScene);
                });
            })
            .Execute();
    }
}

