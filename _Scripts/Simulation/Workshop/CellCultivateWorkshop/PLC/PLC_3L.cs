using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework;
using XFramework.Core;

public class PLC_3L : MonoBehaviour
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
    private void Awake()
    {
        EventDispatcher.RegisterEvent<string>("3L培养罐液位参数变化", liquidTextChange_callBack);
    }

    private void liquidTextChange_callBack(string obj)
    {
        liquidText.text = obj;
    }

    private void Start()
    {
        phText = transform.Find("backGround/parameterSetBar/pHText").GetComponent<Text>();
        pdText = transform.Find("backGround/parameterSetBar/pDText").GetComponent<Text>();
        liquidText = transform.Find("backGround/parameterSetBar/liquidText").GetComponent<Text>();
        temText = transform.Find("backGround/parameterSetBar/temText").GetComponent<Text>();
        speedText = transform.Find("backGround/parameterSetBar/speedText").GetComponent<Text>();
        iOSetPanel_3L = transform.Find("backGround/parameterSetBar/Panel_I_OSet").GetComponent<IOSetPanel_3L>();
        iOSetPanel_3L.SaveEvent.AddListener(SaveEvent_callBack);
        iOSetPanel_3L.Hide();
        Hide();
    }

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
        EventDispatcher.UnregisterEvent<string>("3L培养罐液位参数变化", liquidTextChange_callBack);
    }
}


