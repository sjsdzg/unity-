using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using XFramework.Core;
using XFramework.Simulation;
using XFramework.Module;

public class ProteinPurificationPLC:MonoBehaviour
{
    private Panel_IOSet I_OSet;
    private PLCValveManager valveManager;
    private int valveClickSum;
    private void Awake()
    {
        EventDispatcher.RegisterEvent<string,string>("蛋白纯化参数设置",ReceiveIOSet_Event);
        EventDispatcher.RegisterEvent<string, bool>("蛋白纯化阀门点击之前", ReceiveValveClickStart);
        EventDispatcher.RegisterEvent<int>("重置2D阀门点击次数", ResetValveClickSum);
        EventDispatcher.RegisterEvent<string, ValveState>("重置2D阀门状态", ResetValveState);
        EventDispatcher.RegisterEvent<bool,int>("重置参数设置面板", ResetPanel_IOSet);
    }

    private void ResetPanel_IOSet(bool isShow,int clickSum)
    {
        I_OSet.gameObject.SetActive(isShow);
        I_OSet.ClickButtonSaveSum = clickSum;
    }

    private void ResetValveState(string name, ValveState state)
    {
        valveManager.AutoOpenOrClosePLCValve(name, state);
    }

    private void ResetValveClickSum(int count)
    {
        valveClickSum = count;
    }

    /// <summary>
    /// 蛋白纯化阀门点击之前
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void ReceiveValveClickStart(string name, bool state)
    {
        if (state)
        {
            valveManager.OpenOrClosePLCValve(name,ValveState.ON);
        }
        else
        {
            valveManager.OpenOrClosePLCValve(name, ValveState.OFF);
        }        
    }

    private void ReceiveIOSet_Event(string arg1, string arg2)
    {
        I_OSet.gameObject.SetActive(true);
        I_OSet.SetData(arg1,arg2);
    }

    private void Start()
    {
        I_OSet = transform.Find("background/Panel_IOSet").GetComponent<Panel_IOSet>();
        I_OSet.OnClick.AddListener(I_OSet_onClick);
        valveManager = transform.Find("background/PlCValveManager").GetComponent<PLCValveManager>();
        valveManager.OnClick.AddListener(Valve_onClick);
        I_OSet.gameObject.SetActive(false);
    }

    private void Valve_onClick(string name, bool state)
    {
        valveClickSum++;
        Debug.Log("阀门点击次数 = " + valveClickSum);
        ProductionGuideManager.Instance.CloseAllGuide();
        EventDispatcher.ExecuteEvent<string, int>("蛋白纯化阀门点击之后", name, valveClickSum);
       
    }
    private void I_OSet_onClick(int arg0,string arg1,string arg2)
    {
        EventDispatcher.ExecuteEvent<int,string,string>("蛋白纯化参数设置完成", arg0, arg1, arg2);
    }

    private void OnDestroy()
    {
        EventDispatcher.UnregisterEvent<string, string>("蛋白纯化参数设置", ReceiveIOSet_Event);
        EventDispatcher.UnregisterEvent<string, bool>("蛋白纯化阀门点击之前", ReceiveValveClickStart);
        EventDispatcher.UnregisterEvent<int>("重置2D阀门点击次数", ResetValveClickSum);
        EventDispatcher.UnregisterEvent<string, ValveState>("重置2D阀门状态", ResetValveState);
        EventDispatcher.UnregisterEvent<bool,int>("重置参数设置面板", ResetPanel_IOSet);
    }

}

