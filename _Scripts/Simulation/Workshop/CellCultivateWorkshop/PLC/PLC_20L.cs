using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework;
using XFramework.PLC;
using XFramework.Core;
using XFramework.Simulation;
using DG.Tweening;

public class PLC_20L : MonoBehaviour
{
    /// <summary>
    /// PH值
    /// </summary>
    private Text phText;
    /// <summary>
    /// 溶氧
    /// </summary>
    private Text pdText;
    /// <summary>
    /// 液位
    /// </summary>
    private Text liquidText;
    /// <summary>
    /// 温度
    /// </summary>
    private Text temText;
    /// <summary>
    /// 搅拌速率
    /// </summary>
    private Text speedText;

    private IOSetPanel_3L iOSetPanel_3L;
    /// <summary>
    /// 加入培养基数量输入框
    /// </summary>
    private InputField addSumInputField;


    /// <summary>
    /// PLC 元件
    /// </summary>
    private PLCPipeFittingManager m_PLCPipeFitting;
    private Image fadeImage;
    private void Awake()
    {
        EventDispatcher.RegisterEvent<string>("20L培养罐阀门点击前", ValveBeforeEvent);
        EventDispatcher.RegisterEvent<string>("200L培养罐进料结束", FeedstockEnd_callBack);
        EventDispatcher.RegisterEvent<string>("20L培养罐液位参数变化", liquidTextChange_callBack);
        EventDispatcher.RegisterEvent<IOSetPanelType>("设置培养罐加入培养基参数", SetAddSumInputFieldEvent);
    }

    private void SetAddSumInputFieldEvent(IOSetPanelType arg1)
    {
        if (arg1.Equals(iOSetPanel_3L.type))
        {
            addSumInputField.interactable = true;
            fadeImage.transform.gameObject.SetActive(true);
            fadeImage.DOColor(Color.red, 0.2f).SetLoops(-1,LoopType.Yoyo);
            addSumInputField.onValueChanged.AddListener(x =>
            {
                if (addSumInputField.text.Equals("15"))
                {
                    DOTween.Kill(fadeImage);
                    fadeImage.transform.gameObject.SetActive(false);
                    addSumInputField.interactable = false;
                    EventDispatcher.ExecuteEvent<IOSetPanelType, string>("设置培养罐加入培养基参数结束", arg1, addSumInputField.text);
                }
            });
        }
    }

    private void liquidTextChange_callBack(string obj)
    {
        liquidText.text = obj;
    }

    private void FeedstockEnd_callBack(string obj)
    {
        m_PLCPipeFitting.InvokeFitting(obj, false);
    }

    private void Start()
    {
        phText = transform.Find("backGround/parameterSetBar/pHText").GetComponent<Text>();
        pdText = transform.Find("backGround/parameterSetBar/pDText").GetComponent<Text>();
        liquidText = transform.Find("backGround/parameterSetBar/liquidText").GetComponent<Text>();
        temText = transform.Find("backGround/parameterSetBar/temText").GetComponent<Text>();
        speedText = transform.Find("backGround/parameterSetBar/speedText").GetComponent<Text>();
        iOSetPanel_3L = transform.Find("backGround/parameterSetBar/Panel_I_OSet").GetComponent<IOSetPanel_3L>();
        addSumInputField = transform.Find("backGround/parameterSetBar/addSumInputField").GetComponent<InputField>();
        iOSetPanel_3L.SaveEvent.AddListener(SaveEvent_callBack);
        m_PLCPipeFitting = transform.Find("backGround/PLCValue").GetComponent<PLCPipeFittingManager>();
        m_PLCPipeFitting.Init("Simulation/Stage/细胞培养/细胞培养_流体配置表.xml", m_PLCPipeFitting.transform);
        fadeImage = transform.Find("backGround/parameterSetBar/fadeImage").GetComponent<Image>();
        m_PLCPipeFitting.ValveAfterEvent.AddListener(ValveAfterEvent);
        CoroutineManager.Instance.Invoke(2, () =>
        {
            Hide();
            iOSetPanel_3L.Hide();
        });

    }

    /// <summary>
    /// 参数设置后
    /// </summary>
    /// <param name="ph"></param>
    /// <param name="pd"></param>
    /// <param name="liquid"></param>
    /// <param name="tem"></param>
    private void SaveEvent_callBack(string ph,string pd,string speed, string tem)
    {
        CoroutineManager.Instance.Invoke(10, () =>
        {
            phText.text = ph;
            pdText.text = pd;
            speedText.text = speed;
            temText.text = tem;          
        });
    }
    private void ValveBeforeEvent(string name)
    {
        m_PLCPipeFitting.InvokeFitting(name);
    }
    /// <summary>
    /// 8s后，2D阀门自动关闭
    /// </summary>
    /// <param name="name"></param>
    private void ValveAfterEvent(string name)
    {
        PipeFittingManager.Instance.InvokeFitting(name);
        string nextName = name.Substring(0, name.LastIndexOf("-")) + "-2";
        CoroutineManager.Instance.Invoke(8, () =>
        {
            m_PLCPipeFitting.InvokeFitting(nextName,false);
            PipeFittingManager.Instance.InvokeFitting(nextName);
        });
        EventDispatcher.ExecuteEvent<string>("2D阀门自动关闭", nextName);
        if (name.Equals("2-2-1-1"))
        {
            EventDispatcher.ExecuteEvent("20L罐加入培养基");
        }
    }
    public void ResetParameter()
    {
        m_PLCPipeFitting.CloseAllFitting();
        DOTween.Kill(fadeImage);
        fadeImage.transform.gameObject.SetActive(false);
        addSumInputField.interactable = false;
        addSumInputField.text = "";
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        EventDispatcher.UnregisterEvent<string>("20L培养罐阀门点击前", ValveBeforeEvent);
        EventDispatcher.UnregisterEvent<string>("200L培养罐进料结束", FeedstockEnd_callBack);
        EventDispatcher.UnregisterEvent<string>("20L培养罐液位参数变化", liquidTextChange_callBack);
        EventDispatcher.UnregisterEvent<IOSetPanelType>("设置培养罐加入培养基参数", SetAddSumInputFieldEvent);
    }
}


