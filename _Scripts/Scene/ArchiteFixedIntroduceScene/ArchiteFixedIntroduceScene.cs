using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Simulation;
using XFramework.Common;
using XFramework.Core;
using XFramework.Actions;
using XFramework.Component;
using XFramework.Module;
public partial class ArchiteFixedIntroduceScene : MonoBehaviour
{
    private void Awake()
    {
        InitComponent();
        EventDispatcher.RegisterEvent<string>(Events.ArchiteIntroduce.UIToScene, ReceiveUIToScene_callBack);
    }
    private void ReceiveUIToScene_callBack(string obj)
    {
        switch (obj)
        {
            case "1-1":
                Produce_1_1_Completed();
                break;         
            case "1-2-1":
                Produce_1_2();
                break;
            case "1-2":
                Produce_1_2_Completed();
                break;
            case "1-3-1":
                Produce_1_3();
                break;
            case "1-3":
                Produce_1_3_Completed();
                break;
            case "1-4-1":
                Produce_1_4();
                break;
            case "1-4":
                Produce_1_4_Completed();
                break;
            case "1-5-1":
                Produce_1_5();
                break;     
            default:
                break;
        }
    }
    private void Start()
    {
        Produce_1_1();
    }



    private void OnDestroy()
    {
        EventDispatcher.UnregisterEvent<string>(Events.ArchiteIntroduce.UIToScene, ReceiveUIToScene_callBack);
    }
}

