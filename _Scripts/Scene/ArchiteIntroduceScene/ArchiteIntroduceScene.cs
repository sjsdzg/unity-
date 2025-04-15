using UnityEngine;
using XFramework.Core;

public partial class ArchiteIntroduceScene : MonoBehaviour
{
    private void Awake()
    {
        InitComponent();
        InitOperate();
        EventDispatcher.RegisterEvent<string>(Events.ArchiteIntroduce.UIToScene, ReceiveUIToScene_callBack);
    }

    private void ReceiveUIToScene_callBack(string obj)
    {
        switch (obj)
        {
            case "1-1":
                Produce_1_1_Completed();
                break;
            case "1-2":
                Produce_1_2();
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
            case "1-5":
                Produce_1_5_Completed();
                break;
            case "1-6-1":
                Produce_1_6();
                break;
            case "1-6":
                Produce_1_6_Completed();
                break;
            case "1-7-1":
                Produce_1_7();
                break;
            case "2-1-1":
                Produce_2_1();
                break;
            case "2-1":
                Produce_2_1_Completed();
                break;
            case "2-2-1":
                Produce_2_2();
                break;
            case "2-2":
                Produce_2_2_Completed();
                break;
            case "2-3-1":
                Produce_2_3();
                break;
            case "2-3":
                Produce_2_3_Completed();
                break;
            case "3-1-1":
                Produce_3_1();
                break;
            case "3-1":
                Produce_3_1_Completed();
                break;
            case "3-2-1":
                Produce_3_2();
                break;
            case "3-2":
                Produce_3_2_Completed();
                break;
            case "3-3-1":
                Produce_3_3();
                break;
            case "3-3":
               // Produce_3_3_Completed();
                break;
            case "3-4-1":
                Produce_3_4();
                break;
            case "3-4":
                Produce_3_4_Completed();
                break;
            case "4-1-1":
                Produce_4_1();
                break;
            case "4-1":
                Produce_4_1_Completed();
                break;
            case "4-2-1":
                Produce_4_2();
                break;
            case "4-2":
                Produce_4_2_Completed();
                break;
            case "4-3-1":
                Produce_4_3();
                break;
            case "4-3":
                Produce_4_3_Completed();
                break;
            case "4-4-1":
                Produce_4_4();
                break;
            case "4-4":
                Produce_4_4_Completed();
                break;
            case "5-1-1":
                Produce_5_1();
                break;
            case "5-1":
                Produce_5_1_Completed();
                break;
            case "5-2-1":
                Produce_5_2();
                break;
            case "5-2":
                Produce_5_2_Completed();
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

