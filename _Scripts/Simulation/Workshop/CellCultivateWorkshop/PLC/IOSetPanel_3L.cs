using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XFramework.Core;

public enum IOSetPanelType
{
    Type_3L,
    Type_20L,
    Type_200L
}
public class IOSetPanel_3L : MonoBehaviour
{
    public IOSetPanelType type = IOSetPanelType.Type_3L;
    private Button saveButton;
    /// <summary>
    /// PH值
    /// </summary>
    private InputField phInputField;
    /// <summary>
    /// 溶氧
    /// </summary>
    private InputField pdInputField;
    /// <summary>
    /// 搅拌速率
    /// </summary>
    private InputField speedInputField;
    /// <summary>
    /// 温度
    /// </summary>
    private InputField temInputField;
    private Transform hit;

    public UniEvent<string, string,string, string> SaveEvent = new UniEvent<string, string,string, string>();
    private void Awake()
    {
        EventDispatcher.RegisterEvent<IOSetPanelType, bool>("控制参数设置面板", ShowOrHide);
    }
    private void ShowOrHide(IOSetPanelType arg1, bool arg2)
    {
        if (type== arg1)
        {
            gameObject.SetActive(arg2);
        }
    }
    private void Start()
    {
        saveButton = transform.Find("FirstBg/SecondBg/Content/Button_Save").GetComponent<Button>();
        phInputField = transform.Find("FirstBg/SecondBg/Content/ContentData/ph/InputField_Num").GetComponent<InputField>();
        pdInputField = transform.Find("FirstBg/SecondBg/Content/ContentData/pd/InputField_Num").GetComponent<InputField>();
        speedInputField = transform.Find("FirstBg/SecondBg/Content/ContentData/speed/InputField_Num").GetComponent<InputField>();
        temInputField = transform.Find("FirstBg/SecondBg/Content/ContentData/tem/InputField_Num").GetComponent<InputField>();
        hit = transform.Find("FirstBg/SecondBg/Hit");

        saveButton.onClick.AddListener(SaveButton_onClick);
    }
    /// <summary>
    /// 保存方法
    /// </summary>
    private void SaveButton_onClick()
    {
        Hide();
        SaveEvent.Invoke(phInputField.text, pdInputField.text, speedInputField.text, temInputField.text);
        EventDispatcher.ExecuteEvent<string, string, string, string>("参数设置完成", phInputField.text, pdInputField.text, speedInputField.text, temInputField.text);
        EventDispatcher.ExecuteEvent(Events.PLC.PLCToWorkshop, type);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        EventDispatcher.UnregisterEvent<IOSetPanelType, bool>("控制参数设置面板", ShowOrHide);
    }
}


